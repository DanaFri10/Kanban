using IntroSE.Kanban.Backend.DataAccessLayer;
using log4net;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace IntroSE.Kanban.Backend.BusinessLayer
{
    /// <summary>
    /// Class Task represents a task in the Kanban system.
    /// </summary>
    public class Task
    {
        public DateTime CreationTime { get; private set; }

        private DateTime _dueDate;
        public DateTime DueDate { get => _dueDate ;private set { TaskDTO.DueDate = value; _dueDate = value; } }
        
        private string _title;
        public string Title { get => _title; private set { TaskDTO.Title = value; _title = value; } }

        private string _description;
        public string Description { get => _description; private set { TaskDTO.Description = value; _description = value; } }
        public int TaskID { get; private set; }

        private string _assigneeUser;
        public string AssigneeUser { get => _assigneeUser; private set { TaskDTO.AssigneeUser = value; _assigneeUser = value; } }

        private int _columnNumber;
        public int ColumnNumber { get => _columnNumber; set { TaskDTO.ColumnNumber = value; _columnNumber = value; } }
        public TaskDTO TaskDTO { get; private set; }

        private static readonly int MAX_TITLE_LENGTH = 50;

        private static readonly int MAX_DESCRIPTION_LENGTH = 300;

        public static readonly string UNASSIGNED_TASK = "";

        private static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>
        /// Initializes a new instance of the Task class.
        /// </summary>
        /// <param name="dueDate">The task's due date.</param>
        /// <param name="title">The task's title.</param>
        /// <param name="description">The task's description.</param>
        /// <param name="boardId">The id of the board the task is in.</param>
        /// <exception cref="ArgumentNullException">If one of the fields is null.</exception>
        /// <exception cref="ArgumentException">If one of the fields is invalid.</exception>
        public Task(DateTime dueDate, string title, string description, int boardId)
        {

            if (boardId < 0)
            {
                log.Error("Error: The board id is invalid.");
                throw new ArgumentException("Error: The board id is invalid.");
            }
            ValidTaskParameters(dueDate, title, description);
            
            CreationTime = DateTime.Now;
            _assigneeUser = UNASSIGNED_TASK;
            _columnNumber = Board.BACKLOG_COLUMN;
            _dueDate = dueDate;
            _title = title;
            _description = description;

            TaskDTO = new TaskDTO(title, CreationTime, dueDate, description, _assigneeUser, boardId, _columnNumber);
            TaskDTO.Insert();
            TaskID = TaskDTO.TaskID;
            log.Debug($"A new task was created with the following parameters: dueDate={dueDate}, title={title}, description={description}, boardId={boardId}");
        }

        /// <summary>
        /// Initializes a new instance of the Task class, with the given TaskDTO object.
        /// </summary>
        /// <param name="taskDTO">The given TaskDTO object.</param>
        /// <exception cref="ArgumentNullException">If the taskDTO is null.</exception>
        public Task(TaskDTO taskDTO)
        {
            if (taskDTO == null)
            {
                log.Error("Error: The taskDTO is null.");
                throw new ArgumentNullException("Error: The taskDTO is null.");
            }

            TaskID = taskDTO.TaskID;
            CreationTime = taskDTO.CreationTime;
            _dueDate = taskDTO.DueDate;
            _title = taskDTO.Title;
            _description = taskDTO.Description;
            _assigneeUser = taskDTO.AssigneeUser;
            _columnNumber= taskDTO.ColumnNumber;
            TaskDTO = taskDTO;
        }

        /// <summary>
        /// This method checks if the parameters dueDate, title, description of task are valid.
        /// </summary>
        /// <param name="dueDate">The due date of the task.</param>
        /// <param name="title">The title of the task.</param>
        /// <param name="description">The description of the task.</param>
        /// <exception cref="ArgumentNullException">If any of the parameters are null.</exception>
        /// <exception cref="ArgumentException">If any of the parameters are invalid.</exception>
        private void ValidTaskParameters(DateTime dueDate, string title, string description)
        {
            if (dueDate.Equals(null))
            {
                log.Error("Error: " + nameof(dueDate) + " is null");
                throw new ArgumentNullException("Error: " + nameof(dueDate) + " is null");
            }
            if (title == null)
            {
                log.Error("Error: " + nameof(title) + " is null");
                throw new ArgumentNullException("Error: " + nameof(title) + " is null");
            }
            if (description == null)
            {
                log.Error("Error: " + nameof(description) + " is null");
                throw new ArgumentNullException("Error: " + nameof(description) + " is null");
            }
            if (description.Length > MAX_DESCRIPTION_LENGTH)
            {
                log.Error("Error: Description can't be longer than " + MAX_DESCRIPTION_LENGTH + " characters.");
                throw new ArgumentException("Error: Description can't be longer than " + MAX_DESCRIPTION_LENGTH + " characters.");
            }
            if (title.Length == 0 || String.IsNullOrWhiteSpace(title))
            {
                log.Error("Error: Title can't be empty.");
                throw new ArgumentException("Error: Title can't be empty.");
            }
            if (title.Length > MAX_TITLE_LENGTH)
            {
                log.Error("Error: Title can't be longer than " + MAX_TITLE_LENGTH + " characters.");
                throw new ArgumentException("Error: Title can't be longer than " + MAX_TITLE_LENGTH + " characters.");
            }
            if (dueDate < DateTime.Now)
            {
                log.Error("Error: A due date can not be a past date.");
                throw new ArgumentException("Error: A due date can not be a past date.");
            }
        }

        /// <summary>
        /// This method edits a task according to the given parameters, unless an error has occured.
        /// </summary>
        /// <param name="userEmail">The user's email address.</param>
        /// <param name="dueDate">The due date of the task.</param>
        /// <param name="title">The title of the task.</param>
        /// <param name="description">The description of the task.</param>
        /// <exception cref="ArgumentNullException">If one of the fields is null.</exception>
        /// <exception cref="ArgumentException">If one of the fields is invalid.</exception>
        public void EditTask(string userEmail,DateTime dueDate, string title, string description)
        {
            ValidTaskParameters(dueDate, title, description);
            userEmail = userEmail.ToLower();
            if (!IsAssignee(userEmail))
            {
                log.Error("Error: Couldn't edit task, since the user is not the task's assignee.");
                throw new ArgumentException("Error: Couldn't edit task, since the user is not the task's assignee.");
            }

            this.DueDate = dueDate;
            this.Title = title;
            this.Description = description;

            log.Debug($"The task {TaskID} was editted. The new parameters are: dueDate={dueDate}, title={title}, description={description}");
        }

        /// <summary>
        /// This methods changes the task's assignee, unless an error has occured.
        /// </summary>
        /// <param name="userEmail">The email of the user changing the task's assignee.</param>
        /// <param name="newAssignee">The email of the new assignee.</param>
        /// <exception cref="ArgumentNullException">If one of the users is null.</exception>
        /// <exception cref="ArgumentException">If the user changing the task's assignee is not the current assignee.</exception>
        public void ChangeAssignee(string userEmail, string newAssignee)
        {
            if (userEmail == null || newAssignee == null)
            {
                log.Error("Error: The user is null.");
                throw new ArgumentNullException("Error: The user is null.");
            }

            userEmail = userEmail.ToLower();
            newAssignee = newAssignee.ToLower();

            if (AssigneeUser != UNASSIGNED_TASK && !IsAssignee(userEmail))
            {
                log.Error("Error: Only the task assignee can change the assigned user.");
                throw new ArgumentException("Error: Only the task assignee can change the assigned user.");
            }

            this.AssigneeUser = newAssignee;

            log.Debug($"The assignee of {TaskID} changed. The old assignee was {userEmail}. The new assignee is {newAssignee}");
        }

        /// <summary>
        /// This method unassignes the task.
        /// </summary>
        /// <exception cref="ArgumentException">If the task is already unassigned.</exception>
        public void Unassign()
        {
            if (this.AssigneeUser == UNASSIGNED_TASK)
            {
                log.Error("Error: This task is already unassgined.");
                throw new ArgumentException("Error: This task is already unassgined.");
            }
            this.AssigneeUser = UNASSIGNED_TASK;

            log.Debug($"The task {TaskID} is unassigned.");
        }

        /// <summary>
        /// This methods checks if the given user is the task's assignee.
        /// </summary>
        /// <param name="userEmail">The user's email address.</param>
        /// <returns>True if the given user is the task's assignee, false otherwise.</returns>
        public bool IsAssignee(string userEmail)
        {
            if(userEmail!=null)
                userEmail = userEmail.ToLower();
            return AssigneeUser.Equals(userEmail);
        }
    }
}
