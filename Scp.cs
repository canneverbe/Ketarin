using System;
using System.Collections.Generic;
using System.Text;
using Tamir.SharpSsh.jsch;
using System.IO;
using Tamir.SharpSsh.java.util;
using System.Net;

namespace Ketarin
{
    /// <summary>
    /// Creates new ScpWebRequests based on the URI.
    /// Required format as of now: sf://user:pass@UrlToFileOnSourceForge
    /// </summary>
    public class ScpWebRequestCreator : IWebRequestCreate
    {
        #region IWebRequestCreate Member

        public WebRequest Create(Uri uri)
        {
            return new ScpWebRequest(uri);
        }

        #endregion
    }

    /// <summary>
    /// Represents an SCP download request.
    /// </summary>
    public class ScpWebRequest : WebRequest
    {
        private Uri requestUri = null;
        private int timeout = 0;
        private ScpWebResponse responseToAbort = null;

        #region Properties

        /// <summary>
        /// Gets or sets the timeout for ScpResponse.
        /// </summary>
        public override int Timeout
        {
            get
            {
                return this.timeout;
            }
            set
            {
                this.timeout = value;
            }
        }

        /// <summary>
        /// Gets the requested URI.
        /// </summary>
        public override Uri RequestUri
        {
            get
            {
                return this.requestUri;
            }
        }

        #endregion

        /// <summary>
        /// Creates a new ScpWebRquest based on the URI.
        /// </summary>
        public ScpWebRequest(Uri uri)
        {
            this.requestUri = uri;
        }

        /// <summary>
        /// Creates a new ScpWebResponse from the current request.
        /// </summary>
        /// <returns></returns>
        public override WebResponse GetResponse()
        {
            this.responseToAbort = new ScpWebResponse(this.requestUri, this.timeout);
            return this.responseToAbort;
        }

        /// <summary>
        /// Aborts the SCP request. Not supported.
        /// </summary>
        public override void Abort()
        {
            throw new NotSupportedException();
        }
    }

    /// <summary>
    /// Represents the server response of an SCP download request.
    /// </summary>
    public class ScpWebResponse : WebResponse
    {
        private Stream responseStream = null;
        private int contentLength = 0;
        private Uri responseUri = null;
        private Session session = null;

        #region Properties
        
        /// <summary>
        /// Gets the file size of the request file.
        /// </summary>
        public override long ContentLength
        {
            get
            {
                return this.contentLength;
            }
            set
            {
                throw new NotSupportedException();
            }
        }

        /// <summary>
        /// Gets the Uri of this server response.
        /// </summary>
        public override Uri ResponseUri
        {
            get
            {
                return this.responseUri;
            }
        }

        /// <summary>
        /// Gets the header information of the response.
        /// Not supported.
        /// </summary>
        public override WebHeaderCollection Headers
        {
            get
            {
                return new WebHeaderCollection();
            }
        }

        #endregion

        /// <summary>
        /// Initiates a new SCP response on sourceforge.net for downloading a file.
        /// </summary>
        /// <param name="uri">URI to download (includes username and password)</param>
        /// <param name="timeout">Timeout for this session</param>
        public ScpWebResponse(Uri uri, int timeout)
        {
            JSch jsch = new JSch();

            string[] userPass = uri.UserInfo.Split(':');
            if (userPass.Length != 2)
            {
                throw new WebException("Username and password information for sourceforge.net incomplete");
            }

            session = jsch.getSession(userPass[0], "frs.sourceforge.net", 22);

            // username and password will be given via UserInfo interface.
            //UserInfo ui = new UserInfo();
            //session.setUserInfo(ui);
            session.setPassword(userPass[1]);
            Hashtable hastable = new Hashtable();
            hastable.put("StrictHostKeyChecking", "no");
            session.setConfig(hastable);

            if (DbManager.Proxy != null)
            {
                session.setProxy(new ProxyHTTP(DbManager.Proxy.Address.Host, DbManager.Proxy.Address.Port));
            }

            try
            {
                session.connect(timeout);
            }
            catch (JSchException e)
            {
                if (e.Message == "Auth fail")
                {
                    throw new WebException("Invalid username or password for sourceforge");
                }
                throw;
            }

            // exec 'scp -f rfile' remotely
            string sfPath = GetSourceforgePath(uri.LocalPath);
            String command = "scp -f " + sfPath.Replace(" ", "\\ ");
            Channel channel = session.openChannel("exec");
            ((ChannelExec)channel).setCommand(command);

            // get I/O streams for remote scp
            Stream outs = channel.getOutputStream();
            Stream ins = channel.getInputStream();

            channel.connect();

            byte[] buf = new byte[1024];

            // send '\0'
            buf[0] = 0; outs.Write(buf, 0, 1); outs.Flush();


            int c = checkAck(ins);
            if (c != 'C')
            {
                return;
            }

            // read '0644 '
            ins.Read(buf, 0, 5);

            while (true)
            {
                ins.Read(buf, 0, 1);
                if (buf[0] == ' ') break;
                this.contentLength = this.contentLength * 10 + (buf[0] - '0');
            }

            String file = null;
            for (int i = 0; ; i++)
            {
                ins.Read(buf, i, 1);
                if (buf[i] == (byte)0x0a)
                {
                    file = Util.getString(buf, 0, i);
                    break;
                }
            }

            this.responseUri = new Uri("scp://" + session.getHost() + sfPath);
            // send '\0'
            buf[0] = 0; outs.Write(buf, 0, 1); outs.Flush();

            this.responseStream = ins;
        }
        
