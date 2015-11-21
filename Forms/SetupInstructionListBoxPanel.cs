using System;
using System.Drawing;
using System.Windows.Forms;
using CDBurnerXP.Controls;
using CDBurnerXP.Forms;
using Ketarin.Properties;

namespace Ketarin.Forms
{
    /// <summary>
    /// Represents a setup instruction as item in a ListBox.
    /// </summary>
    public class SetupInstructionListBoxPanel : ListBoxPanel
    {
        #region Properties

        /// <summary>
        /// Gets or sets the variable names to use for edit dialogs.
        /// </summary>
        public string[] VariableNames
        {
            get; set;
        }

        /// <summary>
        /// Gets or sets the command name to be displayed (heading)
        /// </summary>
        public string CommandText
        {
            get { return lblCommandText.Text; }
            private set { lblCommandText.Text = value; }
        }

        /// <summary>
        /// Gets or sets the command information to be displayed.
        /// </summary>
        public string CommandName
        {
            get { return lblCommandName.Text; }
            private set { lblCommandName.Text = value; }
        }

        /// <summary>
        /// Gets the associated setup instruction.
        /// </summary>
        public SetupInstruction SetupInstruction
        {
            get { return instruction; }
        }

        #endregion

        private TransparentLabel lblCommandText;
        private LinkLabel lnkEdit;
        private LinkLabel lnkRemove;
        private TransparentLabel lblCommandName;
        private TransparentLabel lblMoveUp;
        private TransparentLabel lblMoveDown;
        private SetupInstruction instruction;

        public SetupInstructionListBoxPanel()
        {
            InitializeComponent();
            SetStyle(ControlStyles.OptimizedDoubleBuffer | ControlStyles.AllPaintingInWmPaint, true);
            GetAllMouseDowns(Controls);
        }

        public SetupInstructionListBoxPanel(SetupInstruction instruction) : this()
        {
            UpdateFromInstruction(instruction);
        }

        protected override void OnDoubleClick(EventArgs e)
        {
            base.OnDoubleClick(e);

            EditInstruction();
        }

        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            if (keyData == Keys.Delete)
            {
                RemoveInstruction();
                return true;
            }

            return base.ProcessCmdKey(ref msg, keyData);
        }

