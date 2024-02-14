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
    /// TaskDalController class allows interacting with the Tasks table of the database.
    /// </summary>
    internal class TaskDalController : DalController
    {
        private static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>
        /// Initializes a new instance of the TaskDalController class.
        /// </summary>
        public TaskDalController() : base("Tasks", new string[] {"TaskID"})
        {
        }

        /// <summary>
        /// This method converts the database reader values into a corresponding TaskDTO object.
        /// </summary>
        /// <param name="reader">The reader in the DB.</param>
        /// <returns>The converted DTO object.</returns>
        protected override DTO ConvertReaderToObject(SQLiteDataReader reader)
        {
            //int TaskID, string title, DateTime creationTime, DateTime dueDate, string description, 
            //  string assigneeUser, int boardId, int columnNumber
            DTO result = new TaskDTO(reader.GetInt32(0), reader.GetString(1), DateTime.Parse(reader.GetString(2)), DateTime.Parse(reader.GetString(3)),
                reader.GetString(4), reader.GetString(5), reader.GetInt32(6), reader.GetInt32(7));
            log.Debug($"Converted reader to task DTO.");
            return result;
        }
    }
}
