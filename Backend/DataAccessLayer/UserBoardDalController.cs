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
    /// UserBoardDalController class allows interacting with the UsersBoards table of the database.
    /// </summary>
    internal class UserBoardDalController : DalController
    {
        private static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>
        /// Initializes a new instance of the UserBoardDalController class.
        /// </summary>
        public UserBoardDalController() : base("UsersBoards", new string[] {"BoardID", "UserEmail"})
        {
        }

        /// <summary>
        /// This method converts the database reader values into a corresponding UserBoardDTO object.
        /// </summary>
        /// <param name="reader">The reader in the DB.</param>
        /// <returns>The converted DTO object.</returns>
        protected override DTO ConvertReaderToObject(SQLiteDataReader reader)
        {
            DTO result = new UserBoardDTO(reader.GetInt32(reader.GetOrdinal("BoardID")), reader.GetString(reader.GetOrdinal("UserEmail")));
            log.Debug($"Converted reader to UserBoardDTO.");
            return result;
        }
    }
}
