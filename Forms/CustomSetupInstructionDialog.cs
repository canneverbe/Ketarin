using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.CodeDom.Compiler;

namespace Ketarin.Forms
{
    /// <summary>
    /// Represents a dialog which contains a syntax highlighted textbox
    /// to enter a C# script for a custom setup instruction.
    /// </summary>
    public partial class CustomSetupInstructionDialog : InstructionBaseDialog
    {
        private CustomSetupInstruction instruction = null;

        #region Properties

        public override SetupInstruction SetupInstruction
        {
            set
            {
                CustomSetupInstruction instruction = value as CustomSetupInstruction;
                if (instruction != null)
                {
                    this.instruction = instruction;
                    txtCode.Text = this.instruction.Code;
                }
            }
            get
            {
                return this.instruction;
            }
        }

        #endregion

        public CustomSetupInstructionDialog()
        {
            InitializeComponent();
        }

        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            switch (keyData)
            {
                case Keys.Control | Keys.B:
                case Keys.Control | Keys.Shift | Keys.B:
                    ValidateScript(true);
                    return true;
            }

            return base.ProcessCmdKey(ref msg, keyData);
        }

        private void bOK_Click(object sender, EventArgs e)
        {
            if (!ValidateScript(false))
            {
                DialogResult = DialogResult.None;
                return;
            }

            if (instruction == null)
            {
                this.instruction = new CustomSetupInstruction();
            }

            this.instruction.Code = txtCode.Text;
        }

        private void bTestScript_Click(object sender, EventArgs e)
        {
            ValidateScript(true);
        }

        /// <summary>
        /// Verifies the syntactic validity of the user script.
        /// </summary>
        private bool ValidateScript(bool confirmOK)
        {
            try
            {
                CustomSetupInstruction testInstruction = new CustomSetupInstruction();
                testInstruction.Code = txtCode.Text;

                CompilerErrorCollection errors;
                testInstruction.Compile(out errors);

                txtCode.ClearAllAnnotations();

                if (errors.HasErrors)
                {
                    bool hasScrolled = false;

                    foreach (CompilerError error in errors)
                    {
                        int lineNum = error.Line - testInstruction.LineAtCodeStart;
                        if (!hasScrolled)
                        {
                            hasScrolled = true;
                            txtCode.ScrollToLine(lineNum);
                        }
                        txtCode.SetAnnotation(lineNum, error.ErrorText, error.IsWarning);
                    }
                }
                else
                {
                    if (confirmOK)
                    {
                        MessageBox.Show(this, "No errors could be found in the script.", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    return true;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(this, "The code cannot be compiled: " + ex.Message, Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            return false;
        }
    }
}
