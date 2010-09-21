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
                    commandControl.CommandType = this.instruction.Type;
                    commandControl.Text = this.instruction.Code;
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

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);

            commandControl.Application = this.Application;
            commandControl.VariableNames = this.VariableNames;
        }

        private void bOK_Click(object sender, EventArgs e)
        {
            if (instruction == null)
            {
                this.instruction = new CustomSetupInstruction();
            }

            this.instruction.Code = commandControl.Text;
            this.instruction.Type = commandControl.CommandType;
        }
    }
}
