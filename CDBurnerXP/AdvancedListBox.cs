using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using System.Runtime.InteropServices;
using System.Windows.Forms.VisualStyles;
using CDBurnerXP.Forms;

namespace CDBurnerXP.Controls
{
    public partial class AdvancedListBox : UserControl
    {
        #region ItemChosenEventArgs

        public class ItemChosenEventArgs : EventArgs
        {
            private ListBoxPanel m_Panel = null;

            public ListBoxPanel ChosenPanel
            {
                get
                {
                    return m_Panel;
                }
            }

            public ItemChosenEventArgs(ListBoxPanel panel)
            {
                m_Panel = panel;
            }
        }

        #endregion

        #region Win32 API
        //the ReleaseDC function releases a device context (DC)
        [DllImport("user32.dll")]
        static extern int ReleaseDC(IntPtr hwnd, IntPtr hDC);
        [DllImport("user32.dll")]
        private static extern IntPtr GetWindowDC(IntPtr hwnd);
        private static int WM_NCPAINT = 0x0085;    // WM_NCPAINT message
        private static int WM_ERASEBKGND = 0x0014; // WM_ERASEBKGND message
        private static int WM_PAINT = 0x000F;      // WM_PAINT message

        public static bool IsUsingVisualStyles {
            get
            {
                try
                {
                    return VisualStyleInformation.IsSupportedByOS && VisualStyleInformation.IsEnabledByUser;
                }
                catch (MissingFieldException)
                {
                    return false;
                }
            }
        }
        #endregion

        private PanelCollection m_Panels = new PanelCollection();

        /// <summary>
        /// Occurs when an item is selected and the user hits Enter/Return,
        /// or the user double clicks on an item.
        /// </summary>
        public event EventHandler<ItemChosenEventArgs> ItemChosen;

        #region Properties

