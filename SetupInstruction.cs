using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Xml.Serialization;
using System.Xml;
using System.Data.SQLite;

namespace Ketarin
{
    public class SetupInstruction : ICloneable
    {
        /// <summary>
        /// Application this instruction belongs to.
        /// </summary>
        [XmlIgnore()]
        public ApplicationJob Application { get; set; }

        /// <summary>
        /// Name of the instruction (type).
        /// </summary>
        public virtual string Name
        {
            get
            {
                return string.Empty;
            }
        }

        /// <summary>
        /// Executes the action.
        /// </summary>
        public virtual void Execute()
        {
        }

        /// <summary>
        /// Saves the setup instructions to the database.
        /// </summary>
        public void Save(System.Data.IDbTransaction transaction, int position)
        {
            XmlSerializer serializer = new XmlSerializer(this.GetType());
            XmlWriterSettings settings = new XmlWriterSettings();
            settings.Indent = true;

            StringBuilder output = new StringBuilder();

            using (XmlWriter xmlWriter = XmlWriter.Create(output, settings))
            {
                serializer.Serialize(xmlWriter, this);
            }

            using (IDbCommand command = transaction.Connection.CreateCommand())
            {
                command.Transaction = transaction;
                command.CommandText = "INSERT INTO setupinstructions (JobGuid, Position, Data) VALUES(@JobGuid, @Position, @Data)";
                command.Parameters.Add(new SQLiteParameter("@JobGuid", DbManager.FormatGuid(this.Application.Guid)));
                command.Parameters.Add(new SQLiteParameter("@Position", position));
                command.Parameters.Add(new SQLiteParameter("@Data", output.ToString()));
                command.ExecuteNonQuery();
            }
        }

        #region ICloneable Member

        public object Clone()
        {
            return this.MemberwiseClone();
        }

        #endregion
    }
}
