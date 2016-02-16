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

                // Output all information we can get.
                powerShell.Streams.Debug.DataAdded += this.DebugDataAdded;
                powerShell.Streams.Warning.DataAdded += this.WarningDataAdded;
                powerShell.Streams.Information.DataAdded += this.InfoDataAdded;
                Collection<PSObject> psOutput = powerShell.Invoke();

                foreach (PSObject outputItem in psOutput)
                {
                    // if null object was dumped to the pipeline during the script then a null
                    // object may be present here. check for null to prevent potential NRE.
                    if (outputItem != null)
                    {
                        LogDialog.Log("PowerShell: " + outputItem);
                    }
                }

                if (powerShell.HadErrors)
                {
                    StringBuilder sb = new StringBuilder();
                    foreach (ErrorRecord error in powerShell.Streams.Error)
                    {
                        sb.AppendLine(error.Exception.Message);
                    }

                    throw new ApplicationException(sb.ToString());
                }
            }
        }

        private void InfoDataAdded(object sender, DataAddedEventArgs e)
        {
            LogDialog.Log("PowerShell (info): " + ((PSDataCollection<InformationRecord>) sender)[e.Index].MessageData);
        }

        private void WarningDataAdded(object sender, DataAddedEventArgs e)
        {
            LogDialog.Log("PowerShell (warning): " + ((PSDataCollection<WarningRecord>) sender)[e.Index].Message);
        }

        private void DebugDataAdded(object sender, DataAddedEventArgs e)
        {
            LogDialog.Log("PowerShell (debug): " + ((PSDataCollection<DebugRecord>) sender)[e.Index].Message);
        }
    }
}