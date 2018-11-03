using System;
using Microsoft.Win32;
using System.Xml.Serialization;
using System.IO;
using System.Text;
using System.Runtime.Serialization.Formatters.Binary;
using CDBurnerXP.IO;

namespace CDBurnerXP {

    public interface ISettingsProvider
    {
        object GetValue(params string[] path);

        void SetValue(string value, params string[] path);
    }

    public class RegistrySettingsProvider : ISettingsProvider
    {
        private string m_BaseKey = "Software";

        public RegistrySettingsProvider(string baseKey)
        {
            m_BaseKey = baseKey;
        }

        /// <summary>
        /// Returns the key HKCU/Software/CDBurnerXP
        /// </summary>
        private RegistryKey GetHkcuKey()
        {
            RegistryKey key = Registry.CurrentUser;
            key = key.CreateSubKey(m_BaseKey);
            return key;
        }

        /// <summary>
        /// Gibt den Regsitry-Key zurück, dessen Wert wir setzen müssen. Erstellt ihn, wenn nötig.
        /// </summary>
        /// <param name="section">darf null sein, und wird dann ignoriert</param>
        /// <returns></returns>
        private RegistryKey GetSettingsRegistryKey(string[] path)
        {
            RegistryKey settingsKey = GetHkcuKey();

            // No subkey required
            if (path.Length <= 1) return settingsKey;

            for (int i = 0; i < path.Length - 1; i++)
            {
                settingsKey = settingsKey.CreateSubKey(path[i]);
            }

            return settingsKey;
        }


        #region ISettingsProvider Member

        public object GetValue(params string[] path)
        {
            return GetSettingsRegistryKey(path).GetValue(path[path.Length - 1]);
        }

        public void SetValue(string value, params string[] path)
        {
            try
            {
                GetSettingsRegistryKey(path).SetValue(path[path.Length - 1], value);
            }
            catch (UnauthorizedAccessException ex)
            {
                // Ignore write errors. If settings cannot be stored, so be it.
                Debug.LogError(ex);
            }
        }

        #endregion
    }

    /// <summary>
    /// Speichert Einstellungen eines Programms in der Registry.
    /// Inbesondere gedacht für Fensterpositionen, den Zustand von
    /// Controls etc.
    /// </summary>
    public class Settings {

        private static ISettingsProvider m_Provider = new RegistrySettingsProvider("Software\\Canneverbe Limited");

        public static ISettingsProvider Provider
        {
            get { return m_Provider; }
            set
            {
                // We always need a provider
                if (value != null)
                {
                    m_Provider = value;
                }
            }
        }

        /// <summary>
        /// Findet den passenden Serialisierer (durch Ausprobieren) und gibt das deserialisierte Objekt zurück.
        /// </summary>
        /// <returns>null bei Fehler, sonst ein object</returns>
        private static object FindDeserializerGetValue (string value) {
            SettingsSerializer[] serializers = new SettingsSerializer[] { new PrimitiveSerializer (), new CommonSerializer (), new BinarySerializer () };
            foreach (SettingsSerializer serializer in serializers) {
                object result = serializer.Deserialize (value);
                // Die Deserialisierer brechen frühzeitig ab, wenn sie mit dem Wert nicht klarkommen.
                // Insofern können wir hier alle durchprobieren.
                if (result != null) {
                    return result;
                }
            }
            return null;
        }

        #region Default

        public static bool SetDefault (string key, object value) {
            return SetDefault(System.String.Empty, key, value);
        }

        public static bool SetDefault (string company, string key, object value) {
            return SetDefault(System.String.Empty, key, value);
        }

        public static bool SetDefault (System.Windows.Forms.Control control, string key, object value) {
            return SetDefault(control.Name, key, value);
        }

        #endregion

        #region Setter SetValue

        /// <summary>
        /// Speichert einen beliebigen Wert in der Registry. Einzigste
        /// Bedingung ist, dass der Typ serialisierbar ist (IsSerializable).
        /// </summary>
        /// <param name="key">Name des Schlüssels</param>
        /// <param name="value">Wert, der gepspeichert wird</param>
        public static void SetValue (string key, object value) {
            SetValue(System.String.Empty, key, value);
        }

        /// <summary>
        /// Speichert einen beliebigen Wert in der Registry. Einzigste
        /// Bedingung ist, dass der Typ serialisierbar ist (IsSerializable).
        /// </summary>
        /// <param name="control">Control; Eigenschaft "Name" wird als Sektionsname verwendet</param>
        /// <param name="key">Name des Schlüssels</param>
        /// <param name="value">Wert, der gepspeichert wird</param>
        public static void SetValue (System.Windows.Forms.Control control, string key, object value) {
            SetValue(control.Name, key, value);
        }