        public ListBoxPanel SelectedPanel
        {
            get
            {
                foreach (ListBoxPanel panel in m_Panels)
                {
                    if (panel.Selected) return panel;
                }
                return null;
            }
            set
            {
                foreach (ListBoxPanel panel in m_Panels)
                {
                    if (panel == value)
                    {
                        panel.Selected = true;
                        break;
                    }
                }
            }
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        [EditorAttribute(typeof(System.ComponentModel.Design.CollectionEditor), typeof(System.Drawing.Design.UITypeEditor))]
        public PanelCollection Panels
        {
            get
            {
                return m_Panels;
            }
            set
            {
                if (value == null)
                {
                    throw new ArgumentNullException("value", "The panel collection must not be null.");
                }

                if (value != m_Panels)
                {
                    tableLayout.RowCount = 0;
                    tableLayout.RowStyles.Clear();
                    tableLayout.Controls.Clear();
                    m_Panels = value;
                    foreach (ListBoxPanel panel in m_Panels) {
                        OnPanelAdd(panel);
                    }
                }
            }
        }

        #endregion

        public AdvancedListBox()
        {
            InitializeComponent();
            SetStyle(ControlStyles.OptimizedDoubleBuffer, true);
            SetStyle(ControlStyles.AllPaintingInWmPaint, true);
            SetStyle(ControlStyles.ResizeRedraw, true);
            m_Panels.PanelAdded += new EventHandler(m_Panels_PanelAdded);
            m_Panels.PanelRemoved += new EventHandler(m_Panels_PanelRemoved);
            this.tableLayout.ControlRemoved += new ControlEventHandler(tableLayout_ControlRemoved);
        }

        private void tableLayout_ControlRemoved(object sender, ControlEventArgs e)
        {
            ListBoxPanel panel = e.Control as ListBoxPanel;
            if (panel != null)
            {
                m_Panels.Remove(panel);
            }
        }

        private void m_Panels_PanelRemoved(object sender, EventArgs e)
        {
            ListBoxPanel panel = sender as ListBoxPanel;
            OnPanelRemove(panel);
        }

        private void m_Panels_PanelAdded(object sender, EventArgs e)
        {
            ListBoxPanel panel = sender as ListBoxPanel;
            OnPanelAdd(panel);            
        }

        protected virtual void OnItemChosen(ItemChosenEventArgs e)
        {
            if (ItemChosen != null)
            {
                ItemChosen(this, e);
            }
        }

        protected void OnPanelRemove(ListBoxPanel panel)
        {
            this.tableLayout.Controls.Remove(panel);
            Invalidate();
        }

        protected void OnPanelAdd(ListBoxPanel panel)
        {
            panel.Anchor = AnchorStyles.Left | AnchorStyles.Top | AnchorStyles.Right;
            this.tableLayout.Controls.Add(panel);
            // New rows will automatically be created
            panel.Width = Width;
            panel.DoubleClick += new EventHandler(OnPanelDoubleClick);

            // Prevent flicker and the like
            using (new ControlRedrawLock(this.Parent))
            {
                // Brute force scrollbar update
                this.Width += 1;
                this.Width -= 1;
            }
        }

        void OnPanelDoubleClick(object sender, EventArgs e)
        {
            OnItemChosen(new ItemChosenEventArgs(sender as ListBoxPanel));
        }

        protected override void WndProc(ref Message m)
        {
            if (m.Msg == WM_NCPAINT || m.Msg == WM_ERASEBKGND || m.Msg == WM_PAINT)
            {
                // This draws the same border for the control as textboxes have.
                // Without visual styles, it's a 3D border.
                if (IsUsingVisualStyles)
                {
                    BorderStyle = BorderStyle.FixedSingle;

                    IntPtr hDC = GetWindowDC(Handle);
                    using (Graphics g = Graphics.FromHdc(hDC))
                    {
                        int borderWidth = 1;
                        Color borderColor = VisualStyleInformation.TextControlBorder;

                        ControlPaint.DrawBorder(g, new Rectangle(0, 0, this.Width, this.Height),
                            borderColor, borderWidth, ButtonBorderStyle.Solid,
                            borderColor, borderWidth, ButtonBorderStyle.Solid,
                            borderColor, borderWidth, ButtonBorderStyle.Solid,
                            borderColor, borderWidth, ButtonBorderStyle.Solid);
                    }
                    ReleaseDC(Handle, hDC);
                }
                else
                {
                    BorderStyle = BorderStyle.Fixed3D;
                }
            }
            base.WndProc(ref m);
        }

        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            switch (keyData)
            {
                case Keys.Enter:
                    if (SelectedPanel != null)
                    {
                        OnItemChosen(new ItemChosenEventArgs(SelectedPanel));
                        return true;
                    }
                    break;

                case Keys.PageUp:
                    if (m_Panels.Count > 0)
                    {
                        SelectedPanel = m_Panels[0];
                        SelectedPanel.Focus();
                        ScrollToControl(SelectedPanel);
                    }
                    break;

                case Keys.PageDown:
                    if (m_Panels.Count > 0)
                    {
                        SelectedPanel = m_Panels[m_Panels.Count - 1];
                        SelectedPanel.Focus();
                        ScrollToControl(SelectedPanel);
                    }
                    break;
            }
            return base.ProcessCmdKey(ref msg, keyData);
        }

        internal bool IsPanelLast(ListBoxPanel panel)
        {
            if (m_Panels.Count == 0) return false;

            return m_Panels[m_Panels.Count - 1] == panel;
        }

        /// <summary>
        /// Recreates the list of panels according to the current control order.
        /// </summary>
        public PanelCollection GetPanels()
        {
            PanelCollection panels = new PanelCollection();

            foreach (Control control in tableLayout.Controls)
            {
                if (control is ListBoxPanel)
                {
                    panels.Add(control as ListBoxPanel);
                }
            }

            return panels;
        }
    }
}
