using System;
using System.Collections.ObjectModel;
using System.Management.Automation;
using System.Text;
using Ketarin.Forms;

namespace Ketarin
{
    internal class PowerShellScript
    {
        private readonly string scriptText;

        public PowerShellScript(string scriptText)
        {
            this.scriptText = scriptText;
        }

        internal void Execute(ApplicationJob application)
        {
            using (PowerShell powerShell = PowerShell.Create())
            {
                powerShell.AddScript(this.scriptText);

                // Make application object available to the script.
                powerShell.Runspace.SessionStateProxy.SetVariable("app", application);
                powerShell.Runspace.SessionStateProxy.SetVariable("globalvars", UrlVariable.GlobalVariables);

                Collection<PSObject> psOutput = powerShell.Invoke();

                if (powerShell.HadErrors)
                {
                    StringBuilder sb = new StringBuilder();
                    foreach (ErrorRecord error in powerShell.Streams.Error)
                    {
                        sb.AppendLine(error.Exception.Message);
                    }

                    throw new ApplicationException(sb.ToString());
                }

                foreach (PSObject outputItem in psOutput)
                {
                    // if null object was dumped to the pipeline during the script then a null
                    // object may be present here. check for null to prevent potential NRE.
                    if (outputItem != null)
                    {
                        LogDialog.Log("PowerShell: " + outputItem);
                    }
                }

                // Output all information we can get.
                foreach (ErrorRecord warning in powerShell.Streams.Error)
                {
                    LogDialog.Log("PowerShell (error): " + warning.ErrorDetails.Message);
                }

                foreach (WarningRecord warning in powerShell.Streams.Warning)
                {
                    LogDialog.Log("PowerShell (warning): " + warning.Message);
                }

                foreach (InformationRecord info in powerShell.Streams.Information)
                {
                    LogDialog.Log("PowerShell (info): " + info.MessageData);
                }
            }
        }
    }
}