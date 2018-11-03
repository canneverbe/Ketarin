using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel;
using System.Windows.Forms;
using System.Drawing;
using System.Runtime.InteropServices;
using System.ComponentModel.Design;

namespace CDBurnerXP.Controls
{
    public partial class VistaMenu : System.ComponentModel.Component, IExtenderProvider, ISupportInitialize
    {
        [DllImport("user32.dll", CharSet = CharSet.Unicode)]
        private static extern int SendMessage(HandleRef hWnd, int Msg, IntPtr wParam, IntPtr lParam);


        ContainerControl ownerForm = null;

        //conditionally draw the little lines under menu items with keyboard accelators on Win 2000+
        private bool isUsingKeyboardAccel = false;


        public VistaMenu(ContainerControl parentControl)
            : this()
        {
            this.ownerForm = parentControl;
        }
        public ContainerControl ContainerControl
        {
            get { return this.ownerForm; }
            set { this.ownerForm = value; }
        }
        public override ISite Site
        {
            set
            {
                // Runs at design time, ensures designer initializes ContainerControl
                base.Site = value;
                if (value == null) return;
                IDesignerHost service = value.GetService(typeof(IDesignerHost)) as IDesignerHost;
                if (service == null) return;
                IComponent rootComponent = service.RootComponent;
                this.ContainerControl = rootComponent as ContainerControl;
            }
        }


        void PreVistaMenuItem_Popup(object sender, EventArgs e)
        {
            if (ownerForm == null)
            {
                isUsingKeyboardAccel = true;
                return;
            }


            //#define WM_QUERYUISTATE                 0x0129
            //int ret = SendMessage(new HandleRef(((Menu)sender).GetMainMenu().GetForm(), ((Menu)sender).GetMainMenu().GetForm().Handle), 0x0129, IntPtr.Zero, IntPtr.Zero);
            int ret = SendMessage(new HandleRef(ownerForm, ownerForm.Handle), 0x0129, IntPtr.Zero, IntPtr.Zero);
            

            /*
             The return value is NULL if the focus indicators and the keyboard accelerators are visible.
             Otherwise, the return value can be one or more of the following values:
             
                UISF_HIDEFOCUS	Focus indicators are hidden.
                UISF_HIDEACCEL	Keyboard accelerators are hidden.
                UISF_ACTIVE	Windows XP: A control should be drawn in the style used for active controls.
             */

            //#define UISF_HIDEACCEL                  0x2

            if ((ret & 0x2) != 0)
                isUsingKeyboardAccel = false;
            else
                isUsingKeyboardAccel = true;
        }


        const int SEPARATOR_HEIGHT = 8;
        const int BORDER_VERTICAL = 2;
        const int LEFT_MARGIN = 4;
        const int RIGHT_MARGIN = 6;
        const int SHORTCUT_MARGIN = 20;
        const int ARROW_MARGIN = 12;
        const int ICON_SIZE = 16;


        void MenuItem_MeasureItem(object sender, MeasureItemEventArgs e)
        {
            if (((MenuItem)sender).Text == "-")
                e.ItemHeight = SEPARATOR_HEIGHT;
            else
            {
                e.ItemHeight = (SystemFonts.MenuFont.Height > ICON_SIZE) ? SystemFonts.MenuFont.Height : ICON_SIZE;
                e.ItemHeight += BORDER_VERTICAL;
            }


            Font font = ((MenuItem)sender).DefaultItem ? new Font(SystemFonts.MenuFont, FontStyle.Bold) : SystemFonts.MenuFont;
            e.ItemWidth = LEFT_MARGIN + ICON_SIZE + RIGHT_MARGIN 
                
                //item text width
                + TextRenderer.MeasureText(((MenuItem)sender).Text, font, new Size(0, 0), TextFormatFlags.SingleLine).Width 
                + SHORTCUT_MARGIN
                
                //shortcut text width
                + TextRenderer.MeasureText(ShortcutToString(((MenuItem)sender).Shortcut), SystemFonts.MenuFont, new Size(0, 0), TextFormatFlags.SingleLine).Width 

                //arrow width
                + ((((MenuItem)sender).IsParent) ? ARROW_MARGIN : 0);
        }

