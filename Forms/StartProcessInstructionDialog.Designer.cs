namespace Ketarin.Forms
{
    partial class StartProcessInstructionDialog
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.lblProgram = new System.Windows.Forms.Label();
            this.txtProgram = new Ketarin.Forms.VariableTextBox();
            this.lblParameters = new System.Windows.Forms.Label();
            this.txtParameters = new Ketarin.Forms.VariableTextBox();
            this.bInsertArgument = new wyDay.Controls.SplitButton();
            this.argumentsMenu = new System.Windows.Forms.ContextMenu();
            this.mnuPassive = new System.Windows.Forms.MenuItem();
            this.mnuQN = new System.Windows.Forms.MenuItem();
            this.mnuS = new System.Windows.Forms.MenuItem();
            this.mnuNsis = new System.Windows.Forms.MenuItem();
            this.mnuVerySilent = new System.Windows.Forms.MenuItem();
            this.lnkSilentSetups = new CDBurnerXP.Controls.WebLink();
            this.olvEnvironmentVariables = new CDBurnerXP.Controls.ObjectListView();
            this.colName = new CDBurnerXP.Controls.OLVColumn();
            this.olvValue = new CDBurnerXP.Controls.OLVColumn();
            this.label1 = new System.Windows.Forms.Label();
            this.bBrowse = new System.Windows.Forms.Button();
            this.chkWaitUntilExit = new System.Windows.Forms.CheckBox();
            ((System.ComponentModel.ISupportInitialize)(this.olvEnvironmentVariables)).BeginInit();
            this.SuspendLayout();
            // 
            // bCancel
            // 
            this.bCancel.Location = new System.Drawing.Point(316, 264);
            this.bCancel.TabIndex = 11;
            // 
            // bOK
            // 
            this.bOK.Location = new System.Drawing.Point(235, 264);
            this.bOK.TabIndex = 10;
            this.bOK.Click += new System.EventHandler(this.bOK_Click);
            // 
            // lblProgram
            // 
            this.lblProgram.AutoSize = true;
            this.lblProgram.Location = new System.Drawing.Point(12, 15);
            this.lblProgram.Name = "lblProgram";
            this.lblProgram.Size = new System.Drawing.Size(84, 13);
            this.lblProgram.TabIndex = 0;
            this.lblProgram.Text = "&Program to start:";
            // 
            // txtProgram
            // 
            this.txtProgram.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.txtProgram.Location = new System.Drawing.Point(102, 12);
            this.txtProgram.Name = "txtProgram";
            this.txtProgram.Size = new System.Drawing.Size(255, 20);
            this.txtProgram.TabIndex = 1;
            // 
            // lblParameters
            // 
            this.lblParameters.AutoSize = true;
            this.lblParameters.Location = new System.Drawing.Point(12, 41);
            this.lblParameters.Name = "lblParameters";
            this.lblParameters.Size = new System.Drawing.Size(60, 13);
            this.lblParameters.TabIndex = 3;
            this.lblParameters.Text = "&Arguments:";
            // 
            // txtParameters
            // 
            this.txtParameters.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.txtParameters.Location = new System.Drawing.Point(102, 38);
            this.txtParameters.Name = "txtParameters";
            this.txtParameters.Size = new System.Drawing.Size(222, 20);
            this.txtParameters.TabIndex = 4;
            // 
            // bInsertArgument
            // 
            this.bInsertArgument.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.bInsertArgument.AutoSize = true;
            this.bInsertArgument.Location = new System.Drawing.Point(330, 36);
            this.bInsertArgument.Name = "bInsertArgument";
            this.bInsertArgument.SeparateDropdownButton = false;
            this.bInsertArgument.Size = new System.Drawing.Size(61, 23);
            this.bInsertArgument.SplitMenu = this.argumentsMenu;
            this.bInsertArgument.TabIndex = 5;
            this.bInsertArgument.Text = "I&nsert";
            this.bInsertArgument.UseVisualStyleBackColor = true;
            // 
            // argumentsMenu
            // 
            this.argumentsMenu.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
            this.mnuPassive,
            this.mnuQN,
            this.mnuS,
            this.mnuNsis,
            this.mnuVerySilent});
            // 
            // mnuPassive
            // 
            this.mnuPassive.Index = 0;
            this.mnuPassive.Tag = "/passive";
            this.mnuPassive.Text = "/passive (MS hotfixes)";
            // 
            // mnuQN
            // 
            this.mnuQN.Index = 1;
            this.mnuQN.Tag = "/qn";
            this.mnuQN.Text = "/qn (MSI Installer)";
            // 
            // mnuS
            // 
            this.mnuS.Index = 2;
            this.mnuS.Tag = "/s";
            this.mnuS.Text = "/s (InstallShield)";
            // 
            // mnuNsis
            // 
            this.mnuNsis.Index = 3;
            this.mnuNsis.Tag = "/S";
            this.mnuNsis.Text = "/S (NSIS)";
            // 
            // mnuVerySilent
            // 
            this.mnuVerySilent.Index = 4;
            this.mnuVerySilent.Tag = "/verysilent";
            this.mnuVerySilent.Text = "/verysilent (InnoSetup)";
            // 
            // lnkSilentSetups
            // 
            this.lnkSilentSetups.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.lnkSilentSetups.AutoSize = true;
            this.lnkSilentSetups.Location = new System.Drawing.Point(12, 269);
            this.lnkSilentSetups.Name = "lnkSilentSetups";
            this.lnkSilentSetups.Size = new System.Drawing.Size(143, 13);
            this.lnkSilentSetups.TabIndex = 12;
            this.lnkSilentSetups.TabStop = true;
            this.lnkSilentSetups.Text = "How to control silent setups?";
            this.lnkSilentSetups.Url = "http://unattended.sourceforge.net/installers.php";
            // 
            // olvEnvironmentVariables
            // 
            this.olvEnvironmentVariables.AllColumns.Add(this.colName);
            this.olvEnvironmentVariables.AllColumns.Add(this.olvValue);
            this.olvEnvironmentVariables.AlternateRowBackColor = System.Drawing.Color.Empty;
            this.olvEnvironmentVariables.AlwaysGroupByColumn = null;
            this.olvEnvironmentVariables.AlwaysGroupBySortOrder = System.Windows.Forms.SortOrder.None;
            this.olvEnvironmentVariables.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.olvEnvironmentVariables.CellEditActivation = CDBurnerXP.Controls.ObjectListView.CellEditActivateMode.SingleClick;
            this.olvEnvironmentVariables.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.colName,
            this.olvValue});
            this.olvEnvironmentVariables.GridLines = true;
            this.olvEnvironmentVariables.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.None;
            this.olvEnvironmentVariables.HighlightBackgroundColor = System.Drawing.Color.Empty;
            this.olvEnvironmentVariables.HighlightForegroundColor = System.Drawing.Color.Empty;
            this.olvEnvironmentVariables.LastSortColumn = null;
            this.olvEnvironmentVariables.LastSortOrder = System.Windows.Forms.SortOrder.None;
            this.olvEnvironmentVariables.Location = new System.Drawing.Point(15, 114);
            this.olvEnvironmentVariables.MultiSelect = false;
            this.olvEnvironmentVariables.Name = "olvEnvironmentVariables";
            this.olvEnvironmentVariables.SelectColumnsMenuStaysOpen = false;
            this.olvEnvironmentVariables.ShowGroups = false;
            this.olvEnvironmentVariables.Size = new System.Drawing.Size(376, 127);
            this.olvEnvironmentVariables.Sorting = System.Windows.Forms.SortOrder.Ascending;
            this.olvEnvironmentVariables.TabIndex = 8;
            this.olvEnvironmentVariables.UseCompatibleStateImageBehavior = false;
            this.olvEnvironmentVariables.View = System.Windows.Forms.View.Details;
            // 
            // colName
            // 
            this.colName.AspectName = "Name";
            this.colName.IsEditable = false;
            this.colName.Text = "Name";
            this.colName.Width = 169;
            // 
            // olvValue
            // 
            this.olvValue.AspectName = "Value";
            this.olvValue.FillsFreeSpace = true;
            this.olvValue.Text = "Value";
            this.olvValue.Width = 161;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 98);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(156, 13);
            this.label1.TabIndex = 7;
            this.label1.Text = "&Override environment variables:";
            // 
            // bBrowse
            // 
            this.bBrowse.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.bBrowse.Location = new System.Drawing.Point(363, 10);
            this.bBrowse.Name = "bBrowse";
            this.bBrowse.Size = new System.Drawing.Size(28, 23);
            this.bBrowse.TabIndex = 2;
            this.bBrowse.Text = "...";
            this.bBrowse.UseVisualStyleBackColor = true;
            this.bBrowse.Click += new System.EventHandler(this.bBrowse_Click);
            // 
            // chkWaitUntilExit
            // 
            this.chkWaitUntilExit.AutoSize = true;
            this.chkWaitUntilExit.Checked = true;
            this.chkWaitUntilExit.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkWaitUntilExit.Location = new System.Drawing.Point(15, 64);
            this.chkWaitUntilExit.Name = "chkWaitUntilExit";
            this.chkWaitUntilExit.Size = new System.Drawing.Size(161, 17);
            this.chkWaitUntilExit.TabIndex = 6;
            this.chkWaitUntilExit.Text = "&Wait until process has exited";
            this.chkWaitUntilExit.UseVisualStyleBackColor = true;
            // 
            // StartProcessInstructionDialog
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(403, 299);
            this.Controls.Add(this.chkWaitUntilExit);
            this.Controls.Add(this.bBrowse);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.olvEnvironmentVariables);
            this.Controls.Add(this.lnkSilentSetups);
            this.Controls.Add(this.bInsertArgument);
            this.Controls.Add(this.txtParameters);
            this.Controls.Add(this.lblParameters);
            this.Controls.Add(this.txtProgram);
            this.Controls.Add(this.lblProgram);
            this.MinimumSize = new System.Drawing.Size(400, 300);
            this.Name = "StartProcessInstructionDialog";
            this.Text = "Start Process";
            this.Controls.SetChildIndex(this.bOK, 0);
            this.Controls.SetChildIndex(this.bCancel, 0);
            this.Controls.SetChildIndex(this.lblProgram, 0);
            this.Controls.SetChildIndex(this.txtProgram, 0);
            this.Controls.SetChildIndex(this.lblParameters, 0);
            this.Controls.SetChildIndex(this.txtParameters, 0);
            this.Controls.SetChildIndex(this.bInsertArgument, 0);
            this.Controls.SetChildIndex(this.lnkSilentSetups, 0);
            this.Controls.SetChildIndex(this.olvEnvironmentVariables, 0);
            this.Controls.SetChildIndex(this.label1, 0);
            this.Controls.SetChildIndex(this.bBrowse, 0);
            this.Controls.SetChildIndex(this.chkWaitUntilExit, 0);
            ((System.ComponentModel.ISupportInitialize)(this.olvEnvironmentVariables)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label lblProgram;
        private VariableTextBox txtProgram;
        private System.Windows.Forms.Label lblParameters;
        private VariableTextBox txtParameters;
        private wyDay.Controls.SplitButton bInsertArgument;
        private System.Windows.Forms.ContextMenu argumentsMenu;
        private System.Windows.Forms.MenuItem mnuVerySilent;
        private System.Windows.Forms.MenuItem mnuQN;
        private System.Windows.Forms.MenuItem mnuS;
        private System.Windows.Forms.MenuItem mnuPassive;
        private System.Windows.Forms.MenuItem mnuNsis;
        private CDBurnerXP.Controls.WebLink lnkSilentSetups;
        private CDBurnerXP.Controls.ObjectListView olvEnvironmentVariables;
        private CDBurnerXP.Controls.OLVColumn colName;
        private CDBurnerXP.Controls.OLVColumn olvValue;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button bBrowse;
        private System.Windows.Forms.CheckBox chkWaitUntilExit;
    }
}