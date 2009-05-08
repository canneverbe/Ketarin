namespace Ketarin.Forms
{
    partial class EditVariablesDialog
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(EditVariablesDialog));
            this.lblVariables = new System.Windows.Forms.Label();
            this.lbVariables = new Ketarin.Forms.EditVariablesDialog.VariableListBox();
            this.bCancel = new System.Windows.Forms.Button();
            this.bOK = new System.Windows.Forms.Button();
            this.lblUrl = new System.Windows.Forms.Label();
            this.txtUrl = new Ketarin.Forms.VariableTextBox();
            this.rtfContent = new System.Windows.Forms.RichTextBox();
            this.cmuRtf = new System.Windows.Forms.ContextMenu();
            this.cmnuCopy = new System.Windows.Forms.MenuItem();
            this.cmnuPaste = new System.Windows.Forms.MenuItem();
            this.cmnuCopyMatch = new System.Windows.Forms.MenuItem();
            this.cmnuGoToMatch = new System.Windows.Forms.MenuItem();
            this.sepPreview = new System.Windows.Forms.MenuItem();
            this.cmnuBrowser = new System.Windows.Forms.MenuItem();
            this.bLoad = new System.Windows.Forms.Button();
            this.bUseAsStart = new System.Windows.Forms.Button();
            this.bUseAsEnd = new System.Windows.Forms.Button();
            this.bAdd = new System.Windows.Forms.Button();
            this.bRemove = new System.Windows.Forms.Button();
            this.bFind = new System.Windows.Forms.Button();
            this.lblFind = new System.Windows.Forms.Label();
            this.txtFind = new System.Windows.Forms.TextBox();
            this.lblDesc = new System.Windows.Forms.Label();
            this.txtRegularExpression = new System.Windows.Forms.TextBox();
            this.lblRegex = new System.Windows.Forms.Label();
            this.rbContentUrlStartEnd = new System.Windows.Forms.RadioButton();
            this.rbContentUrlRegex = new System.Windows.Forms.RadioButton();
            this.rbContentText = new System.Windows.Forms.RadioButton();
            this.chkRightToLeft = new System.Windows.Forms.CheckBox();
            this.bPostData = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // lblVariables
            // 
            this.lblVariables.AutoSize = true;
            this.lblVariables.Location = new System.Drawing.Point(12, 56);
            this.lblVariables.Name = "lblVariables";
            this.lblVariables.Size = new System.Drawing.Size(53, 13);
            this.lblVariables.TabIndex = 1;
            this.lblVariables.Text = "&Variables:";
            // 
            // lbVariables
            // 
            this.lbVariables.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)));
            this.lbVariables.FormattingEnabled = true;
            this.lbVariables.Location = new System.Drawing.Point(12, 72);
            this.lbVariables.Name = "lbVariables";
            this.lbVariables.Size = new System.Drawing.Size(94, 303);
            this.lbVariables.TabIndex = 2;
            this.lbVariables.SelectedIndexChanged += new System.EventHandler(this.lbVariables_SelectedIndexChanged);
            // 
            // bCancel
            // 
            this.bCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.bCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.bCancel.Location = new System.Drawing.Point(527, 400);
            this.bCancel.Name = "bCancel";
            this.bCancel.Size = new System.Drawing.Size(75, 23);
            this.bCancel.TabIndex = 22;
            this.bCancel.Text = "Cancel";
            this.bCancel.UseVisualStyleBackColor = true;
            // 
            // bOK
            // 
            this.bOK.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.bOK.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.bOK.Location = new System.Drawing.Point(446, 400);
            this.bOK.Name = "bOK";
            this.bOK.Size = new System.Drawing.Size(75, 23);
            this.bOK.TabIndex = 21;
            this.bOK.Text = "OK";
            this.bOK.UseVisualStyleBackColor = true;
            this.bOK.Click += new System.EventHandler(this.bOK_Click);
            // 
            // lblUrl
            // 
            this.lblUrl.AutoSize = true;
            this.lblUrl.Enabled = false;
            this.lblUrl.Location = new System.Drawing.Point(123, 107);
            this.lblUrl.Name = "lblUrl";
            this.lblUrl.Size = new System.Drawing.Size(100, 13);
            this.lblUrl.TabIndex = 8;
            this.lblUrl.Text = "&Contents from URL:";
            // 
            // txtUrl
            // 
            this.txtUrl.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.txtUrl.Enabled = false;
            this.txtUrl.Location = new System.Drawing.Point(247, 104);
            this.txtUrl.Name = "txtUrl";
            this.txtUrl.Size = new System.Drawing.Size(211, 20);
            this.txtUrl.TabIndex = 9;
            this.txtUrl.TextChanged += new System.EventHandler(this.txtUrl_TextChanged);
            // 
            // rtfContent
            // 
            this.rtfContent.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.rtfContent.BackColor = System.Drawing.SystemColors.Window;
            this.rtfContent.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.rtfContent.ContextMenu = this.cmuRtf;
            this.rtfContent.DetectUrls = false;
            this.rtfContent.Enabled = false;
            this.rtfContent.Font = new System.Drawing.Font("Courier New", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.rtfContent.HideSelection = false;
            this.rtfContent.Location = new System.Drawing.Point(126, 182);
            this.rtfContent.Name = "rtfContent";
            this.rtfContent.ReadOnly = true;
            this.rtfContent.Size = new System.Drawing.Size(476, 193);
            this.rtfContent.TabIndex = 18;
            this.rtfContent.Text = "";
            this.rtfContent.WordWrap = false;
            this.rtfContent.SelectionChanged += new System.EventHandler(this.rtfContent_SelectionChanged);
            this.rtfContent.TextChanged += new System.EventHandler(this.rtfContent_TextChanged);
            // 
            // cmuRtf
            // 
            this.cmuRtf.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
            this.cmnuCopy,
            this.cmnuPaste,
            this.cmnuCopyMatch,
            this.cmnuGoToMatch,
            this.sepPreview,
            this.cmnuBrowser});
            // 
            // cmnuCopy
            // 
            this.cmnuCopy.Enabled = false;
            this.cmnuCopy.Index = 0;
            this.cmnuCopy.Shortcut = System.Windows.Forms.Shortcut.CtrlP;
            this.cmnuCopy.Text = "&Copy";
            this.cmnuCopy.Click += new System.EventHandler(this.cmnuCopy_Click);
            // 
            // cmnuPaste
            // 
            this.cmnuPaste.Index = 1;
            this.cmnuPaste.Shortcut = System.Windows.Forms.Shortcut.CtrlV;
            this.cmnuPaste.Text = "&Paste";
            this.cmnuPaste.Click += new System.EventHandler(this.cmnuPaste_Click);
            // 
            // cmnuCopyMatch
            // 
            this.cmnuCopyMatch.Enabled = false;
            this.cmnuCopyMatch.Index = 2;
            this.cmnuCopyMatch.Shortcut = System.Windows.Forms.Shortcut.CtrlM;
            this.cmnuCopyMatch.Text = "Copy &match";
            this.cmnuCopyMatch.Click += new System.EventHandler(this.cmnuCopyMatch_Click);
            // 
            // cmnuGoToMatch
            // 
            this.cmnuGoToMatch.Index = 3;
            this.cmnuGoToMatch.Shortcut = System.Windows.Forms.Shortcut.CtrlG;
            this.cmnuGoToMatch.Text = "&Go to match";
            this.cmnuGoToMatch.Click += new System.EventHandler(this.cmnuGoToMatch_Click);
            // 
            // sepPreview
            // 
            this.sepPreview.Index = 4;
            this.sepPreview.Text = "-";
            // 
            // cmnuBrowser
            // 
            this.cmnuBrowser.Index = 5;
            this.cmnuBrowser.Text = "&Show in webbrowser";
            this.cmnuBrowser.Click += new System.EventHandler(this.cmnuBrowser_Click);
            // 
            // bLoad
            // 
            this.bLoad.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.bLoad.Enabled = false;
            this.bLoad.Location = new System.Drawing.Point(545, 102);
            this.bLoad.Name = "bLoad";
            this.bLoad.Size = new System.Drawing.Size(57, 23);
            this.bLoad.TabIndex = 11;
            this.bLoad.Text = "&Load";
            this.bLoad.UseVisualStyleBackColor = true;
            this.bLoad.Click += new System.EventHandler(this.bLoad_Click);
            // 
            // bUseAsStart
            // 
            this.bUseAsStart.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.bUseAsStart.Enabled = false;
            this.bUseAsStart.Location = new System.Drawing.Point(126, 381);
            this.bUseAsStart.Name = "bUseAsStart";
            this.bUseAsStart.Size = new System.Drawing.Size(134, 23);
            this.bUseAsStart.TabIndex = 19;
            this.bUseAsStart.Text = "&Use selection as start";
            this.bUseAsStart.UseVisualStyleBackColor = true;
            this.bUseAsStart.Click += new System.EventHandler(this.bUseAsStart_Click);
            // 
            // bUseAsEnd
            // 
            this.bUseAsEnd.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.bUseAsEnd.Enabled = false;
            this.bUseAsEnd.Location = new System.Drawing.Point(266, 381);
            this.bUseAsEnd.Name = "bUseAsEnd";
            this.bUseAsEnd.Size = new System.Drawing.Size(134, 23);
            this.bUseAsEnd.TabIndex = 20;
            this.bUseAsEnd.Text = "Us&e selection as end";
            this.bUseAsEnd.UseVisualStyleBackColor = true;
            this.bUseAsEnd.Click += new System.EventHandler(this.bUseAsEnd_Click);
            // 
            // bAdd
            // 
            this.bAdd.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.bAdd.Location = new System.Drawing.Point(12, 381);
            this.bAdd.Name = "bAdd";
            this.bAdd.Size = new System.Drawing.Size(45, 23);
            this.bAdd.TabIndex = 3;
            this.bAdd.Text = "+";
            this.bAdd.UseVisualStyleBackColor = true;
            this.bAdd.Click += new System.EventHandler(this.bAdd_Click);
            // 
            // bRemove
            // 
            this.bRemove.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.bRemove.Enabled = false;
            this.bRemove.Location = new System.Drawing.Point(61, 381);
            this.bRemove.Name = "bRemove";
            this.bRemove.Size = new System.Drawing.Size(45, 23);
            this.bRemove.TabIndex = 4;
            this.bRemove.Text = "—";
            this.bRemove.UseVisualStyleBackColor = true;
            this.bRemove.Click += new System.EventHandler(this.bRemove_Click);
            // 
            // bFind
            // 
            this.bFind.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.bFind.Enabled = false;
            this.bFind.Location = new System.Drawing.Point(545, 127);
            this.bFind.Name = "bFind";
            this.bFind.Size = new System.Drawing.Size(57, 23);
            this.bFind.TabIndex = 14;
            this.bFind.Text = "&Find";
            this.bFind.UseVisualStyleBackColor = true;
            this.bFind.Click += new System.EventHandler(this.bFind_Click);
            // 
            // lblFind
            // 
            this.lblFind.AutoSize = true;
            this.lblFind.Enabled = false;
            this.lblFind.Location = new System.Drawing.Point(123, 133);
            this.lblFind.Name = "lblFind";
            this.lblFind.Size = new System.Drawing.Size(118, 13);
            this.lblFind.TabIndex = 12;
            this.lblFind.Text = "&Search within contents:";
            // 
            // txtFind
            // 
            this.txtFind.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.txtFind.Enabled = false;
            this.txtFind.Location = new System.Drawing.Point(247, 130);
            this.txtFind.Name = "txtFind";
            this.txtFind.Size = new System.Drawing.Size(292, 20);
            this.txtFind.TabIndex = 13;
            // 
            // lblDesc
            // 
            this.lblDesc.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.lblDesc.Location = new System.Drawing.Point(12, 9);
            this.lblDesc.Name = "lblDesc";
            this.lblDesc.Size = new System.Drawing.Size(590, 42);
            this.lblDesc.TabIndex = 0;
            this.lblDesc.Text = resources.GetString("lblDesc.Text");
            // 
            // txtRegularExpression
            // 
            this.txtRegularExpression.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.txtRegularExpression.Enabled = false;
            this.txtRegularExpression.Location = new System.Drawing.Point(247, 156);
            this.txtRegularExpression.Name = "txtRegularExpression";
            this.txtRegularExpression.Size = new System.Drawing.Size(292, 20);
            this.txtRegularExpression.TabIndex = 16;
            this.txtRegularExpression.TextChanged += new System.EventHandler(this.txtRegularExpression_TextChanged);
            // 
            // lblRegex
            // 
            this.lblRegex.AutoSize = true;
            this.lblRegex.Enabled = false;
            this.lblRegex.Location = new System.Drawing.Point(123, 159);
            this.lblRegex.Name = "lblRegex";
            this.lblRegex.Size = new System.Drawing.Size(117, 13);
            this.lblRegex.TabIndex = 15;
            this.lblRegex.Text = "Use regular e&xpression:";
            // 
            // rbContentUrlStartEnd
            // 
            this.rbContentUrlStartEnd.AutoSize = true;
            this.rbContentUrlStartEnd.Enabled = false;
            this.rbContentUrlStartEnd.Location = new System.Drawing.Point(126, 72);
            this.rbContentUrlStartEnd.Name = "rbContentUrlStartEnd";
            this.rbContentUrlStartEnd.Size = new System.Drawing.Size(162, 17);
            this.rbContentUrlStartEnd.TabIndex = 5;
            this.rbContentUrlStartEnd.Text = "Content from URL (st&art/end)";
            this.rbContentUrlStartEnd.UseVisualStyleBackColor = true;
            this.rbContentUrlStartEnd.CheckedChanged += new System.EventHandler(this.rbContentUrlStartEnd_CheckedChanged);
            // 
            // rbContentUrlRegex
            // 
            this.rbContentUrlRegex.AutoSize = true;
            this.rbContentUrlRegex.Enabled = false;
            this.rbContentUrlRegex.Location = new System.Drawing.Point(294, 72);
            this.rbContentUrlRegex.Name = "rbContentUrlRegex";
            this.rbContentUrlRegex.Size = new System.Drawing.Size(210, 17);
            this.rbContentUrlRegex.TabIndex = 6;
            this.rbContentUrlRegex.Text = "Content from URL (&Regular Expression)";
            this.rbContentUrlRegex.UseVisualStyleBackColor = true;
            this.rbContentUrlRegex.CheckedChanged += new System.EventHandler(this.rbContentUrlRegex_CheckedChanged);
            // 
            // rbContentText
            // 
            this.rbContentText.AutoSize = true;
            this.rbContentText.Enabled = false;
            this.rbContentText.Location = new System.Drawing.Point(510, 72);
            this.rbContentText.Name = "rbContentText";
            this.rbContentText.Size = new System.Drawing.Size(99, 17);
            this.rbContentText.TabIndex = 7;
            this.rbContentText.Text = "&Textual content";
            this.rbContentText.UseVisualStyleBackColor = true;
            this.rbContentText.CheckedChanged += new System.EventHandler(this.rbContentText_CheckedChanged);
            // 
            // chkRightToLeft
            // 
            this.chkRightToLeft.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.chkRightToLeft.AutoSize = true;
            this.chkRightToLeft.Enabled = false;
            this.chkRightToLeft.Location = new System.Drawing.Point(545, 158);
            this.chkRightToLeft.Name = "chkRightToLeft";
            this.chkRightToLeft.Size = new System.Drawing.Size(47, 17);
            this.chkRightToLeft.TabIndex = 17;
            this.chkRightToLeft.Text = "&RTL";
            this.chkRightToLeft.UseVisualStyleBackColor = true;
            this.chkRightToLeft.CheckedChanged += new System.EventHandler(this.chkRightToLeft_CheckedChanged);
            // 
            // bPostData
            // 
            this.bPostData.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.bPostData.Enabled = false;
            this.bPostData.Location = new System.Drawing.Point(464, 102);
            this.bPostData.Name = "bPostData";
            this.bPostData.Size = new System.Drawing.Size(75, 23);
            this.bPostData.TabIndex = 10;
            this.bPostData.Text = "&POST data";
            this.bPostData.UseVisualStyleBackColor = true;
            this.bPostData.Click += new System.EventHandler(this.bPostData_Click);
            // 
            // EditVariablesDialog
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(614, 435);
            this.Controls.Add(this.bPostData);
            this.Controls.Add(this.chkRightToLeft);
            this.Controls.Add(this.rbContentText);
            this.Controls.Add(this.rbContentUrlRegex);
            this.Controls.Add(this.rbContentUrlStartEnd);
            this.Controls.Add(this.txtRegularExpression);
            this.Controls.Add(this.lblRegex);
            this.Controls.Add(this.lblDesc);
            this.Controls.Add(this.txtFind);
            this.Controls.Add(this.lblFind);
            this.Controls.Add(this.bFind);
            this.Controls.Add(this.bRemove);
            this.Controls.Add(this.bAdd);
            this.Controls.Add(this.bUseAsEnd);
            this.Controls.Add(this.bUseAsStart);
            this.Controls.Add(this.bLoad);
            this.Controls.Add(this.rtfContent);
            this.Controls.Add(this.txtUrl);
            this.Controls.Add(this.lblUrl);
            this.Controls.Add(this.bOK);
            this.Controls.Add(this.bCancel);
            this.Controls.Add(this.lbVariables);
            this.Controls.Add(this.lblVariables);
            this.MinimizeBox = false;
            this.MinimumSize = new System.Drawing.Size(630, 300);
            this.Name = "EditVariablesDialog";
            this.SavePosition = true;
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.Text = "Edit Variables";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label lblVariables;
        private VariableListBox lbVariables;
        private System.Windows.Forms.Button bCancel;
        private System.Windows.Forms.Button bOK;
        private System.Windows.Forms.Label lblUrl;
        private VariableTextBox txtUrl;
        private System.Windows.Forms.RichTextBox rtfContent;
        private System.Windows.Forms.Button bLoad;
        private System.Windows.Forms.Button bUseAsStart;
        private System.Windows.Forms.Button bUseAsEnd;
        private System.Windows.Forms.Button bAdd;
        private System.Windows.Forms.Button bRemove;
        private System.Windows.Forms.Button bFind;
        private System.Windows.Forms.Label lblFind;
        private System.Windows.Forms.TextBox txtFind;
        private System.Windows.Forms.Label lblDesc;
        private System.Windows.Forms.TextBox txtRegularExpression;
        private System.Windows.Forms.Label lblRegex;
        private System.Windows.Forms.RadioButton rbContentUrlStartEnd;
        private System.Windows.Forms.RadioButton rbContentUrlRegex;
        private System.Windows.Forms.RadioButton rbContentText;
        private System.Windows.Forms.ContextMenu cmuRtf;
        private System.Windows.Forms.MenuItem cmnuCopy;
        private System.Windows.Forms.MenuItem cmnuCopyMatch;
        private System.Windows.Forms.MenuItem cmnuGoToMatch;
        private System.Windows.Forms.CheckBox chkRightToLeft;
        private System.Windows.Forms.Button bPostData;
        private System.Windows.Forms.MenuItem sepPreview;
        private System.Windows.Forms.MenuItem cmnuBrowser;
        private System.Windows.Forms.MenuItem cmnuPaste;
    }
}
