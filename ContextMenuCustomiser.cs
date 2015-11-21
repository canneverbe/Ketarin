using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using Microsoft.Win32;

namespace Ketarin.Forms
{
    /// <summary>
    /// Allows adding custom items to exiting native Win32 context menus.
    /// </summary>
    public class ContextMenuCustomiser : NativeWindow
    {
        private IntPtr m_PopupMenu = IntPtr.Zero;
        private readonly List<ContextMenuItem> m_Items = new List<ContextMenuItem>();
        private static short m_LastId = 111;

        /// <summary>
        /// This event is fired, when the context menu is being created.
        /// At this point, changes can still be applied to the menu structure.
        /// </summary>
        public event EventHandler ContextMenuCreating;

        #region WinAPI

        [DllImport("user32.dll", SetLastError = true)]
        static extern IntPtr FindWindow(string lpClassName, string lpWindowName);
        [DllImport("user32.dll")]
        static extern bool IsMenu(IntPtr hMenu);

        #endregion

        #region Properties

        /// <summary>
        /// Gets a list of the menu items which are to be
        /// added to the context menu.
        /// </summary>
        public List<ContextMenuItem> MenuItems
        {
            get
            {
                return m_Items;
            }
        }

        #endregion

        /// <summary>
        /// Initialises a new custom context menu for the given control.
        /// </summary>
        /// <param name="control">The control, whose context menu is to be modified.</param>
        public ContextMenuCustomiser(Control control)
        {
            AssignHandle(control.Handle);
        }

        /// <summary>
        /// Initialises a new custom context menu for the given control.
        /// </summary>
        /// <param name="control">The control, whose context menu is to be modified.</param>
        /// <param name="items">The items which are to be added</param>
        public ContextMenuCustomiser(Control control, ContextMenuItem[] items)
        {
            AssignHandle(control.Handle);
            m_Items.AddRange(items);
        }

        /// <summary>
        /// Determines a free ID which can be used for a new context menu item.
        /// </summary>
        internal static short GetNextId()
        {
            return ++m_LastId;
        }

        /// <summary>
        /// Checks whether or not a context menu is being opened,
        /// stores its handle and builds the custom menu.
        /// </summary>
        private void FindContextMenu()
        {
            IntPtr mnuHandle = FindWindow("#32768", null);
            if (m_PopupMenu != mnuHandle)
            {
                m_PopupMenu = mnuHandle;
                BuildContextMenu();
            }
        }

        /// <summary>
        /// Inserts all custom items into the context menu.
        /// </summary>
        private void BuildContextMenu()
        {
            const int MN_GETHMENU = 0x01E1;

            if (ContextMenuCreating != null)
            {
                ContextMenuCreating(this, null);
            }

            // Find menu window
            IntPtr mnuHwnd = User32.SendMessage(m_PopupMenu, MN_GETHMENU, IntPtr.Zero, IntPtr.Zero);
            
            if (IsMenu(mnuHwnd))
            {
                // Insert items
                foreach (ContextMenuItem item in m_Items)
                {
                    item.InsertMenuItem(mnuHwnd);
                }
            }
        }

        /// <summary>
        /// This function analyses the windows message ID and
        /// fires an event for a context menu item if necessary.
        /// </summary>
        protected void FireEvent(List<ContextMenuItem> items, int msg)
        {
            // Go through menu items and check if something matches the message ID
            foreach (ContextMenuItem item in items)
            {
                int itemMsg = User32.WM_USER + item.Id;
                if (itemMsg == msg)
                {
                    if (item.EventHandler != null)
                    {
                        item.EventHandler(item);
                        return;
                    }
                }
                // Go through sub menu items
                if (item.MenuItems != null)
                {
                    FireEvent(item.MenuItems, msg);
                }
            }
        }

        /// <summary>
        /// Some windows messages are being watched, in order to determine
        /// when to insert the custom items.
        /// </summary>
        [DebuggerStepThrough()]
        protected override void WndProc(ref Message m)
        {
            FireEvent(m_Items, m.Msg);

            switch (m.Msg)
            {
                case 0x2: // WM_DESTROY
                    this.ReleaseHandle();
                    break;

                case 0x20: // WM_SETCURSOR
                    // Look for menu after the context menu key (keyboard) is pressed
                    FindContextMenu();
                    break;
                case 0x215: // WM_CAPTURECHANGED
                    // Look for menu after rightclick
                    FindContextMenu();
                    break;
            }

            base.WndProc(ref m);
        }
    }
}
