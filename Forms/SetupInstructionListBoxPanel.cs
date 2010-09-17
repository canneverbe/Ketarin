using System;
using System.Collections.Generic;
using System.Text;
using CDBurnerXP.Controls;
using System.Windows.Forms;
using System.Drawing;
using CDBurnerXP.Forms;
using System.Threading;

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
        private ListBoxPanel.TransparentLabel lblMoveUp;
        private ListBoxPanel.TransparentLabel lblMoveDown;
        private SetupInstruction instruction = null;

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
            if (InstructionBaseDialog.ShowDialog(this, this.instruction, this.VariableNames))
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
            this.lblCommandName = new CDBurnerXP.Controls.ListBoxPanel.TransparentLabel();
            this.lblCommandText = new CDBurnerXP.Controls.ListBoxPanel.TransparentLabel();
            this.lnkEdit = new System.Windows.Forms.LinkLabel();
            this.lnkRemove = new System.Windows.Forms.LinkLabel();
            this.lblMoveDown = new CDBurnerXP.Controls.ListBoxPanel.TransparentLabel();
            this.lblMoveUp = new CDBurnerXP.Controls.ListBoxPanel.TransparentLabel();
            this.SuspendLayout();
            // 
            // lblCommandName
            // 
            this.lblCommandName.AutoHeight = false;
            this.lblCommandName.AutoSize = true;
            this.lblCommandName.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(240)))), ((int)(((byte)(240)))), ((int)(((byte)(240)))));
            this.lblCommandName.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblCommandName.Location = new System.Drawing.Point(3, 4);
            this.lblCommandName.Name = "lblCommandName";
            this.lblCommandName.Size = new System.Drawing.Size(97, 13);
            this.lblCommandName.TabIndex = 0;
            this.lblCommandName.Text = "Command Name";
            // 
            // lblCommandText
            // 
            this.lblCommandText.AutoHeight = false;
            this.lblCommandText.AutoSize = true;
            this.lblCommandText.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(240)))), ((int)(((byte)(240)))), ((int)(((byte)(240)))));
            this.lblCommandText.Location = new System.Drawing.Point(3, 20);
            this.lblCommandText.Name = "lblCommandText";
            this.lblCommandText.Size = new System.Drawing.Size(74, 13);
            this.lblCommandText.TabIndex = 1;
            this.lblCommandText.Text = "Command text";
            // 
            // lnkEdit
            // 
            this.lnkEdit.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.lnkEdit.AutoSize = true;
            this.lnkEdit.BackColor = System.Drawing.Color.Transparent;
            this.lnkEdit.Location = new System.Drawing.Point(225, 20);
            this.lnkEdit.Name = "lnkEdit";
            this.lnkEdit.Size = new System.Drawing.Size(25, 13);
            this.lnkEdit.TabIndex = 2;
            this.lnkEdit.TabStop = true;
            this.lnkEdit.Text = "Edit";
            this.lnkEdit.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.lnkEdit_LinkClicked);
            // 
            // lnkRemove
            // 
            this.lnkRemove.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.lnkRemove.AutoSize = true;
            this.lnkRemove.BackColor = System.Drawing.Color.Transparent;
            this.lnkRemove.Location = new System.Drawing.Point(253, 20);
            this.lnkRemove.Name = "lnkRemove";
            this.lnkRemove.Size = new System.Drawing.Size(47, 13);
            this.lnkRemove.TabIndex = 3;
            this.lnkRemove.TabStop = true;
            this.lnkRemove.Text = "Remove";
            this.lnkRemove.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.lnkRemove_LinkClicked);
            // 
            // lblMoveDown
            // 
            this.lblMoveDown.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.lblMoveDown.AutoHeight = false;
            this.lblMoveDown.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(240)))), ((int)(((byte)(240)))), ((int)(((byte)(240)))));
            this.lblMoveDown.Cursor = System.Windows.Forms.Cursors.Hand;
            this.lblMoveDown.Image = global::Ketarin.Properties.Resources.Arrow_Down;
            this.lblMoveDown.Location = new System.Drawing.Point(283, 0);
            this.lblMoveDown.Name = "lblMoveDown";
            this.lblMoveDown.Size = new System.Drawing.Size(17, 17);
            this.lblMoveDown.TabIndex = 5;
            this.lblMoveDown.Click += new System.EventHandler(this.lblMoveDown_Click);
            // 
            // lblMoveUp
            // 
            this.lblMoveUp.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.lblMoveUp.AutoHeight = false;
            this.lblMoveUp.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(240)))), ((int)(((byte)(240)))), ((int)(((byte)(240)))));
            this.lblMoveUp.Cursor = System.Windows.Forms.Cursors.Hand;
            this.lblMoveUp.Image = global::Ketarin.Properties.Resources.Arrow_Up;
            this.lblMoveUp.Location = new System.Drawing.Point(266, -1);
            this.lblMoveUp.Name = "lblMoveUp";
            this.lblMoveUp.Size = new System.Drawing.Size(17, 17);
            this.lblMoveUp.TabIndex = 4;
            this.lblMoveUp.Click += new System.EventHandler(this.lblMoveUp_Click);
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
            this.Size = new System.Drawing.Size(303, 41);
            this.ResumeLayout(false);
            this.PerformLayout();

        }
    }
}