        #endregion

        public static void SetValue (string ownRootNodeName, System.Windows.Forms.Control control, string key, object value) {
            SetValue(control.Name, key, value);
        }

        /// <summary>
        /// Speichert einen beliebigen Wert in der Registry. Einzigste
        /// Bedingung ist, dass der Typ serialisierbar ist (IsSerializable).
        /// </summary>
        /// <param name="ownRootNodeName">Gibt den Hauptknoten an. Kann null oder leer sein, dann wird WW-Ziel + YXZ genommen.</param>
        /// <param name="section">Unterschlüssel (noch unterhalb des Dialogs). Kann null sein, und wird dann ignoriert.</param>
        /// <param name="key">Schlüssel</param>
        /// <param name="value">Wert</param>
        public static void SetValue (string section, string key, object value) {
            if (key == null || key == string.Empty) {
                throw new ArgumentException ("Der Schlüssel darf nicht leer sein.");
            }

            if (value == null)
            {
                m_Provider.SetValue(null, section, key);
                return;
            }

            if (!value.GetType ().IsSerializable) {
                throw new ArgumentException ("Werte müssen serialisierbar sein (typeof(...).IsSerializable == true)");
            }

            if (value.GetType().IsEnum)
            {
                value = Convert.ToInt32(value);
            }

            // Wir unterscheiden verschiedene Arten von Daten
            // - "Einfache" (sog. primitive) Typen: string, bool, int, float (Verarbeitung mit .Parse() und ToString()
            //   - Enum-Werte werden ebenfalls so verarbeitet
            // - Allgemeine Typen: Point, Size, PointF, SizeF (Selbstgeschriebene Funktionen)
            // - Sonstige serialisierbare Typen: Werden mit dem BinarySerializer verarbeitet

            SettingsSerializer serializer = null;

            if (value.GetType ().IsPrimitive) {
                serializer = new PrimitiveSerializer ();
            }
            else if (CommonSerializer.SupportsType (value.GetType ())) {
                // Allgemeine Typen
                serializer = new CommonSerializer ();
            }
            else {
                // Binär als letzte Chance
                serializer = new BinarySerializer ();
            }

            if (serializer == null) {
                throw new NotImplementedException ("Kein passender Serialisierer verfügbar.");
            }
            m_Provider.SetValue(serializer.Serialize(value), section, key);
        }


        #region Getter GetValue

        public static object GetValue (string key) {
            return GetValue(System.String.Empty, key, null);
        }

        public static object GetValue (System.Windows.Forms.Control control, string key) {
            return GetValue(control, key);
        }

        public static object GetValue (string key, object defaultValue) {
            return GetValue(System.String.Empty, key, defaultValue);
        }

        public static object GetValue (System.Windows.Forms.Control control, string key, object defaultValue) {
            return GetValue(control.Name, key, defaultValue);
        }

        public static object GetValue(string section, string key, object defaultValue)
        {
            try
            {
                string value = m_Provider.GetValue(section, key) as string;
                object result = FindDeserializerGetValue(value);
                return (result == null) ? defaultValue : result;
            }
            catch (UnauthorizedAccessException ex)
            {
                Debug.LogError(ex);
                return null;
            }
        }

        #endregion


        public abstract class SettingsSerializer {
            /// <summary>
            /// Serialisiert einen beliebigen Typen.
            /// </summary>
            /// <returns>Typname:Wert.ToString()</returns>
            public virtual string Serialize (object value) {
                return value.GetType ().FullName + ":" + value.ToString ();
            }

            /// <summary>
            /// Deserialisiert einen String zu einem Objekt. Gibt null bei Fehlschlag zurück.
            /// Diese Methode muss für jeden Typen extra implementiert werden.
            /// </summary>
            public virtual object Deserialize (string value) {
                throw new NotImplementedException ();
            }

            protected string[] SplitTypeValue (string value) {
                if (value == null) { return null; }
                int colon = value.IndexOf (':');
                if (colon == -1 || value.Length <= colon) {
                    return null;
                }
                return new string[] { value.Substring (0, colon), value.Substring (colon + 1) };
            }
        }

