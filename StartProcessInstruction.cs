using System;
using System.Collections.Generic;
using System.Text;
using System.Diagnostics;
using System.Xml.Serialization;
using System.Xml;
using System.Data;
using System.Data.SQLite;

namespace Ketarin
{
    /// <summary>
    /// Represents an instruction that starts a process.
    /// </summary>
    [Serializable()]
    public class StartProcessInstruction : SetupInstruction
    {
        private SerializableDictionary<string, string> environmentVariables = new SerializableDictionary<string, string>();

        #region Properties

        /// <summary>
        /// Gets a list of environment variables to override, including the values to use.
        /// </summary>
        [XmlElement("EnvironmentVariables")]
        public SerializableDictionary<string, string> EnvironmentVariables
        {
            get
            {
                return this.environmentVariables;
            }
            set
            {
                // For serializer
                this.environmentVariables = value;
            }
        }

        public override string Name
        {
            get
            {
                return "Start process";
            }
        }

        /// <summary>
        /// File name to be executed.
        /// </summary>
        public string FileName
        {
            get; set;
        }

        /// <summary>
        /// Parameters for file execution.
        /// </summary>
        public string Parameters
        {
            get; set;
        }

        #endregion

        #region ISetupCommand Member

        public override void Execute()
        {
            string fileName = Application.Variables.ReplaceAllInString(FileName);
            string parameters = Application.Variables.ReplaceAllInString(Parameters);

            ProcessStartInfo startInfo = new ProcessStartInfo(fileName, parameters);

            foreach (var variable in EnvironmentVariables)
            {
                if (!string.IsNullOrEmpty(variable.Value))
                {
                    startInfo.EnvironmentVariables[variable.Key] = variable.Value;
                }
            }

            startInfo.CreateNoWindow = true;
            Process proc = Process.Start(startInfo);
            proc.WaitForExit();

            if (proc.ExitCode != 0)
            {
                throw new ApplicationException(string.Format("Process exited with error code {0}", proc.ExitCode));
            }
        }

        #endregion

        public override string ToString()
        {
            if (string.IsNullOrEmpty(Parameters))
            {
                return this.FileName;
            }
            else
            {
                return string.Format("Start \"{0}\" with following parameters: {1}", FileName, Parameters);
            }
        }
    }
}
