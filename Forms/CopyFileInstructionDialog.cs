using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Collections;
using System.IO;

namespace Ketarin.Forms
{
    public partial class CopyFileInstructionDialog : InstructionBaseDialog
    {
        private CopyFileInstruction instruction = null;

        public override SetupInstruction SetupInstruction
        {
            set
            {
                CopyFileInstruction instruction = value as CopyFileInstruction;
                if (instruction != null)
                {
                    this.instruction = instruction;
                    txtSource.Text = this.instruction.Source;
                    txtTarget.Text = this.instruction.Target;
                }
            }
            get
            {
                return this.instruction;
            }
        }

        public CopyFileInstructionDialog()
        {
            InitializeComponent();
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);

            txtSource.SetVariableNames(this.VariableNames);
            txtTarget.SetVariableNames(this.VariableNames);

            // Add all directories to the button
            foreach (DictionaryEntry var in Environment.GetEnvironmentVariables())
            {
                try
                {
                    if (Directory.Exists(var.Value as string))
                    {
                        MenuItem newItem = new MenuItem(var.Key as string);
                        newItem.Click += new EventHandler(EnvironmentVariableClick);
                        environmentMenu.MenuItems.Add(newItem);
                    }
                }
                catch
                {
                    continue;
                }
            }
        }

        private void EnvironmentVariableClick(object sender, EventArgs e)
        {
            txtTarget.AppendText("%" + ((MenuItem)sender).Text + "%");
        }

        private void bOK_Click(object sender, EventArgs e)
        {
            // Both information must be given
            if (string.IsNullOrEmpty(txtSource.Text))
            {
                MessageBox.Show(this, "You did not specify a source file to copy.\r\n\r\nSpecify a source file in order to add the setup instruction.", System.Windows.Forms.Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.None);
                DialogResult = DialogResult.None;
                return;
            }

            if (string.IsNullOrEmpty(txtTarget.Text))
            {
                MessageBox.Show(this, "You did not specify a target location for the file to copy.\r\n\r\nSpecify a target location in order to add the setup instruction.", System.Windows.Forms.Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.None);
                DialogResult = DialogResult.None;
                return;
            }

            if (instruction == null)
            {
                this.instruction = new CopyFileInstruction();
            }

            this.instruction.Source = txtSource.Text;
            this.instruction.Target = txtTarget.Text;
        }
    }
}
