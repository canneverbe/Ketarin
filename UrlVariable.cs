using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Net;

namespace Ketarin
{
    class UrlVariable
    {
        private string m_Url;
        private string m_StartText;
        private string m_EndText;
        private string m_Name;
        private string m_TempContent = string.Empty;

        #region Properties

        public string Url
        {
            get { return m_Url; }
            set { m_Url = value; }
        }

        public string StartText
        {
            get { return m_StartText; }
            set { m_StartText = value; }
        }

        public string EndText
        {
            get { return m_EndText; }
            set { m_EndText = value; }
        }

        public string Name
        {
            get { return m_Name; }
            set { m_Name = value; }
        }

        public string TempContent
        {
            get { return m_TempContent; }
            set { m_TempContent = value; }
        }

        #endregion

        public UrlVariable()
        {
        }

        public UrlVariable(string name)
        {
            m_Name = name;
        }

        public void Hydrate(IDataReader reader)
        {
            m_Name = reader["VariableName"] as string;
            m_StartText = reader["StartText"] as string;
            m_EndText = reader["EndText"] as string;
            m_Url = reader["Url"] as string;
        }

        public string ReplaceInString(string url)
        {
            string find = "{" + m_Name + "}";
            // If variable is unused, don't make any efforts
            if (!url.Contains(find)) return url;

            string page = string.Empty;
            // Get the content we need to put in
            using (WebClient client = new WebClient())
            {
                page = client.DownloadString(m_Url);
            }

            // Normalise line-breaks
            page = page.Replace("\r\n", "\n");
            page = page.Replace("\r", "\n");

            int startPos = page.IndexOf(m_StartText);
            if (startPos < 0) return url;

            int endOfStart = startPos + m_StartText.Length;

            int endPos = page.IndexOf(m_EndText, endOfStart);
            if (endPos < 0) return url;

            string result = page.Substring(endOfStart, endPos - endOfStart);

            url = url.Replace(find, result);

            return url;
        }
    }
}
