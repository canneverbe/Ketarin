using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using CDBurnerXP;

namespace Ketarin
{
    /// <summary>
    /// Represents an action, that can be invoked through a keyboard shortcut.
    /// </summary>
    internal class Hotkey
    {
        public static readonly Keys[] EndKeys = new Keys[] { Keys.A, Keys.B, Keys.C, Keys.D, Keys.E, Keys.F, Keys.G, Keys.H, Keys.I, Keys.J, Keys.K, Keys.L, Keys.M, Keys.N, Keys.O, Keys.P, Keys.Q, Keys.R, Keys.S, Keys.T, Keys.U, Keys.V, Keys.W, Keys.X, Keys.Y, Keys.Z, Keys.D0, Keys.D1, Keys.D2, Keys.D3, Keys.D4, Keys.D5, Keys.D6, Keys.D7, Keys.D8, Keys.D9, Keys.D0, Keys.F1, Keys.F2, Keys.F3, Keys.F4, Keys.F5, Keys.F6, Keys.F7, Keys.F8, Keys.F9, Keys.F10, Keys.F11, Keys.F12 };

        #region Properties

        /// <summary>
        /// Gets the human readable representation of the key combination required
        /// to invoke this hotkey.
        /// </summary>
        public string Shortcut { get; private set; }

        /// <summary>
        /// Gets or sets the name of the hotkey action.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the display name of the hotkey action.
        /// </summary>
        public string DisplayName { get; set; }

        #endregion

        /// <summary>
        /// Creates a new instance of the hotkey class.
        /// </summary>
        /// <param name="name">Name of the hotkey</param>
        /// <param name="displayName">Display name of the hotkey</param>
        public Hotkey(string name, string displayName)
        {
            this.Name = name;
            this.DisplayName = displayName;
        }

        /// <summary>
        /// Gets the human readable representation of the given keys.
        /// </summary>
        /// <param name="keys">Pressed keys</param>
        /// <returns>Keys separated by plus sign</returns>
        public static string GetShortcutString(Keys keys)
        {
            if (keys == Keys.None)
            {
                return string.Empty;
            }

            List<string> singleKeys = new List<string>();

            if ((keys & Keys.Control) == Keys.Control)
            {
                singleKeys.Add("Control");
                keys = keys ^ Keys.Control;
            }
            if ((keys & Keys.Alt) == Keys.Alt)
            {
                singleKeys.Add("Alt");
                keys = keys ^ Keys.Alt;
            }
            if ((keys & Keys.Shift) == Keys.Shift)
            {
                singleKeys.Add("Shift");
                keys = keys ^ Keys.Shift;
            }

            foreach (Keys key in EndKeys)
            {
                if (keys == key)
                {
                    singleKeys.Add(new KeysConverter().ConvertTo(key, typeof(string)) as string);
                }
            }

            return String.Join("+", singleKeys.ToArray());
        }

        /// <summary>
        /// Gets the human readable representation of the given keys + double click action.
        /// </summary>
        /// <param name="keys">Pressed keys</param>
        /// <returns>Keys separated by plus sign</returns>
        private string GetDoubleClickShortcutString(Keys keys)
        {
            return (GetShortcutString(keys) + "+Double-click").TrimStart('+');
        }

        /// <summary>
        /// Sets the key combination for this hotkey.
        /// </summary>
        /// <param name="keys">Keys required to press</param>
        public void SetKeyShortcut(System.Windows.Forms.Keys keys)
        {
            this.Shortcut = GetShortcutString(keys);
        }

        /// <summary>
        /// Sets the key/doubleclick combination for this hotkey.
        /// </summary>
        /// <param name="keys">Keys to hold, when executing a double click</param>
        public void SetDoubleclickShortcut(Keys keys)
        {
            this.Shortcut = GetDoubleClickShortcutString(keys);
        }

        /// <summary>
        /// Gets a list of all available hotkeys.
        /// </summary>
        /// <returns></returns>
        public static List<Hotkey> GetHotkeys()
        {
            List<Hotkey> hotkeys = new List<Hotkey>();
            hotkeys.Add(new Hotkey("OpenWebsite", "Open website"));
            hotkeys.Add(new Hotkey("Edit", "Edit application"));
            hotkeys.Add(new Hotkey("Update", "Update application"));
            hotkeys.Add(new Hotkey("ForceDownload", "Force download"));
            hotkeys.Add(new Hotkey("InstallSelected", "Install selected applications"));
            hotkeys.Add(new Hotkey("OpenFile", "Open file"));
            hotkeys.Add(new Hotkey("OpenFolder", "Open folder"));
            hotkeys.Add(new Hotkey("CheckUpdate", "Check for update"));
            hotkeys.Add(new Hotkey("UpdateAndInstall", "Update and install"));

            foreach (Hotkey hotkey in hotkeys)
            {
                hotkey.Shortcut = Settings.GetValue("Hotkey: " + hotkey.Name, string.Empty) as string;
            }

            return hotkeys;
        }

        /// <summary>
        /// Determines whether or not the hotkey is a match for the given key combination.
        /// </summary>
        /// <param name="keys">Keys pressed by user</param>
        public bool IsMatch(Keys keys)
        {
            if (string.IsNullOrEmpty(this.Shortcut)) return false;

            return (GetShortcutString(keys) == this.Shortcut);
        }

        /// <summary>
        /// Determines whether or not the hotkey is a match for the given key combination plus double click.
        /// </summary>
        /// <param name="keys">Keys pressed by user</param>
        public bool IsDoubleClickMatch(Keys keys)
        {
            if (string.IsNullOrEmpty(this.Shortcut)) return false;

            return (GetDoubleClickShortcutString(keys) == this.Shortcut);
        }
    }
}
