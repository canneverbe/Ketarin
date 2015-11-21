using System;
using System.IO;

namespace Ketarin
{
    /// <summary>
    /// Copies a file to a specified location.
    /// </summary>
    [Serializable()]
    public class CopyFileInstruction : SetupInstruction
    {
        /// <summary>
        /// Gets or sets the source file to copy.
        /// </summary>
        public string Source { get; set; }

        /// <summary>
        /// Gets or sets the target location.
        /// </summary>
        public string Target { get; set; }

        public override string Name
        {
            get
            {
                return "Copy file";
            }
        }

        public CopyFileInstruction()
        {
            // Reasonable default
            Source = "{file}";
        }

        public override void Execute()
        {
            string source = Environment.ExpandEnvironmentVariables(Application.Variables.ReplaceAllInString(Source));
            string target = Environment.ExpandEnvironmentVariables(Application.Variables.ReplaceAllInString(Target));

            // Ensure that target dir exists
            Directory.CreateDirectory(Path.GetDirectoryName(target));

            File.Copy(source, target, true);
        }

        public override string ToString()
        {
            return string.Format("Copy \"{0}\" to \"{1}\"", Source, Target);
        }
    }
}
