using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SQLite;
using log4net;
using System.Reflection;

namespace IntroSE.Kanban.Backend.DataAccessLayer
{
    /// <summary>
    /// BoardDalController class allows interacting with the Boards table of the database.
    /// </summary>
    internal class BoardDalController : DalController
    {
        private static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>
        /// Initializes a new instance of the BoardDalController class.
        /// </summary>
        public BoardDalController() : base("Boards", new string[] {"BoardID"})
        {
        }

        /// <summary>
        /// This method converts the database reader values into a corresponding BoardDTO object.
        /// </summary>
        /// <param name="reader">The reader in the DB.</param>
        /// <returns>The converted DTO object.</returns>
        protected override DTO ConvertReaderToObject(SQLiteDataReader reader)
        {
            DTO result = new BoardDTO(reader.GetInt32(reader.GetOrdinal("BoardID")), reader.GetString(reader.GetOrdinal("BoardName")), reader.GetString(reader.GetOrdinal("BoardOwner")));
            log.Debug($"Converted reader to board DTO.");
            return result;
        }

        
    }
}
