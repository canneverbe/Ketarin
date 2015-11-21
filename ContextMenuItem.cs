using System;
using System.Collections.Generic;
using Microsoft.Win32;

namespace Ketarin.Forms
{
    /// <summary>
    /// Represents a custom item within a native context menu.
    /// </summary>
    public class ContextMenuItem
    {
        /// <summary>
        /// Delegate for a function, which handles the click event for a menu item.
        /// </summary>
        /// <param name="menuItem">Item which has been clicked</param>
        public delegate void ItemSelectHandler(ContextMenuItem menuItem);

        private readonly short m_Id;
        private int m_Position = -1;
        private string m_Text;
        private readonly List<ContextMenuItem> m_Items = new List<ContextMenuItem>();

        #region Properties

        /// <summary>
        /// User defined object which belongs to the menu item.
        /// </summary>
        public object Tag { get; set; }

        /// <summary>
        /// Gets the list of sub menu items.
        /// </summary>
        public List<ContextMenuItem> MenuItems
        {
            get
            {
                return m_Items;
            }
        }

        /// <summary>
        /// Gets the ID of the menu item. The value can be used
        /// to identify a menu item.
        /// </summary>
        public short Id
        {
            get
            {
                return m_Id;
            }
        }

        /// <summary>
        /// Gets or sets the position within the menu.
        /// -1 is at the end, 0 at the start.
        /// </summary>
        public int Position
        {
            get
            {
                return m_Position;
            }
            set
            {
                m_Position = value;
            }
        }

        /// <summary>
        /// Gets or sets the textual representation of the menu item.
        /// An empty string represents a separator.
        /// </summary>
        public string Text
        {
            get
            {
                return m_Text;
            }
            set
            {
                m_Text = value;
            }
        }

        /// <summary>
        /// Gets or sets the function, which is executed when the
        /// item is clicked.
        /// </summary>
        public ItemSelectHandler EventHandler { get; set; }

        #endregion

        /// <summary>
        /// Creates a new context menu item with the given text and given position.
        /// </summary>
        public ContextMenuItem(string text, int position) : this(text)
        {
            m_Position = position;
        }

        /// <summary>
        /// Creates a new context menu item with the given text. Inserted at the end.
        /// </summary>
        public ContextMenuItem(string text)
        {
            m_Text = text;
            m_Id = ContextMenuCustomiser.GetNextId();
        }

        /// <summary>
        /// Inserts this menu item and all sub items into the given menu.
        /// </summary>
        /// <param name="hmenu">Handle of a Win32 context menu</param>
        internal void InsertMenuItem(IntPtr hmenu)
        {
            if (string.IsNullOrEmpty(Text) || Text == "-")
            {
                // Separator
                User32.InsertMenu(hmenu, Position, (uint)(User32.MenuFlags.MF_BYPOSITION | User32.MenuFlags.MF_SEPARATOR), new IntPtr(User32.WM_USER + Id), null);
            }
            else
            {
                // Do we have submenus?
                if (m_Items.Count > 0)
                {
                    IntPtr subMenuItem = User32.CreatePopupMenu();
                    foreach (ContextMenuItem item in m_Items)
                    {
                        item.InsertMenuItem(subMenuItem);
                    }
                    User32.InsertMenu(hmenu, Position, (uint)(User32.MenuFlags.MF_BYPOSITION | User32.MenuFlags.MF_POPUP), subMenuItem, Text);
                }
                else
                {
                    User32.InsertMenu(hmenu, Position, (uint)User32.MenuFlags.MF_BYPOSITION, new IntPtr(User32.WM_USER + Id), Text);
                }                
            }
        }
    }
}