        private void lnkEdit_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            EditInstruction();
        }

        private void lblMoveUp_Click(object sender, EventArgs e)
        {
            int oldIndex = this.Parent.Controls.IndexOf(this);
            if (oldIndex <= 0) return;

            Control parent = this.Parent;

            Control[] oldControls = new Control[this.Parent.Controls.Count];
            parent.Controls.CopyTo(oldControls, 0);

            oldControls[oldIndex] = oldControls[oldIndex - 1];
            oldControls[oldIndex - 1] = this;

            UpdateParent(parent, oldControls);
        }

        private void lblMoveDown_Click(object sender, EventArgs e)
        {
            int oldIndex = this.Parent.Controls.IndexOf(this);
            if (oldIndex >= this.Parent.Controls.Count - 1) return;

            Control parent = this.Parent;

            Control[] oldControls = new Control[this.Parent.Controls.Count];
            parent.Controls.CopyTo(oldControls, 0);

            oldControls[oldIndex] = oldControls[oldIndex + 1];
            oldControls[oldIndex + 1] = this;

            UpdateParent(parent, oldControls);
        }

        private void UpdateParent(Control parent, Control[] oldControls)
        {
            // Prevent flicker and the like
            using (new ControlRedrawLock(parent.Parent))
            {
                parent.Controls.AddRange(oldControls);

                // Brute force scrollbar update
                parent.Width += 1;
                parent.Width -= 1;

                this.Select();
            }
        }

        private void UpdateFromInstruction(SetupInstruction instruction)
        {
            this.instruction = instruction;
            CommandText = instruction.ToString();
            CommandName = instruction.Name;
        }

        private void RemoveInstruction()
        {
            this.Parent.Controls.Remove(this);
        }

        private void EditInstruction()
        {
            if (InstructionBaseDialog.ShowDialog(this, this.instruction, this.VariableNames, this.instruction.Application))
            {
                UpdateFromInstruction(instruction);
            }
        }

        private void lnkRemove_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            RemoveInstruction();
        }

        private void InitializeComponent()
        {
            this.lblCommandName = new TransparentLabel();
            this.lblCommandText = new TransparentLabel();
            this.lnkEdit = new LinkLabel();
            this.lnkRemove = new LinkLabel();
            this.lblMoveDown = new TransparentLabel();
            this.lblMoveUp = new TransparentLabel();
            this.SuspendLayout();
            // 
            // lblCommandName
            // 
            this.lblCommandName.AutoHeight = false;
            this.lblCommandName.AutoSize = true;
            this.lblCommandName.BackColor = Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(240)))), ((int)(((byte)(240)))), ((int)(((byte)(240)))));
            this.lblCommandName.Font = new Font("Microsoft Sans Serif", 8.25F, FontStyle.Bold, GraphicsUnit.Point, ((byte)(0)));
            this.lblCommandName.Location = new Point(3, 4);
            this.lblCommandName.Name = "lblCommandName";
            this.lblCommandName.Size = new Size(97, 13);
            this.lblCommandName.TabIndex = 0;
            this.lblCommandName.Text = "Command Name";
            // 
            // lblCommandText
            // 
            this.lblCommandText.AutoHeight = false;
            this.lblCommandText.AutoSize = true;
            this.lblCommandText.BackColor = Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(240)))), ((int)(((byte)(240)))), ((int)(((byte)(240)))));
            this.lblCommandText.Location = new Point(3, 20);
            this.lblCommandText.Name = "lblCommandText";
            this.lblCommandText.Size = new Size(74, 13);
            this.lblCommandText.TabIndex = 1;
            this.lblCommandText.Text = "Command text";
            // 
            // lnkEdit
            // 
            this.lnkEdit.Anchor = ((AnchorStyles)((AnchorStyles.Bottom | AnchorStyles.Right)));
            this.lnkEdit.AutoSize = true;
            this.lnkEdit.BackColor = Color.Transparent;
            this.lnkEdit.Location = new Point(225, 20);
            this.lnkEdit.Name = "lnkEdit";
            this.lnkEdit.Size = new Size(25, 13);
            this.lnkEdit.TabIndex = 2;
            this.lnkEdit.TabStop = true;
            this.lnkEdit.Text = "Edit";
            this.lnkEdit.LinkClicked += this.lnkEdit_LinkClicked;
            // 
            // lnkRemove
            // 
            this.lnkRemove.Anchor = ((AnchorStyles)((AnchorStyles.Bottom | AnchorStyles.Right)));
            this.lnkRemove.AutoSize = true;
            this.lnkRemove.BackColor = Color.Transparent;
            this.lnkRemove.Location = new Point(253, 20);
            this.lnkRemove.Name = "lnkRemove";
            this.lnkRemove.Size = new Size(47, 13);
            this.lnkRemove.TabIndex = 3;
            this.lnkRemove.TabStop = true;
            this.lnkRemove.Text = "Remove";
            this.lnkRemove.LinkClicked += this.lnkRemove_LinkClicked;
            // 
            // lblMoveDown
            // 
            this.lblMoveDown.Anchor = ((AnchorStyles)((AnchorStyles.Top | AnchorStyles.Right)));
            this.lblMoveDown.AutoHeight = false;
            this.lblMoveDown.BackColor = Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(240)))), ((int)(((byte)(240)))), ((int)(((byte)(240)))));
            this.lblMoveDown.Cursor = Cursors.Hand;
            this.lblMoveDown.Image = Resources.Arrow_Down;
            this.lblMoveDown.Location = new Point(283, 0);
            this.lblMoveDown.Name = "lblMoveDown";
            this.lblMoveDown.Size = new Size(17, 17);
            this.lblMoveDown.TabIndex = 5;
            this.lblMoveDown.Click += this.lblMoveDown_Click;
            // 
            // lblMoveUp
            // 
            this.lblMoveUp.Anchor = ((AnchorStyles)((AnchorStyles.Top | AnchorStyles.Right)));
            this.lblMoveUp.AutoHeight = false;
            this.lblMoveUp.BackColor = Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(240)))), ((int)(((byte)(240)))), ((int)(((byte)(240)))));
            this.lblMoveUp.Cursor = Cursors.Hand;
            this.lblMoveUp.Image = Resources.Arrow_Up;
            this.lblMoveUp.Location = new Point(266, -1);
            this.lblMoveUp.Name = "lblMoveUp";
            this.lblMoveUp.Size = new Size(17, 17);
            this.lblMoveUp.TabIndex = 4;
            this.lblMoveUp.Click += this.lblMoveUp_Click;
            // 
            // SetupInstructionListBoxPanel
            // 
            this.Controls.Add(this.lblMoveDown);
            this.Controls.Add(this.lblMoveUp);
            this.Controls.Add(this.lnkRemove);
            this.Controls.Add(this.lnkEdit);
            this.Controls.Add(this.lblCommandText);
            this.Controls.Add(this.lblCommandName);
            this.Name = "SetupInstructionListBoxPanel";
            this.Size = new Size(303, 41);
            this.ResumeLayout(false);
            this.PerformLayout();

        }
    }
}
