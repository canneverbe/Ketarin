// Ketarin - Copyright (C) 2008  Canneverbe Limited
// The icons used within the application are *NOT* licensed under the GNU General Public License.

// This program is free software; you can redistribute it and/or
// modify it under the terms of the GNU General Public License
// as published by the Free Software Foundation; either version 2
// of the License, or (at your option) any later version.

// This program is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.

// You should have received a copy of the GNU General Public License
// along with this program; if not, write to the Free Software
// Foundation, Inc., 51 Franklin Street, Fifth Floor, Boston, MA  02110-1301, USA.

using System;
using System.Collections.Generic;
using System.Windows.Forms;
using CDBurnerXP;

namespace Ketarin
{
    static class Program
    {
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            try
            {
                DbManager.LoadOrCreateDatabase();

                Settings.Provider = new DbManager.SettingsProvider();

                if (Settings.GetValue("AuthorGuid") == null)
                {
                    Settings.SetValue("AuthorGuid", Guid.NewGuid().ToString("B"));
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Could not create or load the database file: " + ex.Message);
                return;
            }

            Application.Run(new MainForm());
        }
    }
}
