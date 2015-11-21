using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SQLite;
using System.IO;
using System.Xml;
using System.Xml.Serialization;

namespace Ketarin
{
    /// <summary>
    /// Exports and imports Ketarin settings to files.
    /// </summary>
    internal static class SettingsExporter
    {
        public static void ExportToFile(string filename)
        {
            XmlWriterSettings settings = new XmlWriterSettings {Indent = true};

            XmlWriter writer = XmlWriter.Create(filename, settings);

            writer.WriteStartElement("Ketarin");

            writer.WriteStartElement("Settings");
            // Export normal settings 
            XmlSerializer serializer = new XmlSerializer(typeof(SerializableDictionary<string, string>));
            serializer.Serialize(writer, DbManager.GetSettings());
            writer.WriteEndElement();

            // Export global variables (stored in "variables" table)
            writer.WriteStartElement("GlobalVariables");
            foreach (UrlVariable var in UrlVariable.GlobalVariables.Values)
            {
                writer.WriteStartElement("Variable");
                writer.WriteAttributeString("Name", var.Name);
                writer.WriteAttributeString("Content", var.CachedContent);
                writer.WriteEndElement();
            }
            writer.WriteEndElement();

            // Export code snippets (stored in "snippets" table)
            writer.WriteStartElement("CodeSnippets");
            foreach (Snippet snippet in DbManager.GetSnippets())
            {
                writer.WriteStartElement("Snippet");
                writer.WriteAttributeString("Guid", DbManager.FormatGuid(snippet.Guid));
                writer.WriteAttributeString("Name", snippet.Name);
                writer.WriteAttributeString("Type", ((int)snippet.Type).ToString());
                writer.WriteString(snippet.Text);
                writer.WriteEndElement();
            }
            writer.WriteEndElement();

            writer.WriteStartElement("SetupLists");
            // Save setup lists
            foreach (ApplicationList list in DbManager.GetSetupLists(DbManager.GetJobs()))
            {
                if (!list.IsPredefined)
                {
                    writer.WriteStartElement("List");
                    writer.WriteAttributeString("Name", list.Name);
                    writer.WriteAttributeString("Guid", DbManager.FormatGuid(list.Guid));

                    writer.WriteStartElement("Applications");
                    foreach (ApplicationJob app in list.Applications)
                    {
                        writer.WriteStartElement("Application");
                        writer.WriteAttributeString("Guid", DbManager.FormatGuid(app.Guid));
                        writer.WriteAttributeString("Name", app.Name);
                        writer.WriteEndElement();
                    }
                    writer.WriteEndElement();

                    writer.WriteEndElement();
                }
            }
            writer.WriteEndElement();

            writer.Close();
        }

        public static void ImportFromFile(string filename)
        {
            XmlDocument doc = new XmlDocument();
            doc.Load(filename);

            // Import settings from file as dictionary
            XmlElement settingsElem = doc.SelectSingleNode("//Settings/dictionary") as XmlElement ??
                                      doc.SelectSingleNode("//dictionary") as XmlElement;

            if (settingsElem != null)
            {
                XmlSerializer serializer = new XmlSerializer(typeof(SerializableDictionary<string, string>));

                using (StringReader textReader = new StringReader(settingsElem.OuterXml))
                {
                    DbManager.SetSettings(serializer.Deserialize(textReader) as Dictionary<string, string>);
                }
            }
            
            // Import global variables
            XmlElement varNodes = doc.SelectSingleNode("//GlobalVariables") as XmlElement;
            if (varNodes != null)
            {
                UrlVariable.GlobalVariables.Clear();

                foreach (XmlElement varElem in doc.SelectNodes("//GlobalVariables/Variable"))
                {
                    UrlVariable newVar = new UrlVariable
                    {
                        Name = varElem.GetAttribute("Name"),
                        CachedContent = varElem.GetAttribute("Content")
                    };
                    UrlVariable.GlobalVariables[newVar.Name] = newVar;
                }

                UrlVariable.GlobalVariables.Save();
            }

            // Import code snippets
            XmlElement snippetNodes = doc.SelectSingleNode("//CodeSnippets") as XmlElement;
            if (snippetNodes != null)
            {
                using (SQLiteCommand comm = DbManager.Connection.CreateCommand())
                {
                    comm.CommandText = "DELETE FROM snippets";
                    comm.ExecuteNonQuery();
                }

                foreach (XmlElement snippetElem in doc.SelectNodes("//CodeSnippets/Snippet"))
                {
                    Snippet snippet = new Snippet
                    {
                        Guid = new Guid(snippetElem.GetAttribute("Guid")),
                        Name = snippetElem.GetAttribute("Name"),
                        Type = (ScriptType) Convert.ToInt32(snippetElem.GetAttribute("Type")),
                        Text = snippetElem.InnerText
                    };
                    snippet.Save();
                }
            }

            XmlElement setupNodes = doc.SelectSingleNode("//SetupLists") as XmlElement;
            if (setupNodes != null)
            {
                using (IDbCommand command = DbManager.Connection.CreateCommand())
                {
                    command.CommandText = @"DELETE FROM setuplists_applications";
                    command.ExecuteNonQuery();
                }

                using (IDbCommand command = DbManager.Connection.CreateCommand())
                {
                    command.CommandText = @"DELETE FROM setuplists";
                    command.ExecuteNonQuery();
                }

                foreach (XmlElement listElem in doc.SelectNodes("//SetupLists/List"))
                {
                    ApplicationList list = new ApplicationList
                    {
                        Name = listElem.GetAttribute("Name"),
                        Guid = new Guid(listElem.GetAttribute("Guid"))
                    };

                    foreach (XmlElement appListElem in listElem.SelectNodes("Applications/Application"))
                    {
                        Guid guid = new Guid(appListElem.GetAttribute("Guid"));

                        ApplicationJob job = DbManager.GetJob(guid);
                        if (job != null)
                        {
                            list.Applications.Add(job);
                        }
                    }

                    list.Save();
                }
            }            
        }
    }
}