        void MenuItem_DrawItem(object sender, DrawItemEventArgs e)
        {
            //MenuItem menuItem = (MenuItem)sender;
            //MenuHelper menuHelper = new MenuHelper(menuItem, e.Graphics, this);
            bool menuSelected = (e.State & DrawItemState.Selected) == DrawItemState.Selected;

            if (menuSelected)
                e.Graphics.FillRectangle(SystemBrushes.Highlight, e.Bounds);
            else
                e.Graphics.FillRectangle(SystemBrushes.Menu, e.Bounds);

            if (((MenuItem)sender).Text == "-")
            {
                //draw the separator
                int yCenter = e.Bounds.Top + (e.Bounds.Height / 2);

                e.Graphics.DrawLine(SystemPens.ControlDark, e.Bounds.Left, yCenter, (e.Bounds.Left + e.Bounds.Width), yCenter);
            }
            else //regular menu items
            {
                //draw the item text
                DrawText(sender, e, menuSelected);

                if (((MenuItem)sender).Checked)
                {
                    if (((MenuItem)sender).RadioCheck)
                    {
                        //draw the bullet
                        ControlPaint.DrawMenuGlyph(e.Graphics,
                            e.Bounds.Left + (LEFT_MARGIN + ICON_SIZE + RIGHT_MARGIN - SystemInformation.MenuCheckSize.Width) / 2,
                            e.Bounds.Top + (e.Bounds.Height - SystemInformation.MenuCheckSize.Height) / 2 + 1,
                            SystemInformation.MenuCheckSize.Width,
                            SystemInformation.MenuCheckSize.Height,
                            MenuGlyph.Bullet,
                            menuSelected ? SystemColors.HighlightText : SystemColors.MenuText,
                            menuSelected ? SystemColors.Highlight : SystemColors.Menu);
                    }
                    else
                    {
                        //draw the check mark
                        ControlPaint.DrawMenuGlyph(e.Graphics,
                            e.Bounds.Left + (LEFT_MARGIN + ICON_SIZE + RIGHT_MARGIN - SystemInformation.MenuCheckSize.Width) / 2,
                            e.Bounds.Top + (e.Bounds.Height - SystemInformation.MenuCheckSize.Height) / 2 + 1,
                            SystemInformation.MenuCheckSize.Width,
                            SystemInformation.MenuCheckSize.Height,
                            MenuGlyph.Checkmark,
                            menuSelected ? SystemColors.HighlightText : SystemColors.MenuText,
                            menuSelected ? SystemColors.Highlight : SystemColors.Menu);
                    }
                }
                else
                {
                    Image drawImg = GetImage(((MenuItem)sender));

                    if (drawImg != null)
                    {
                        //draw the image
                        if (((MenuItem)sender).Enabled)
                            e.Graphics.DrawImage(drawImg, e.Bounds.Left + LEFT_MARGIN,
                                e.Bounds.Top + ((e.Bounds.Height - ICON_SIZE) / 2),
                                ICON_SIZE, ICON_SIZE);
                        else
                            ControlPaint.DrawImageDisabled(e.Graphics, drawImg,
                                e.Bounds.Left + LEFT_MARGIN,
                                e.Bounds.Top + ((e.Bounds.Height - ICON_SIZE) / 2),
                                SystemColors.Menu);
                    }
                }
            }
        }


        private string ShortcutToString(Shortcut shortcut)
        {
            if (shortcut != Shortcut.None)
            {
                Keys keys = (Keys)shortcut;
                return System.ComponentModel.TypeDescriptor.GetConverter(keys.GetType()).ConvertToString(keys);
            }

            return null;
        }

        private void DrawText(object sender, DrawItemEventArgs e, bool isSelected)
        {
            string shortcutText = ShortcutToString(((MenuItem)sender).Shortcut);

            int yPos = e.Bounds.Top + (e.Bounds.Height - SystemFonts.MenuFont.Height) / 2;
            Size textSize;

            Font font = ((MenuItem)sender).DefaultItem ? new Font(SystemFonts.MenuFont, FontStyle.Bold) : SystemFonts.MenuFont;
            textSize = TextRenderer.MeasureText(((MenuItem)sender).Text, font, new Size(0, 0), TextFormatFlags.SingleLine);

            //Draw the menu item text
            if (((MenuItem)sender).Enabled)
            {
                TextRenderer.DrawText(e.Graphics, ((MenuItem)sender).Text, SystemFonts.MenuFont,
                    new Rectangle(e.Bounds.Left + LEFT_MARGIN + ICON_SIZE + RIGHT_MARGIN, yPos, textSize.Width, textSize.Height),
                    isSelected ? SystemColors.HighlightText : SystemColors.MenuText,
                    TextFormatFlags.SingleLine | (isUsingKeyboardAccel ? 0 : TextFormatFlags.HidePrefix));
            }
            else
            {
                ControlPaint.DrawStringDisabled(e.Graphics, ((MenuItem)sender).Text, SystemFonts.MenuFont, isSelected ? SystemColors.Highlight : SystemColors.Menu,
                    new Rectangle(e.Bounds.Left + LEFT_MARGIN + ICON_SIZE + RIGHT_MARGIN, yPos, textSize.Width, textSize.Height),
                    TextFormatFlags.SingleLine | (isUsingKeyboardAccel ? 0 : TextFormatFlags.HidePrefix));
            }

            //Draw the shortcut text
            if (shortcutText != null)
            {
                textSize = TextRenderer.MeasureText(shortcutText, SystemFonts.MenuFont, Size.Empty, TextFormatFlags.SingleLine);

                if (((MenuItem)sender).Enabled)
                {
                    TextRenderer.DrawText(e.Graphics, shortcutText, SystemFonts.MenuFont,
                        new Rectangle(e.Bounds.Width - textSize.Width - ARROW_MARGIN, yPos, textSize.Width, textSize.Height),
                        isSelected ? SystemColors.HighlightText : SystemColors.MenuText,
                        TextFormatFlags.SingleLine);
                }
                else
                {
                    ControlPaint.DrawStringDisabled(e.Graphics, shortcutText, SystemFonts.MenuFont, isSelected ? SystemColors.Highlight : SystemColors.Menu,
                        new Rectangle(e.Bounds.Width - textSize.Width - ARROW_MARGIN, yPos, textSize.Width, textSize.Height), TextFormatFlags.SingleLine);
                }
            }
        }
    }
}
