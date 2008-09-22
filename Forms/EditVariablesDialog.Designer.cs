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
            this.lbVariables = new System.Windows.Forms.ListBox();
            this.bCancel = new System.Windows.Forms.Button();
            this.bOK = new System.Windows.Forms.Button();
            this.lblUrl = new System.Windows.Forms.Label();
            this.txtUrl = new System.Windows.Forms.TextBox();
            this.rtfContent = new System.Windows.Forms.RichTextBox();
            this.bLoad = new System.Windows.Forms.Button();
            this.bUseAsStart = new System.Windows.Forms.Button();
            this.bUseAsEnd = new System.Windows.Forms.Button();
            this.bAdd = new System.Windows.Forms.Button();
            this.bRemove = new System.Windows.Forms.Button();
            this.bFind = new System.Windows.Forms.Button();
            this.lblFind = new System.Windows.Forms.Label();
            this.txtFind = new System.Windows.Forms.TextBox();
            this.lblDesc = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // lblVariables
            // 
            this.lblVariables.AutoSize = true;
            this.lblVariables.Location = new System.Drawing.Point(12, 56);
            this.lblVariables.Name = "lblVariables";
            this.lblVariables.Size = new System.Drawing.Size(53, 13);
            this.lblVariables.TabIndex = 0;
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
            this.lbVariables.TabIndex = 1;
            this.lbVariables.SelectedIndexChanged += new System.EventHandler(this.lbVariables_SelectedIndexChanged);
            // 
            // bCancel
            // 
            this.bCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.bCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.bCancel.Location = new System.Drawing.Point(525, 400);
            this.bCancel.Name = "bCancel";
            this.bCancel.Size = new System.Drawing.Size(75, 23);
            this.bCancel.TabIndex = 21;
            this.bCancel.Text = "Cancel";
            this.bCancel.UseVisualStyleBackColor = true;
            // 
            // bOK
            // 
            this.bOK.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.bOK.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.bOK.Location = new System.Drawing.Point(444, 400);
            this.bOK.Name = "bOK";
            this.bOK.Size = new System.Drawing.Size(75, 23);
            this.bOK.TabIndex = 20;
            this.bOK.Text = "OK";
            this.bOK.UseVisualStyleBackColor = true;
            // 
            // lblUrl
            // 
            this.lblUrl.AutoSize = true;
            this.lblUrl.Enabled = false;
            this.lblUrl.Location = new System.Drawing.Point(123, 75);
            this.lblUrl.Name = "lblUrl";
            this.lblUrl.Size = new System.Drawing.Size(100, 13);
            this.lblUrl.TabIndex = 2;
            this.lblUrl.Text = "&Contents from URL:";
            // 
            // txtUrl
            // 
            this.txtUrl.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.txtUrl.Enabled = false;
            this.txtUrl.Location = new System.Drawing.Point(247, 72);
            this.txtUrl.Name = "txtUrl";
            this.txtUrl.Size = new System.Drawing.Size(290, 20);
            this.txtUrl.TabIndex = 3;
            this.txtUrl.TextChanged += new System.EventHandler(this.txtUrl_TextChanged);
            // 
            // rtfContent
            // 
            this.rtfContent.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.rtfContent.BackColor = System.Drawing.SystemColors.Window;
            this.rtfContent.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.rtfContent.DetectUrls = false;
            this.rtfContent.Enabled = false;
            this.rtfContent.Font = new System.Drawing.Font("Courier New", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.rtfContent.HideSelection = false;
            this.rtfContent.Location = new System.Drawing.Point(126, 124);
            this.rtfContent.Name = "rtfContent";
            this.rtfContent.ReadOnly = true;
            this.rtfContent.Size = new System.Drawing.Size(474, 251);
            this.rtfContent.TabIndex = 8;
            this.rtfContent.Text = "";
            this.rtfContent.WordWrap = false;
            this.rtfContent.SelectionChanged += new System.EventHandler(this.rtfContent_SelectionChanged);
            this.rtfContent.TextChanged += new System.EventHandler(this.rtfContent_TextChanged);
            // 
            // bLoad
            // 
            this.bLoad.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.bLoad.Enabled = false;
            this.bLoad.Location = new System.Drawing.Point(543, 70);
            this.bLoad.Name = "bLoad";
            this.bLoad.Size = new System.Drawing.Size(57, 23);
            this.bLoad.TabIndex = 4;
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
            this.bUseAsStart.TabIndex = 11;
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
            this.bUseAsEnd.TabIndex = 12;
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
            this.bAdd.TabIndex = 9;
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
            this.bRemove.TabIndex = 10;
            this.bRemove.Text = "—";
            this.bRemove.UseVisualStyleBackColor = true;
            this.bRemove.Click += new System.EventHandler(this.bRemove_Click);
            // 
            // bFind
            // 
            this.bFind.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.bFind.Enabled = false;
            this.bFind.Location = new System.Drawing.Point(543, 95);
            this.bFind.Name = "bFind";
            this.bFind.Size = new System.Drawing.Size(57, 23);
            this.bFind.TabIndex = 7;
            this.bFind.Text = "&Find";
            this.bFind.UseVisualStyleBackColor = true;
            this.bFind.Click += new System.EventHandler(this.bFind_Click);
            // 
            // lblFind
            // 
            this.lblFind.AutoSize = true;
            this.lblFind.Enabled = false;
            this.lblFind.Location = new System.Drawing.Point(123, 101);
            this.lblFind.Name = "lblFind";
            this.lblFind.Size = new System.Drawing.Size(118, 13);
            this.lblFind.TabIndex = 5;
            this.lblFind.Text = "&Search within contents:";
            // 
            // txtFind
            // 
            this.txtFind.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.txtFind.Enabled = false;
            this.txtFind.Location = new System.Drawing.Point(247, 98);
            this.txtFind.Name = "txtFind";
            this.txtFind.Size = new System.Drawing.Size(290, 20);
            this.txtFind.TabIndex = 6;
            // 
            // lblDesc
            // 
            this.lblDesc.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.lblDesc.Location = new System.Drawing.Point(12, 9);
            this.lblDesc.Name = "lblDesc";
            this.lblDesc.Size = new System.Drawing.Size(588, 42);
            this.lblDesc.TabIndex = 0;
            this.lblDesc.Text = resources.GetString("lblDesc.Text");
            // 
            // EditVariablesDialog
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(612, 435);
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
            this.MinimumSize = new System.Drawing.Size(600, 300);
            this.Name = "EditVariablesDialog";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.Text = "Edit Variables";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label lblVariables;
        private System.Windows.Forms.ListBox lbVariables;
        private System.Windows.Forms.Button bCancel;
        private System.Windows.Forms.Button bOK;
        private System.Windows.Forms.Label lblUrl;
        private System.Windows.Forms.TextBox txtUrl;
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
    }
}