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
    /// TaskDTO class represents a record of the Tasks table in the database.
    /// </summary>
    public class TaskDTO : DTO
    {
        public int TaskID { get; private set; }
        
        public string Title
        {
            get => _title; 
            set
            {
                _dalController.Update(new string[] { TaskID.ToString() }, "Title", value.ToString());
                _title = value;
            }
        }
        private string _title;

        public DateTime CreationTime { get; }
        
        public DateTime DueDate
        {
            get => _dueDate; 
            set
            {
                _dalController.Update(new string[] { TaskID.ToString() }, "DueDate", value.ToString());
                _dueDate = value;
            }
        }
        private DateTime _dueDate;

        public string Description
        {
            get => _description; 
            set
            {
                _dalController.Update(new string[] { TaskID.ToString() }, "Description", value);
                _description = value;
            }
        }
        private string _description;
        public string AssigneeUser
        {
            get => _assigneeUser; 
            set
            {
                _dalController.Update(new string[] { TaskID.ToString() }, "AssigneeUser", value);
                _assigneeUser = value;
            }
        }
        private string _assigneeUser;
        public int BoardID { get; }
        public int ColumnNumber
        {
            get => _columnNumber; set
            {
                _dalController.Update(new string[] { TaskID.ToString() }, "ColumnNumber", value.ToString());
                _columnNumber = value;
            }
        }
        private int _columnNumber;
        private static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);


        /// <summary>
        /// Constructor for loading data from DB.
        /// </summary>
        /// <param name="taskID">The task's ID.</param>
        /// <param name="creationTime">The task's creation time.</param>
        /// <param name="dueDate">The task's due date.</param>
        /// <param name="title">The task's title.</param>
        /// <param name="description">The task's description.</param>
        /// <param name="assigneeUser">The task's assignee user.</param>
        /// <param name="boardId">The id of the board the task is in.</param>
        /// <param name="columnNumber">The number of the column the task is in.</param>
        /// <exception cref="ArgumentNullException">If one of the fields is null.</exception>
        /// <exception cref="ArgumentException">If one of the fields is invalid.</exception>
        public TaskDTO(int taskID, string title, DateTime creationTime, DateTime dueDate, string description, 
            string assigneeUser, int boardId, int columnNumber) : base(new TaskDalController())
        {
            CreationTime = creationTime;
            _dueDate = dueDate;
            _title = title;
            _description = description;
            TaskID = taskID;
            _assigneeUser = assigneeUser;
            BoardID = boardId;
            _columnNumber = columnNumber;
        }

        /// <summary>
        /// Constructor for initializing a new instance of the TaskDTO class, according to the given parameters.
        /// </summary>
        /// <param name="creationTime">The task's creation time.</param>
        /// <param name="dueDate">The task's due date.</param>
        /// <param name="title">The task's title.</param>
        /// <param name="description">The task's description.</param>
        /// <param name="assigneeUser">The task's assignee user.</param>
        /// <param name="boardId">The id of the board the task is in.</param>
        /// <param name="columnNumber">The number of the column the task is in.</param>
        /// <exception cref="ArgumentNullException">If one of the fields is null.</exception>
        /// <exception cref="ArgumentException">If one of the fields is invalid.</exception>
        public TaskDTO(string title, DateTime creationTime, DateTime dueDate, string description,
    string assigneeUser, int boardId, int columnNumber) : base(new TaskDalController())
        {
            CreationTime = creationTime;
            _dueDate = dueDate;
            _title = title;
            _description = description;
            _assigneeUser = assigneeUser;
            BoardID = boardId;
            _columnNumber = columnNumber;
        }

        /// <summary>
        /// This method deletes the taskDTO from the DB.
        /// </summary>
        public override void Delete()
        {
            _dalController.Delete(new string[] { TaskID.ToString() });
            log.Debug($"Deleted the taskDTO with id {TaskID} from DB.");
        }

        /// <summary>
        /// This method inserts the taskDTO to the DB.
        /// </summary>
        public override void Insert()
        {
            _dalController.Insert(new string[] { "Title" , "CreationTime", "DueDate" , "Description" , "AssigneeUser", "BoardID" , "ColumnNumber" }, 
                                    new string[] { Title, CreationTime.ToString(), DueDate.ToString(), Description , AssigneeUser , BoardID.ToString(), ColumnNumber.ToString()});
            TaskID = _dalController.GetMaxValue("TaskID");
            log.Debug($"Inserted the taskDTO with id {TaskID} to the DB.");
        }
    }
}
