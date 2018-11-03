using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;
using CDBurnerXP.Forms;
using CDBurnerXP.IO;

namespace CDBurnerXP.Controls
{
    public partial class ListBoxPanel : UserControl
    {
        #region Transparent label
        
        public sealed class TransparentLabel : Label
        {
            private bool controlScaled;

            [DefaultValue(false)]
            public bool AutoHeight { get; set; }

            public TransparentLabel()
            {
                this.BackColor = Color.FromArgb(0, this.BackColor);
            }

            protected override void OnResize(EventArgs e)
            {
                base.OnResize(e);
                this.SetHeightAutomatically();
            }

            protected override void ScaleControl(SizeF factor, BoundsSpecified specified)
            {
                base.ScaleControl(factor, specified);

                this.controlScaled = true;
            }

            private void SetHeightAutomatically()
            {
                if (this.AutoHeight && !string.IsNullOrEmpty(this.Text) && this.controlScaled)
                {
                    Size preferred = this.GetPreferredSize(this.Size);
                    this.Height = preferred.Height;
                }
            }

            int WM_GETTEXT = 0xD;
            int WM_LBUTTONDBLCLK = 0x203;
            bool doubleclickflag = false;

            /// <summary>
            /// Double clicking a .NET label copies its content to the clipboard.
            /// Try to avoid this behaviour.
            /// </summary>
            protected override void WndProc(ref Message m)
            {
                if (m.Msg == WM_LBUTTONDBLCLK)
                {
                    doubleclickflag = true;
                }
                if (m.Msg == WM_GETTEXT && doubleclickflag)
                {
                    object currentClipboardContent = SafeClipboard.GetData(DataFormats.Text);
                    if (currentClipboardContent != null)
                    {
                        this.BeginInvoke((MethodInvoker) delegate()
                        {
                            SafeClipboard.SetData(currentClipboardContent, true);
                        });
                    }
                    doubleclickflag = false;
                    return;
                }
                base.WndProc(ref m);
            }
        }

        #endregion

        private bool m_Selected = false;

        #region Properties

        [DefaultValue(false)]
        public bool Selected
        {
            get
            {
                return m_Selected;
            }
            set
            {
                if (m_Selected == value) return;

                m_Selected = value;
                if (m_Selected)
                {
                    // Only if a neighbouring ListBoxPanel got the focus,
                    // another one loses the focus. So basically, we have 
                    // HideSelection = false
                    if (Parent != null)
                    {
                        foreach (ListBoxPanel panel in Parent.Controls)
                        {
                            if (panel != this) panel.Selected = false;
                        }
                    }
                }

                // Prevent flicker and the like
                using (new ControlRedrawLock(this.Parent))
                {
                    // Brute force scrollbar update
                    this.Parent.Width += 1;
                    this.Parent.Width -= 1;

                    SetFontColor(this);
                }
            }
        }
       

        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public new Point Location {
            get
            {
                return base.Location;
            }
        }

        [Browsable(false),DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public new int TabIndex
        {
            get
            {
                return 0;
            }
        }

        #endregion

        public ListBoxPanel()
        {
            InitializeComponent();
            SetStyle(ControlStyles.OptimizedDoubleBuffer | ControlStyles.AllPaintingInWmPaint, true);
            GetAllMouseDowns(Controls);
            SetFontColor(this);
        }

        protected void SetFontColor(Control control)
        {
            if (!(control is Button))
            {
                control.ForeColor = (Selected && FocusedOrChildFocused(this)) ? SystemColors.HighlightText : SystemColors.ControlText;
            }
            else
            {
                control.ForeColor = SystemColors.ControlText;
            }

            foreach (Control c in control.Controls)
            {
                SetFontColor(c);
            }
        }

        #region Child Events

        /// <summary>
        /// This function makes sure, that all child controls
        /// send the necessary events to the owner control.
        /// </summary>
        public void GetAllMouseDowns(ControlCollection controls)
        {
            if (DesignMode) return;

            foreach (Control control in controls)
            {
                control.MouseDown += new MouseEventHandler(OnChildMouseDown);
                control.DoubleClick += new EventHandler(OnChildDoubleClick);
                GetAllMouseDowns(control.Controls);
            }
        }

        void OnChildGotFocus(object sender, EventArgs e)
        {
            Selected = true;
            SetFontColor(this);
            Invalidate();
        }

        void OnChildLostFocus(object sender, EventArgs e)
        {
            SetFontColor(this);
            Invalidate();
        }

        void OnChildDoubleClick(object sender, EventArgs e)
        {
            OnDoubleClick(e);
        }

        void OnChildMouseDown(object sender, MouseEventArgs e)
        {
            Selected = true;
            Focus();
            OnMouseDown(e);
        }

        protected override void OnControlAdded(ControlEventArgs e)
        {
            base.OnControlAdded(e);

            e.Control.GotFocus += new EventHandler(OnChildGotFocus);
            e.Control.LostFocus += new EventHandler(OnChildLostFocus);
        }

        protected override void OnControlRemoved(ControlEventArgs e)
        {
            base.OnControlRemoved(e);

            e.Control.GotFocus -= new EventHandler(OnChildGotFocus);
            e.Control.LostFocus -= new EventHandler(OnChildLostFocus);
        }

        #endregion

        public bool FocusedOrChildFocused(Control controlToCheck)
        {
            if (controlToCheck.Focused) return true;
            foreach (Control c in controlToCheck.Controls)
            {
                if (FocusedOrChildFocused(c)) return true;
            }
            return false;
        }

        protected override void OnPaintBackground(PaintEventArgs e)
        {
            if (Selected)
            {
                Brush brush = null;
                if (FocusedOrChildFocused(this))
                {
                    brush = new LinearGradientBrush(new Point(0, 0), new Point(0, Height * 3), SystemColors.Highlight, SystemColors.Window);
                }
                else
                {
                    brush = new SolidBrush(SystemColors.Control);
                }
                e.Graphics.FillRectangle(brush, ClientRectangle);
                brush.Dispose();
            }
            else
            {
                e.Graphics.Clear(BackColor);
            }

            // The last panel should not draw the bottom line
            if (Parent != null)
            {
                AdvancedListBox advancedList = Parent.Parent as AdvancedListBox;
                if (advancedList != null && advancedList.IsPanelLast(this)) return;
            }

            Point p1 = new Point(0, Height - 1);
            Point p2 = new Point(Width, Height - 1);

            using (Pen p = new Pen(Color.LightGray, 1))
            {
                p.DashStyle = DashStyle.Dot;
                e.Graphics.DrawLine(p, p1, p2);
            }   
        }

        protected override void OnMouseDown(MouseEventArgs e)
        {
            base.OnMouseDown(e);
            Selected = true;
            SetFontColor(this);
            Invalidate();
        }

        protected override void OnGotFocus(EventArgs e)
        {
            base.OnGotFocus(e);
            Selected = true;
            SetFontColor(this);
            Invalidate();
        }

        protected override void OnLostFocus(EventArgs e)
        {
            base.OnLostFocus(e);
            SetFontColor(this);
            Invalidate();
        }

        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            switch (keyData)
            {
                case Keys.Up:
                    Parent.SelectNextControl(this, false, true, false, true);
                    return true;

                case Keys.Down:
                    Parent.SelectNextControl(this, true, true, false, true);
                    return true;
            }
            return base.ProcessCmdKey(ref msg, keyData);
        }
    }
}
