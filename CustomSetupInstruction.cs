using System;
using System.Collections.Generic;
using System.Text;
using System.Reflection;
using System.Windows.Forms;

namespace Ketarin
{
    /// <summary>
    /// Represents a custom setup instruction (C#) code.
    /// </summary>
    public class CustomSetupInstruction : SetupInstruction
    {
        #region Properties

        /// <summary>
        /// Gets or sets the code of the setup instruction.
        /// </summary>
        public string Code { get; set; }

        /// <summary>
        /// Gets or sets the type of command.
        /// </summary>
        public ScriptType Type { get; set; }

        public override string Name
        {
            get
            {
                return "Custom script";
            }
        }

        #endregion

        public CustomSetupInstruction()
        {
            Type = ScriptType.Batch;
        }

        public override void Execute()
        {
            if (Type == ScriptType.CS)
            {
                UserCSScript script = new UserCSScript(Code);
                script.Execute(this.Application);
            }
            else
            {
                Updater.ExecuteCommand(this.Application, Code);
            }
        }

        public override string ToString()
        {
            if (string.IsNullOrEmpty(Code))
            {
                return string.Empty;
            }

            string[] lines = Code.Split('\n');
            if (lines.Length > 0)
            {
                return lines[0];
            }

            return base.ToString();
        }
    }
}
