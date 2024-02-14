using log4net;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace IntroSE.Kanban.Backend.DataAccessLayer
{
    /// <summary>
    /// ColumnDTO class represents a record of the Columns table in the database.
    /// </summary>
    public class ColumnDTO : DTO
    {
        
        public int ColumnNumber { get; }
        public int BoardID { get; }

        private int _tasksLimit;
        public int TasksLimit { 
            get => _tasksLimit; 
            set
            {
                _dalController.Update(new string[] { BoardID.ToString(), ColumnNumber.ToString() }, "TasksLimit", value.ToString());
                _tasksLimit = value;
            }
        }
        public List<TaskDTO> Tasks { get; set; }

        private static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>
        /// Constructor for load data from DB, that gets the board id, column number, tasks limit.
        /// </summary>
        /// <param name="boardId">The id of the board.</param>
        /// <param name="columnNumber">The column number.</param>
        /// <param name="tasksLimit">The limit of the column.</param>
        public ColumnDTO(int boardId, int columnNumber, int tasksLimit) : base(new ColumnDalController())
        {
            BoardID = boardId;
            ColumnNumber = columnNumber;
            _tasksLimit = tasksLimit;
            TaskDalController taskDalController = new TaskDalController();
            List<DTO> DTOs = taskDalController.Select(new string[] { "BoardID" , "ColumnNumber"}, new string[] { BoardID.ToString() ,columnNumber.ToString()});
            Tasks = new List<TaskDTO>();
            foreach (DTO dto in DTOs)
            {
                Tasks.Add((TaskDTO)dto);
            }
            
            log.Debug($"Created new Column with the following parameters: boardId={boardId}, columnNumber={columnNumber}, tasksLimit={tasksLimit}.");
        }

        /// <summary>
        /// Constructor for load data from DB, that gets the board id, column number.
        /// </summary>
        /// <param name="boardId">the boardID of the board's column</param>
        /// <param name="columnNumber">the number of the column in board</param>
        public ColumnDTO(int boardId, int columnNumber) : base(new ColumnDalController())
        {
            BoardID = boardId;
            ColumnNumber = columnNumber;
            _tasksLimit = -1;//defult when there is not limit
            Tasks = new List<TaskDTO>();
            log.Debug($"Created new Column with the following parameters: boardId={boardId}, columnNumber={columnNumber}.");
        }

        /// <summary>
        /// This method deleted the columnDTO from the DB.
        /// </summary>
        public override void Delete()
        {
            _dalController.Delete(new string[] { BoardID.ToString(), ColumnNumber.ToString() });
            foreach (TaskDTO taskDTO in Tasks)
            {
                taskDTO.Delete();
            }
            log.Debug($"Deleted the columnDTO with id {BoardID} from DB.");
        }

        /// <summary>
        /// This method inserts the columnDTO to to DB.
        /// </summary>
        public override void Insert()
        {
            _dalController.Insert(new string[] { "BoardID", "ColumnNumber" , "TasksLimit" }, 
                new string[] { BoardID.ToString(), ColumnNumber.ToString(), TasksLimit.ToString() });
            log.Debug($"Inserted the columnDTO with id {BoardID} to the DB.");
        }
    }
}
