using System;
using System.Collections.Generic;
using System.Text;
using Ketarin.Forms;
using System.Windows.Forms.VisualStyles;
using System.ComponentModel;
using System.Windows.Forms;
using System.Drawing;

namespace Ketarin
{
    /// <summary>
    /// A text box which displays a gray coloured hint.
    /// </summary>
    class HintTextBox : PaintableTextBoxBase
    {
        private string hintText = string.Empty;
        private HorizontalAlign hintTextAlign = HorizontalAlign.Left;

        #region Properties

        /// <summary>
        /// Gets or sets the hint text do display in the text box.
        /// </summary>
        [DefaultValue("")]
        public string HintText
        {
            get
            {
                return this.hintText;
            }
            set
            {
                if (this.hintText != value)
                {
                    this.hintText = value;
                    Invalidate();
                }
            }
        }

        /// <summary>
        /// Gets or sets the location of the hint text.
        /// </summary>
        [DefaultValue(HorizontalAlign.Left)]
        public HorizontalAlign HintTextAlign
        {
            get
            {
                return this.hintTextAlign;
            }
            set
            {
                if (this.hintTextAlign != value)
                {
                    this.hintTextAlign = value;
                    Invalidate();
                }
            }
        }

        #endregion

        protected override void OnPaint(System.Windows.Forms.PaintEventArgs e)
        {
            base.OnPaint(e);


            TextFormatFlags flags = (this.hintTextAlign == HorizontalAlign.Left) ? TextFormatFlags.Left : TextFormatFlags.Right;
            TextRenderer.DrawText(e.Graphics, this.hintText, this.Font, new Rectangle(0, 1, Width, Height), Color.Gray, flags);
        }
    }
}
