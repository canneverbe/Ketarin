using System.ComponentModel;
using System.Windows.Forms;
using CDBurnerXP.Controls;

namespace Ketarin.Forms
{
    partial class InstallingApplicationsDialog
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private IContainer components = null;

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
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(InstallingApplicationsDialog));
            this.bCancel = new System.Windows.Forms.Button();
            this.lblSetupStatus = new System.Windows.Forms.Label();
            this.progressBar = new CDBurnerXP.Controls.MarqueeProgressBar();
            this.bgwSetup = new System.ComponentModel.BackgroundWorker();
            this.lbShowHideDetails = new System.Windows.Forms.Label();
            this.imageList = new System.Windows.Forms.ImageList(this.components);
            this.olvLog = new CDBurnerXP.Controls.ObjectListView();
            this.colTime = new CDBurnerXP.Controls.OLVColumn();
            this.colMessage = new CDBurnerXP.Controls.OLVColumn();
            this.imlListIcons = new System.Windows.Forms.ImageList(this.components);
            this.lblEvents = new System.Windows.Forms.Label();
            this.pnlExpanded = new System.Windows.Forms.Panel();
            ((System.ComponentModel.ISupportInitialize)(this.olvLog)).BeginInit();
            this.pnlExpanded.SuspendLayout();
            this.SuspendLayout();
            // 
            // bCancel
            // 
            this.bCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.bCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.bCancel.Location = new System.Drawing.Point(313, 193);
            this.bCancel.Name = "bCancel";
            this.bCancel.Size = new System.Drawing.Size(75, 23);
            this.bCancel.TabIndex = 5;
            this.bCancel.Text = "Cancel";
            this.bCancel.UseVisualStyleBackColor = true;
            this.bCancel.Click += new System.EventHandler(this.bCancel_Click);
            // 
            // lblSetupStatus
            // 
            this.lblSetupStatus.AutoSize = true;
            this.lblSetupStatus.Location = new System.Drawing.Point(12, 9);
            this.lblSetupStatus.Name = "lblSetupStatus";
            this.lblSetupStatus.Size = new System.Drawing.Size(96, 13);
            this.lblSetupStatus.TabIndex = 0;
            this.lblSetupStatus.Text = "Installing appliction";
            // 
            // progressBar
            // 
            this.progressBar.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.progressBar.Location = new System.Drawing.Point(15, 25);
            this.progressBar.MarqueeAnimationSpeed = 50;
            this.progressBar.Name = "progressBar";
            this.progressBar.Size = new System.Drawing.Size(373, 18);
            this.progressBar.TabIndex = 1;
            // 
            // bgwSetup
            // 
            this.bgwSetup.WorkerSupportsCancellation = true;
            this.bgwSetup.DoWork += new System.ComponentModel.DoWorkEventHandler(this.bgwSetup_DoWork);
            this.bgwSetup.RunWorkerCompleted += new System.ComponentModel.RunWorkerCompletedEventHandler(this.bgwSetup_RunWorkerCompleted);
            // 
            // lbShowHideDetails
            // 
            this.lbShowHideDetails.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.lbShowHideDetails.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.lbShowHideDetails.ImageIndex = 3;
            this.lbShowHideDetails.ImageList = this.imageList;
            this.lbShowHideDetails.Location = new System.Drawing.Point(14, 193);
            this.lbShowHideDetails.Name = "lbShowHideDetails";
            this.lbShowHideDetails.Size = new System.Drawing.Size(94, 23);
            this.lbShowHideDetails.TabIndex = 4;
            this.lbShowHideDetails.Text = "        Show details";
            this.lbShowHideDetails.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.lbShowHideDetails.MouseLeave += new System.EventHandler(this.lbDetails_MouseLeave);
            this.lbShowHideDetails.Click += new System.EventHandler(this.lbDetails_Click);
            this.lbShowHideDetails.MouseDown += new System.Windows.Forms.MouseEventHandler(this.lbDetails_MouseUp);
            this.lbShowHideDetails.MouseUp += new System.Windows.Forms.MouseEventHandler(this.lbDetails_MouseUp);
            this.lbShowHideDetails.MouseEnter += new System.EventHandler(this.lbDetails_MouseEnter);
            // 
            // imageList
            // 
            this.imageList.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imageList.ImageStream")));
            this.imageList.TransparentColor = System.Drawing.Color.Fuchsia;
            this.imageList.Images.SetKeyName(0, "arrow_up_bw.bmp");
            this.imageList.Images.SetKeyName(1, "arrow_up_color.bmp");
            this.imageList.Images.SetKeyName(2, "arrow_up_color_pressed.bmp");
            this.imageList.Images.SetKeyName(3, "arrow_down_bw.bmp");
            this.imageList.Images.SetKeyName(4, "arrow_down_color.bmp");
            this.imageList.Images.SetKeyName(5, "arrow_down_color_pressed.bmp");
            // 
            // olvLog
            // 
            this.olvLog.AllColumns.Add(this.colTime);
            this.olvLog.AllColumns.Add(this.colMessage);
            this.olvLog.AlternateRowBackColor = System.Drawing.Color.Empty;
            this.olvLog.AlwaysGroupByColumn = null;
            this.olvLog.AlwaysGroupBySortOrder = System.Windows.Forms.SortOrder.None;
            this.olvLog.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.olvLog.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.colTime,
            this.colMessage});
            this.olvLog.FullRowSelect = true;
            this.olvLog.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.None;
            this.olvLog.HideSelection = false;
            this.olvLog.HighlightBackgroundColor = System.Drawing.Color.Empty;
            this.olvLog.HighlightForegroundColor = System.Drawing.Color.Empty;
            this.olvLog.LastSortColumn = null;
            this.olvLog.LastSortOrder = System.Windows.Forms.SortOrder.None;
            this.olvLog.Location = new System.Drawing.Point(0, 16);
            this.olvLog.MultiSelect = false;
            this.olvLog.Name = "olvLog";
            this.olvLog.ShowGroups = false;
            this.olvLog.ShowItemToolTips = true;
            this.olvLog.Size = new System.Drawing.Size(371, 116);
            this.olvLog.SmallImageList = this.imlListIcons;
            this.olvLog.TabIndex = 1;
            this.olvLog.UseCompatibleStateImageBehavior = false;
            this.olvLog.View = System.Windows.Forms.View.Details;
            // 
            // colTime
            // 
            this.colTime.AspectName = "Time";
            this.colTime.AspectToStringFormat = "{0:t}";
            this.colTime.Text = "Time";
            this.colTime.Width = 55;
            // 
            // colMessage
            // 
            this.colMessage.AspectName = "Message";
            this.colMessage.FillsFreeSpace = true;
            this.colMessage.Text = "Message";
            this.colMessage.Width = 350;
            // 
            // imlListIcons
            // 
            this.imlListIcons.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imlListIcons.ImageStream")));
            this.imlListIcons.TransparentColor = System.Drawing.Color.Transparent;
            this.imlListIcons.Images.SetKeyName(0, "Symbol Check.png");
            this.imlListIcons.Images.SetKeyName(1, "Symbol Warning 2.png");
            this.imlListIcons.Images.SetKeyName(2, "Symbol Error 2.png");
            // 
            // lblEvents
            // 
            this.lblEvents.AutoSize = true;
            this.lblEvents.Location = new System.Drawing.Point(-3, 0);
            this.lblEvents.Name = "lblEvents";
            this.lblEvents.Size = new System.Drawing.Size(43, 13);
            this.lblEvents.TabIndex = 0;
            this.lblEvents.Text = "&Events:";
            // 
            // pnlExpanded
            // 
            this.pnlExpanded.Controls.Add(this.lblEvents);
            this.pnlExpanded.Controls.Add(this.olvLog);
            this.pnlExpanded.Location = new System.Drawing.Point(17, 53);
            this.pnlExpanded.Name = "pnlExpanded";
            this.pnlExpanded.Size = new System.Drawing.Size(371, 132);
            this.pnlExpanded.TabIndex = 3;
            // 
            // InstallingApplicationsDialog
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(400, 228);
            this.Controls.Add(this.pnlExpanded);
            this.Controls.Add(this.lbShowHideDetails);
            this.Controls.Add(this.progressBar);
            this.Controls.Add(this.lblSetupStatus);
            this.Controls.Add(this.bCancel);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.MinimumSize = new System.Drawing.Size(400, 130);
            this.Name = "InstallingApplicationsDialog";
            this.SavePosition = true;
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Installing Applications";
            ((System.ComponentModel.ISupportInitialize)(this.olvLog)).EndInit();
            this.pnlExpanded.ResumeLayout(false);
            this.pnlExpanded.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private Button bCancel;
        private Label lblSetupStatus;
        private MarqueeProgressBar progressBar;
        private BackgroundWorker bgwSetup;
        private Label lbShowHideDetails;
        private ImageList imageList;
        private ObjectListView olvLog;
        private OLVColumn colMessage;
        private Label lblEvents;
        private Panel pnlExpanded;
        private ImageList imlListIcons;
        private OLVColumn colTime;
    }
}