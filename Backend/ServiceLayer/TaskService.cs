using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;
using log4net;

using IntroSE.Kanban.Backend.BusinessLayer;


namespace IntroSE.Kanban.Backend.ServiceLayer
{
    /// <summary>
    /// Class TaskService allows the Presentation Layer to interact with
    /// the Task system of the application.
    /// It supports operations such as creating, editing, getting, assigning and moving Tasks 
    /// between different board Columns, and more.
    /// 
    /// The returned value of the Service methods is a Json representation
    /// of a Response in the following form:
    /// {
    ///     "ErrorMessage": string, (if an error occured, otherwise null)
    ///     "ErrorOccured": bool,
    ///     "ReturnValue": string (Json representation of the returned object, if exists, otherwise null)
    /// }
    /// as explained in the class diagram.
    /// </summary>
    public class TaskService
    {
        private BoardController _boardController;
        private UserController _userController;

        private static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        internal TaskService(BoardController boardController, UserController userController)
        {
            this._boardController = boardController;
            this._userController = userController;
            log.Info("Initialized TaskService.");
        }

        /// <summary>
        /// This method returns a list with all the In Progress Tasks assigned to the specified user.
        /// </summary>
        /// <param name="userEmail">The user's email address.</param>
        /// <returns>
        /// A Json representation of a response containing an error message, if occured,
        /// and a Json representation of a list with all the In Progress Tasks assigned to the user, if exists.
        /// The Task Json is in the following form:
        /// {
        ///     "CreationTime": string of DateTime,
        ///     "DueDate": string of DateTime,
        ///     "Title": string,
        ///     "Description": string,
        ///     "TaskID": int,
        ///     "AssigneeUser": string
        /// }
        /// as explained in the class diagram.
        /// </returns>
        public string ListInProgressTasks(string userEmail)
        {
            try
            {
                if (!_userController.IsLoggedIn(userEmail))
                {
                    string errorMessage = "Error: User must be logged in to list in progress tasks.";
                    log.Error(errorMessage);
                    return new Response<Board>(errorMessage).ToJson();
                }

                List<BusinessLayer.Task> tasks = _boardController.ListInProgressTasks(userEmail);
                List<Task> serviceTasks = new List<Task>();
                tasks.ForEach(t => serviceTasks.Add(new Task(t)));
                return new Response<List<Task>>(serviceTasks).ToJson();
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                return new Response<Task>(ex.Message).ToJson();
            }
        }

        /// <summary>
        /// This method creates a new unassigned task in the specified board and inserts it to the first column.
        /// </summary>
        /// <param name="userEmail">The board member's email address.</param>
        /// <param name="boardName">The name of the board we want to insert the task into.</param>
        /// <param name="dueDate">The due date of the task.</param>
        /// <param name="title">The title of the task.</param>
        /// <param name="description">The description of the task.</param>
        /// <returns>A Json representation of a response containing a Task object, or an error message, if occured.</returns>
        public string CreateTask(string userEmail, string boardName, DateTime dueDate, string title, string description)
        {
            try
            {
                if (!_userController.IsLoggedIn(userEmail))
                {
                    string errorMessage = "Error: User must be logged in to create tasks.";
                    log.Error(errorMessage);
                    return new Response<Task>(errorMessage).ToJson();
                }
                BusinessLayer.Task task = _boardController.CreateTask(userEmail, boardName, dueDate, title, description);
                Task taskSer = new Task(task);
                log.Debug($"User {userEmail} has created a new task with the title {title} in board {boardName}.");
                return new Response<Task>(taskSer).ToJson();
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                return new Response<Task>(ex.Message).ToJson();
            }
        }

        /// <summary>
        /// This method moves the task with the specified taskId in the board with the specified board name,
        /// to the next column in the board.
        /// </summary>
        /// <param name="userEmail">The task assignee's email address.</param>
        /// <param name="boardName">The name of the board containing the task.</param>
        /// <param name="columnNumber">The current column number of the task.</param>
        /// <param name="taskId">The id of the task.</param>
        /// <returns>A Json representation of a response containing an error message, if occured.</returns>
        public string MoveTask(string userEmail, string boardName, int columnNumber, int taskId)
        {
            try
            {
                if (!_userController.IsLoggedIn(userEmail))
                {
                    string errorMessage = "Error: User must be logged in to move tasks.";
                    log.Error(errorMessage);
                    return new Response<Task>(errorMessage).ToJson();
                }
                _boardController.MoveTask(userEmail, boardName, columnNumber, taskId);
                log.Debug($"User {userEmail} has moved task {taskId} in board {boardName} from column {columnNumber} to column {columnNumber + 1}.");
                return new Response<Task>().ToJson();
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                return new Response<Task>(ex.Message).ToJson();
            }
        }