        /// <summary>
        /// Closes the WebResponse and the underlying connections.
        /// </summary>
        public override void Close()
        {
            if (this.responseStream != null)
            {
                // send '\0'
                byte[] buf = new byte[1024];
                buf[0] = 0;
                this.responseStream.Write(buf, 0, 1); this.responseStream.Flush();

                this.responseStream.Close();
            }

            if (this.session != null)
            {
                this.session.disconnect();
            }
        }

        /// <summary>
        /// Extracts the path of a file in the SCP file system from a HTTP download path.
        /// </summary>
        private string GetSourceforgePath(string path)
        {
            // From URL to path in SCP:
            // Look for "projects". Extract project name and built path.
            // Example: phpMyAdmin -> /p/ph/phpmyadmin
            // Look for "files": Anything after that is the remaining path

            string baseUrl = "/home/pfs/project/{0}/{1}";
            string projectPart = string.Empty;
            string filePart = string.Empty;

            string[] parts = path.Split('/');
            for (int i = 0; i < parts.Length; i++)
            {
                if (parts[i] == "projects" || parts[i] == "project")
                {
                    string projectName = parts[i + 1];
                    projectPart = projectName.Substring(0, 1) + "/" + projectName.Substring(0, 2) + "/" + projectName;
                }
                if (parts[i] == "files" || (i > 0 && parts[i-1] == "project"))
                {
                    for (int j = i + 1; j < parts.Length; j++)
                    {
                        filePart += parts[j] + "/";
                    }
                    // Might be "/download" at the end -> remove
                    filePart = filePart.Replace("/download", "").TrimEnd('/');
                }
            }

            // Possibly scheme: http://downloads.sourceforge.net/tortoisesvn/TortoiseSVN-1.6.11.20210-x64-svn-1.6.13.msi?download
            // Use location header.
            if (string.IsNullOrEmpty(projectPart) && path.Contains("downloads.sourceforge.net"))
            {
                try
                {
                    HttpWebRequest request = WebRequest.Create(path.Insert(0, "http:")) as HttpWebRequest;
                    request.AllowAutoRedirect = false;
                    WebResponse response = request.GetResponse();
                    // Remove %20 and the like
                    return GetSourceforgePath(new Uri(response.Headers["Location"]).ToString());
                }
                catch (Exception)
                {
                    // Try alternative path
                }
            }

            return string.Format(baseUrl, projectPart, filePart);
        }

        public static int checkAck(Stream ins)
        {
            int b = ins.ReadByte();
            // b may be 0 for success,
            //          1 for error,
            //          2 for fatal error,
            //          -1
            if (b == 0) return b;
            if (b == -1) return b;

            if (b == 1 || b == 2)
            {
                StringBuilder sb = new StringBuilder();
                int c;
                do
                {
                    c = ins.ReadByte();
                    sb.Append((char)c);
                }
                while (c != '\n');
                if (b == 1)
                { // error
                    Console.Write(sb.ToString());
                }
                if (b == 2)
                { // fatal error
                    Console.Write(sb.ToString());
                }
            }
            return b;
        }

        /// <summary>
        /// Returns the response stream from which to read the file.
        /// </summary>
        public override Stream GetResponseStream()
        {
            return this.responseStream;
        }
    }
}