        /// <summary>
        /// Übernimmt die Serialisierung aller primitiven Typen (int, bool, etc.).
        /// </summary>
        public class PrimitiveSerializer : SettingsSerializer {
            public override object Deserialize (string value) {
                string[] data = SplitTypeValue (value);
                if (data == null) {
                    return null;
                }

                // Wir holen uns den Typen, und rufen von diesem dann die statische Methode "Parse" auf
                System.Type type = null;
                try {
                    type = Type.GetType (data[0]);
                }
                catch (FileLoadException) {
                    // Typ unbekannt o.ä.
                    return null;
                }
                if (type == null || !type.IsPrimitive) {
                    return null;
                }

                object result = null;
                try {
                    result = type.InvokeMember ("Parse", System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Static | System.Reflection.BindingFlags.InvokeMethod, null, type, new object[] { data[1] });
                }
                catch (Exception) {
                    // Umwandlung ungültig, also null zurückgeben
                    return null;
                }
                return result;
            }
        }

        /// <summary>
        /// Deserialisiert verschiedene häufig genutzte Typen. Aktuell: Size, Point, string.
        /// </summary>
        public class CommonSerializer : SettingsSerializer {
            /// <summary>
            /// Gibt an, ob von einem bestimmten Typen die Serialisierung möglich ist.
            /// </summary>
            public static bool SupportsType (Type type) {
                Type[] types = new Type[] { typeof (System.Drawing.Point), typeof (System.Drawing.Size), typeof (string), typeof (System.Drawing.Color) };
                return (Array.IndexOf (types, type) >= 0);
            }

            public override string Serialize (object value) {
                string result = value.GetType ().FullName + ":";
                // Für jeden Typen eine andere Serialisierung
                if (value.GetType () == typeof (string)) {
                    return result + value.ToString ();
                }
                if (value.GetType () == typeof (System.Drawing.Color)) {
                    return result + ((System.Drawing.Color)value).ToArgb ().ToString ();
                }
                if (value.GetType () == typeof (System.Drawing.Point)) {
                    System.Drawing.Point point = (System.Drawing.Point)value;
                    return result + string.Format ("{0},{1}", point.X, point.Y);

                }
                if (value.GetType () == typeof (System.Drawing.Size)) {
                    System.Drawing.Size size = (System.Drawing.Size)value;
                    return result + string.Format ("{0},{1}", size.Width, size.Height);
                }
                return string.Empty;
            }

            public override object Deserialize (string value) {
                string[] data = SplitTypeValue (value);
                if (data == null) {
                    return null;
                }

                switch (data[0]) {
                    case "System.Drawing.Color":
                        return System.Drawing.Color.FromArgb (System.Convert.ToInt32 (data[1]));
                    case "System.String":
                        return data[1];
                    case "System.Drawing.Point":
                        System.Drawing.Point point = new System.Drawing.Point ();
                        string[] coords = data[1].Split (',');
                        point.X = System.Convert.ToInt32 (coords[0]);
                        point.Y = System.Convert.ToInt32 (coords[1]);
                        return point;
                    case "System.Drawing.Size":
                        System.Drawing.Size size = new System.Drawing.Size ();
                        string[] dimension = data[1].Split (',');
                        size.Width = System.Convert.ToInt32 (dimension[0]);
                        size.Height = System.Convert.ToInt32 (dimension[1]);
                        return size;
                }

                return null;
            }
        }

        /// <summary>
        /// Serialisiert so ziemlich alles, aber binär. Diese Methode ist aber nur als
        /// letzte Chance vorgesehen, da der base64 string recht lang ist und das
        /// sieht in der Registry nicht so schön aus (vor allem lässt er sich schlecht bearbeiten).
        /// </summary>
        public class BinarySerializer : SettingsSerializer {
            /// <returns>base64-kodierter string</returns>
            public override string Serialize (object value) {
                MemoryStream store = new MemoryStream ();
                BinaryFormatter serializer = new BinaryFormatter ();
                serializer.Serialize (store, value);
                string result = System.Convert.ToBase64String (store.ToArray ());
                store.Close ();
                return result;
            }

            public override object Deserialize (string value) {
                if (value == null) { return null; }
                BinaryFormatter serializer = new BinaryFormatter ();
                try
                {
                    using (MemoryStream stream = new MemoryStream(System.Convert.FromBase64String(value)))
                    {
                        return serializer.Deserialize(stream); ;
                    }
                }
                catch (FormatException)
                {
                    // Assume a simple string as last resort
                    return value;
                }
                catch (Exception)
                {
                    return null;
                }
            }
        }

    }
}
