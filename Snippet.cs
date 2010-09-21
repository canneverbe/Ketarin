using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Data.SQLite;

namespace Ketarin
{
    /// <summary>
    /// Represents a piece of code that can be re-used for any commands.
    /// </summary>
    public class Snippet
    {
        /// <summary>
        /// Gets or sets the GUID of the snippet.
        /// </summary>
        public Guid Guid { get; set; }

        /// <summary>
        /// Gets or sets the name of the snippet.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the language/type of the snippet.
        /// </summary>
        public ScriptType Type { get; set; }

        /// <summary>
        /// Gets or sets the content of the snippet.
        /// </summary>
        public string Text { get; set; }

        /// <summary>
        /// Saves the snippet to the database.
        /// </summary>
        public void Save()
        {
            if (this.Guid == null || this.Guid == Guid.Empty)
            {
                this.Guid = Guid.NewGuid();
            }

            using (IDbCommand command = DbManager.Connection.CreateCommand())
            {
                command.CommandText = @"INSERT OR REPLACE INTO snippets (SnippetGuid, Name, Type, Text) VALUES (@SnippetGuid, @Name, @Type, @Text)";
                command.Parameters.Add(new SQLiteParameter("@SnippetGuid", DbManager.FormatGuid(this.Guid)));
                command.Parameters.Add(new SQLiteParameter("@Name", Name));
                command.Parameters.Add(new SQLiteParameter("@Type", Type.ToString()));
                command.Parameters.Add(new SQLiteParameter("@Text", Text));
                command.ExecuteNonQuery();
            }
        }

        internal void Hydrate(IDataReader reader)
        {
            this.Name = reader["Name"] as string;
            this.Type = Command.ConvertToScriptType(reader["Type"] as string);
            this.Text = reader["Text"] as string;
            this.Guid = new Guid(reader["SnippetGuid"] as string);
        }

        /// <summary>
        /// Removes a snippet from the database.
        /// </summary>
        internal void Delete()
        {
            using (IDbCommand command = DbManager.Connection.CreateCommand())
            {
                command.CommandText = @"DELETE FROM snippets WHERE SnippetGuid = @SnippetGuid";
                command.Parameters.Add(new SQLiteParameter("@SnippetGuid", DbManager.FormatGuid(this.Guid)));
                command.ExecuteNonQuery();
            }
        }
    }
}