        /// <summary>
        /// This method edits the details of the task with the specified taskId,
        /// in the board with the specified board name.
        /// </summary>
        /// <param name="userEmail">The task assignee's email address.</param>
        /// <param name="boardName">The name of the board containing the task we want to edit.</param>
        /// <param name="taskId">The id of the task we want to edit.</param>
        /// <param name="dueDate">The new due date of the task.</param>
        /// <param name="title">The new title for the task.</param>
        /// <param name="description">The new description of the task.</param>
        /// <returns>A Json representation of a response containing an error message, if occured.</returns>
        public string EditTask(string userEmail, string boardName, int taskId, DateTime dueDate, string title, string description)
        {
            try
            {
                if (!_userController.IsLoggedIn(userEmail))
                {
                    string errorMessage = "Error: User must be logged in to edit tasks.";
                    log.Error(errorMessage);
                    return new Response<Task>(errorMessage).ToJson();
                }
                _boardController.EditTask(userEmail, boardName, taskId, dueDate, title, description);
                log.Debug($"User {userEmail} has edited task {taskId} in board {boardName}.");
                return new Response<Task>().ToJson();
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                return new Response<Task>(ex.Message).ToJson();
            }
        }

        /// <summary>
        /// This method returns the task with the specified taskId,
        /// in the board with the specified board name.
        /// </summary>
        /// <param name="userEmail">The email of the board member.</param>
        /// <param name="boardName">The board containing the task.</param>
        /// <param name="taskId">The ID of the task to return.</param>
        /// <returns>
        /// A Json representation of a response containing an error message, if occured,
        /// and a Json representation of a Task object as a return value, if exists.
        /// The Task Json is in the following form:
        /// {
        ///     "TaskID": int,
        ///     "Title": string,
        ///     "CreationTime": string of DateTime,
        ///     "DueDate": string of DateTime,
        ///     "Description": string,
        ///     "AssigneeUser": string
        /// }
        /// </returns>
        public string GetTask(string userEmail, string boardName, int taskId)
        {
            try
            {
                if (!_userController.IsLoggedIn(userEmail))
                {
                    string errorMessage = "Error: User must be logged in to get tasks.";
                    log.Error(errorMessage);
                    return new Response<Task>(errorMessage).ToJson();
                }

                BusinessLayer.Task task = _boardController.GetTask(userEmail, boardName, taskId);
                Task taskSer = new Task(task); // service object
                return new Response<Task>(taskSer).ToJson();
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                return new Response<Task>(ex.Message).ToJson();
            }
        }

        /// <summary>
        /// This method assigns the task with the specified task id, in the specified board, to a new assignee.
        /// </summary>
        /// <param name="userEmail">The email address of the user who wants to assign the task to himself or another user.</param>
        /// <param name="boardName">The board containing the task.</param>
        /// <param name="taskId">The id of the task we want to assign.</param>
        /// <param name="newAssignee">The email address of the user we want to assign the task to.</param>
        /// <returns>A Json representation of a response containing an error message, if occured.</returns>
        public string AssignTask(string userEmail, string boardName, int taskId, string newAssignee)
        {
            try
            {
                if (!_userController.IsLoggedIn(userEmail))
                {
                    string errorMessage = "Error: User must be logged in to assign tasks.";
                    log.Error(errorMessage);
                    return new Response<Task>(errorMessage).ToJson();
                }
                if(!_userController.UserExists(newAssignee))
                {
                    string errorMessage = "Error: User must exist in order to have tasks assigned.";
                    log.Error(errorMessage);
                    return new Response<Task>(errorMessage).ToJson();
                }
                _boardController.AssignTask(userEmail, boardName, taskId, newAssignee);
                log.Debug($"User {userEmail} has assigned task {taskId} in board {boardName} to user {newAssignee}.");
                return new Response<Task>().ToJson();
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                return new Response<Task>(ex.Message).ToJson();
            }
        }
    }
}
