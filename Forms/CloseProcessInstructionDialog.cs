using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Diagnostics;

namespace Ketarin.Forms
{
    public partial class CloseProcessInstructionDialog : InstructionBaseDialog
    {
        private CloseProcessInstruction instruction = null;

        public override SetupInstruction SetupInstruction
        {
            set
            {
                CloseProcessInstruction instruction = value as CloseProcessInstruction;
                if (instruction != null)
                {
                    this.instruction = instruction;
                    cboProcessName.Text = instruction.ProcessName;
                }
            }
            get
            {
                return this.instruction;
            }
        }

        public CloseProcessInstructionDialog()
        {
            InitializeComponent();
        }

        protected override void OnLoad(EventArgs e)
        {
            if (DesignMode) return;

            base.OnLoad(e);

            // Fill list of running processes as suggestion
            try
            {
                List<string> names = new List<string>();
                foreach (Process proc in Process.GetProcesses())
                {
                    if (!names.Contains(proc.ProcessName))
                    {
                        names.Add(proc.ProcessName);
                    }
                }
                names.Sort();
                cboProcessName.Items.AddRange(names.ToArray());
            }
            catch (Exception)
            {
                // No harm done
            }
        }

        private void bOK_Click(object sender, EventArgs e)
        {
            // At least, a process name needs to be given
            if (string.IsNullOrEmpty(cboProcessName.Text))
            {
                MessageBox.Show(this, "You did not specify a process to close.\r\n\r\nSpecify a process name in order to add the setup instruction.", System.Windows.Forms.Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.None);
                DialogResult = DialogResult.None;
                return;
            }

            if (instruction == null)
            {
                this.instruction = new CloseProcessInstruction();
            }

            this.instruction.ProcessName = cboProcessName.Text;
        }
    }
}
