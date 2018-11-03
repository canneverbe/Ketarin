using System.Drawing;
using System.Windows.Forms;
using System.ComponentModel;

namespace CDBurnerXP.Controls
{
    /// <summary>
    /// Represents a separation line with optional header text.
    /// </summary>
    [System.ComponentModel.DesignerCategory("Code")]
    public class Separator : Label
    {
        private int m_Offset = 4;

        /// <summary>
        /// Not implemented.
        /// </summary>
        [Browsable(false)]
        [EditorBrowsable(EditorBrowsableState.Never)]
        public override bool AutoSize
        {
            get { return false; }
            set { }
        }

        /// <summary>
        /// Gets or sets the foreground color of the control.
        /// </summary>
        [DefaultValue(typeof(Color), "ActiveCaption")]
        public override Color ForeColor
        {
            get { return base.ForeColor; }
            set { base.ForeColor = value; }
        }

        /// <summary>
        /// Gets or sets the alignment of text in the label.
        /// </summary>
        [DefaultValue(typeof(ContentAlignment), "MiddleLeft")]
        public override ContentAlignment TextAlign
        {
            get { return base.TextAlign; }
            set { base.TextAlign = value; }
        }

        /// <summary>
        /// Initializes a new instance of the Separator class.
        /// </summary>
        public Separator()
        {
            this.ForeColor = SystemColors.ActiveCaption;
            base.AutoSize = false;
            base.TextAlign = ContentAlignment.MiddleLeft;
        }

        /// <summary>
        /// Raises the Paint event.
        /// </summary>
        /// <param name="e"></param>
        protected override void OnPaint(System.Windows.Forms.PaintEventArgs e)
        {
            base.OnPaint(e);

            int stringWidth;

            if (Text.Length == 0)
            {
                m_Offset = 0;
                stringWidth = 0;
            }
            else
            {
                stringWidth = TextRenderer.MeasureText(e.Graphics, Text, Font).Width;
            }

            // First, draw the grey line
            Point startPoint = new Point(stringWidth + m_Offset, Height / 2);
            if (RightToLeft == RightToLeft.Yes)
            {
                startPoint.X = Width - startPoint.X;
            }

            Point endPoint = new Point(Width - 2, Height / 2);
            if (RightToLeft == RightToLeft.Yes)
            {
                endPoint.X = 2;
            }

            using (Pen p = new Pen(SystemColors.ButtonShadow, 1))
            {
                e.Graphics.DrawLine(p, startPoint, endPoint);
            }

            // Now the white line below
            startPoint.Y += 1;
            endPoint.Y += 1;
            using (Pen p = new Pen(SystemColors.ButtonHighlight, 1))
            {
                e.Graphics.DrawLine(p, startPoint, endPoint);
            }

            // Now the last piece, so that it looks like that
            // GGGGGGGGGGGGGGGGGGGGGGGGGGGW
            // WWWWWWWWWWWWWWWWWWWWWWWWWWWW
            if (RightToLeft == RightToLeft.Yes)
            {
                endPoint.X -= 1;
            }
            else
            {
                endPoint.X += 1;
            }

            startPoint = endPoint;
            endPoint.Y -= 1;
            using (Pen p = new Pen(SystemColors.ButtonHighlight, 1))
            {
                e.Graphics.DrawLine(p, startPoint, endPoint);
            }
        }
    }
}
