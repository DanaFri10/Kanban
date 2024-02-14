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
    /// UserDalController class allows interacting with the Users table of the database.
    /// </summary>
    public class UserDalController : DalController
    {
        private static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>
        /// Initializes a new instance of the UserDalController class.
        /// </summary>
        public UserDalController() : base("Users", new string[] { "EmailAddress" })
        {
        }

        /// <summary>
        /// This method converts the database reader values into a corresponding UserDTO object.
        /// </summary>
        /// <param name="reader">The reader in the DB.</param>
        /// <returns>The converted DTO object.</returns>
        protected override DTO ConvertReaderToObject(SQLiteDataReader reader)
        {
            DTO result = new UserDTO(reader.GetString(reader.GetOrdinal("EmailAddress")), reader.GetString(reader.GetOrdinal("Password")));
            log.Debug($"Converted reader to UserDTO.");
            return result;
        }
    }
}
