using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using CDBurnerXP.Forms;

namespace Ketarin.Forms
{
    public partial class InstructionBaseDialog : PersistentForm
    {
        private string[] variableNames = new string[0];

        #region Properties

        /// <summary>
        /// Gets or sets the currently edited application.
        /// </summary>
        public ApplicationJob Application
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the variable names to show
        /// within the context menu.
        /// </summary>
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        [Browsable(false)]
        public string[] VariableNames
        {
            get
            {
                return this.variableNames;
            }
            set
            {
                this.variableNames = value;
            }
        }

        public virtual SetupInstruction SetupInstruction
        {
            set
            {
            }
            get
            {
                return null;
            }
        }

        #endregion

        public InstructionBaseDialog()
        {
            InitializeComponent();

            AcceptButton = bOK;
            CancelButton = bCancel;
        }

        /// <summary>
        /// Shows the appropriate edit dialog for each setup instruction.
        /// </summary>
        /// <param name="currentVariables">Currently used variables (for use in textboxes)</param>
        /// <returns>true, if the user did not cancel</returns>
        public static bool ShowDialog(IWin32Window parent, SetupInstruction instruction, string[] currentVariables, ApplicationJob application)
        {
            InstructionBaseDialog dialog = null;

            if (instruction is StartProcessInstruction)
            {
                dialog = new StartProcessInstructionDialog();
            }
            else if (instruction is CopyFileInstruction)
            {
                dialog = new CopyFileInstructionDialog();
            }
            else if (instruction is CustomSetupInstruction)
            {
                dialog = new CustomSetupInstructionDialog();
            }
            else if (instruction is CloseProcessInstruction)
            {
                dialog = new CloseProcessInstructionDialog();
            }

            if (dialog != null)
            {
                dialog.Application = application;
                dialog.SetupInstruction = instruction;
                dialog.VariableNames = currentVariables;
                if (dialog.ShowDialog(parent) == DialogResult.OK)
                {
                    return true;
                }
            }

            return false;
        }
    }
}
