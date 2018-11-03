using System;
using System.Collections.Generic;
using System.Text;
using System.Diagnostics;
using CDBurnerXP.Forms;
using System.Windows.Forms;
using System.ComponentModel;

namespace CDBurnerXP.Controls
{
    /// <summary>
    /// Due to the similar class name, this class will automatically be used
    /// for the ObjectListView.
    /// </summary>
    public class ListView : System.Windows.Forms.ListView, IMirrorControl
    {
        public new int VirtualListSize
        {
            get { return base.VirtualListSize; }
            set
            {
                // Must set top value to at least one less than value due to
                // off-by-one error in base.VirtualListSize
                int topIndex = this.TopItem == null ? 0 : this.TopItem.Index;
                topIndex = Math.Min(topIndex, Math.Abs(value - 1));
                if (topIndex < Items.Count)
                {
                    this.TopItem = this.Items[topIndex];
                }
                else
                {
                    this.TopItem = null;
                }

                try
                {
                    base.VirtualListSize = value;
                }
                catch (NullReferenceException)
                {
                    // .NET bug, ignore
                }
            }

        }

        [DebuggerStepThrough()]
        protected override void WndProc(ref System.Windows.Forms.Message m)
        {
            try
            {
                base.WndProc(ref m);
            }
            catch (InvalidOperationException)
            {
                // Bug in virtual list view
            }
        }

        #region IMirrorControl Member

        [DefaultValue(false)]
        public bool Mirror { get; set; }

        protected override CreateParams CreateParams
        {
            get
            {
                if (this.Mirror)
                {
                    CreateParams mirroredCp = base.CreateParams;
                    mirroredCp.ExStyle = mirroredCp.ExStyle | 0x400000;
                    return mirroredCp;
                }

                return base.CreateParams;
            }
        }

        #endregion
    }
}
