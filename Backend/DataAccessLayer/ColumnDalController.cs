using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SQLite;
using log4net;
using System.Reflection;

namespace IntroSE.Kanban.Backend.DataAccessLayer
{
    /// <summary>
    /// ColumnDalController class allows interacting with the Columns table of the database.
    /// </summary>
    public class ColumnDalController: DalController
    {
        private static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>
        /// Initializes a new instance of the ColumnDalController class.
        /// </summary>
        public ColumnDalController() : base("Columns", new string[] { "BoardID", "ColumnNumber" })
        {
        }

        /// <summary>
        /// This method converts the database reader values into a corresponding ColumnDTO object.
        /// </summary>
        /// <param name="reader">The reader in the DB.</param>
        /// <returns>The converted DTO object.</returns>
        protected override DTO ConvertReaderToObject(SQLiteDataReader reader)
        {
            DTO result = new ColumnDTO(reader.GetInt32(reader.GetOrdinal("BoardID")), reader.GetInt32(reader.GetOrdinal("ColumnNumber")), 
                reader.GetInt32(reader.GetOrdinal("TasksLimit")));
            log.Debug($"Converted reader to column DTO.");
            return result;
        }
    }
}
