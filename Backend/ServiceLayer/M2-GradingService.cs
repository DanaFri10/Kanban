using System;
using System.Linq;
using System.Collections.Generic;


namespace IntroSE.Kanban.Backend.ServiceLayer
{
    /// <summary>
    /// A class for grading your work <b>ONLY</b>. The methods are not using good SE practices and you should <b>NOT</b> infer any insight on how to write the service layer/business layer. 
    /// <para>
    /// Each of the class' methods should return a JSON string with the following structure (see <see cref="System.Text.Json"/>):
    /// <code>
    /// {
    ///     "ErrorMessage": &lt;string&gt;,
    ///     "ReturnValue": &lt;object&gt;
    /// }
    /// </code>
    /// Where:
    /// <list type="bullet">
    ///     <item>
    ///         <term>ReturnValue</term>
    ///         <description>
    ///             The return value of the function.
    ///             <para>
    ///                 The value may be either a <paramref name="primitive"/>, a <paramref name="Task"/>, or an array of of them. See below for the definition of <paramref name="Task"/>.
    ///             </para>
    ///             <para>If the function does not return a value or an exception has occorred, then the field should be either null or undefined.</para>
    ///         </description>
    ///     </item>
    ///     <item>
    ///         <term>ErrorMessage</term>
    ///         <description>If an exception has occorred, then this field will contain a string of the error message. Otherwise, the field will be null or undefined.</description>
    ///     </item>
    /// </list>
    /// </para>
    /// <para>
    /// An empty response is a response that both fields are either null or undefined.
    /// </para>
    /// <para>
    /// The structure of the JSON of a Task, is:
    /// <code>
    /// {
    ///     "Id": &lt;int&gt;,
    ///     "CreationTime": &lt;DateTime&gt;,
    ///     "Title": &lt;string&gt;,
    ///     "Description": &lt;string&gt;,
    ///     "DueDate": &lt;DateTime&gt;
    /// }
    /// </code>
    /// </para>
    /// </summary>
    public class GradingService
    {
        private ServiceFactory _serviceFactory;
        public UserService UserService { get; }
        public BoardService BoardService { get; }
        public TaskService TaskService { get; }

        public GradingService()
        {
            _serviceFactory = new ServiceFactory();
            UserService = _serviceFactory.UserService;
            BoardService = _serviceFactory.BoardService;
            TaskService = _serviceFactory.TaskService;
        }



        /// <summary>
        /// This method registers a new user to the system.
        /// </summary>
        /// <param name="email">The user email address, used as the username for logging the system.</param>
        /// <param name="password">The user password.</param>
        /// <returns>An empty response, unless an error occurs (see <see cref="GradingService"/>)</returns>
        public string Register(string email, string password)
        {
            Response<User> response = Response<User>.FromJson(UserService.CreateUser(email, password));
            if (response.ErrorOccured)
                return new GradingResponse<object>(response.ErrorMessage).ToJson();
            return "{}";
        }


        /// <summary>
        ///  This method logs in an existing user.
        /// </summary>
        /// <param name="email">The email address of the user to login</param>
        /// <param name="password">The password of the user to login</param>
        /// <returns>A response with the user's email, unless an error occurs (see <see cref="GradingService"/>)</returns>
        public string Login(string email, string password)
        {
            Response<User> response = Response<User>.FromJson(UserService.Login(email, password));
            if (response.ErrorOccured)
                return new GradingResponse<object>(response.ErrorMessage).ToJson();
            return new GradingResponse<string>(null, email).ToJson();
        }


        /// <summary>
        /// This method logs out a logged in user. 
        /// </summary>
        /// <param name="email">The email of the user to log out</param>
        /// <returns>An empty response, unless an error occurs (see <see cref="GradingService"/>)</returns>
        public string Logout(string email)
        {
            Response<User> response = Response<User>.FromJson(UserService.Logout(email));
            if (response.ErrorOccured)
                return new GradingResponse<object>(response.ErrorMessage).ToJson();
            return "{}";
        }

        /// <summary>
        /// This method limits the number of tasks in a specific column.
        /// </summary>
        /// <param name="email">The email address of the user, must be logged in</param>
        /// <param name="boardName">The name of the board</param>
        /// <param name="columnOrdinal">The column ID. The first column is identified by 0, the ID increases by 1 for each column</param>
        /// <param name="limit">The new limit value. A value of -1 indicates no limit.</param>
        /// <returns>An empty response, unless an error occurs (see <see cref="GradingService"/>)</returns>
        public string LimitColumn(string email, string boardName, int columnOrdinal, int limit)
        {
            Response<Board> response = Response<Board>.FromJson(BoardService.LimitColumn(email, boardName, columnOrdinal, limit));
            if (response.ErrorOccured)
                return new GradingResponse<object>(response.ErrorMessage).ToJson();
            return "{}";
        }

        /// <summary>
        /// This method gets the limit of a specific column.
        /// </summary>
        /// <param name="email">The email address of the user, must be logged in</param>
        /// <param name="boardName">The name of the board</param>
        /// <param name="columnOrdinal">The column ID. The first column is identified by 0, the ID increases by 1 for each column</param>
        /// <returns>A response with the column's limit, unless an error occurs (see <see cref="GradingService"/>)</returns>
        public string GetColumnLimit(string email, string boardName, int columnOrdinal)
        {
            Response<Board> response = Response<Board>.FromJson(BoardService.GetBoard(email, boardName));
            if (response.ErrorOccured)
                return new GradingResponse<object>(response.ErrorMessage).ToJson();

            Board board = response.DeserializeReturnValue();
            if (columnOrdinal < 0 || columnOrdinal >= board.Columns.Count)
                return new GradingResponse<object>("Error: Invalid column ID.").ToJson();

            int columnLimit = board.Columns[columnOrdinal].TasksLimit;
            return new GradingResponse<int>(null, columnLimit).ToJson();
        }


        /// <summary>
        /// This method gets the name of a specific column
        /// </summary>
        /// <param name="email">The email address of the user, must be logged in</param>
        /// <param name="boardName">The name of the board</param>
        /// <param name="columnOrdinal">The column ID. The first column is identified by 0, the ID increases by 1 for each column</param>
        /// <returns>A response with the column's name, unless an error occurs (see <see cref="GradingService"/>)</returns>
        public string GetColumnName(string email, string boardName, int columnOrdinal)
        {
            Response<Board> response = Response<Board>.FromJson(BoardService.GetBoard(email, boardName));
            if (response.ErrorOccured)
                return new GradingResponse<object>(response.ErrorMessage).ToJson();

            Board board = response.DeserializeReturnValue();
            string result;
            switch (columnOrdinal)
            {
                case 0:
                    result = "backlog";
                    break;
                case 1:
                    result = "in progress";
                    break;
                case 2:
                    result = "done";
                    break;
                default:
                    return new GradingResponse<object>("Error: Invalid column ID.").ToJson();
            }
            return new GradingResponse<string>(null, result).ToJson();
        }


        /// <summary>
        /// This method adds a new task.
        /// </summary>
        /// <param name="email">Email of the user. The user must be logged in.</param>
        /// <param name="boardName">The name of the board</param>
        /// <param name="title">Title of the new task</param>
        /// <param name="description">Description of the new task</param>
        /// <param name="dueDate">The due date if the new task</param>
        /// <returns>An empty response, unless an error occurs (see <see cref="GradingService"/>)</returns>
        public string AddTask(string email, string boardName, string title, string description, DateTime dueDate)
        {
            Response<Task> response = Response<Task>.FromJson(TaskService.CreateTask(email, boardName, dueDate, title, description));
            if (response.ErrorOccured)
                return new GradingResponse<object>(response.ErrorMessage).ToJson();
            //return new GradingResponse<object>(null, email).ToJson();
            return "{}";
        }


        /// <summary>
        /// This method updates the due date of a task
        /// </summary>
        /// <param name="email">Email of the user. Must be logged in</param>
        /// <param name="boardName">The name of the board</param>
        /// <param name="columnOrdinal">The column ID. The first column is identified by 0, the ID increases by 1 for each column</param>
        /// <param name="taskId">The task to be updated identified task ID</param>
        /// <param name="dueDate">The new due date of the column</param>
        /// <returns>An empty response, unless an error occurs (see <see cref="GradingService"/>)</returns>
        public string UpdateTaskDueDate(string email, string boardName, int columnOrdinal, int taskId, DateTime dueDate)
        {
            Response<Board> response = Response<Board>.FromJson(BoardService.GetBoard(email, boardName));
            if (response.ErrorOccured)
                return new GradingResponse<object>(response.ErrorMessage).ToJson();
            // Necessary here because our EditTask searches all columns. (taskId unique to board)
            Board board = response.DeserializeReturnValue();
            if (columnOrdinal < 0 || columnOrdinal >= board.Columns.Count)
                return new GradingResponse<object>("Error: Invalid column ID: " + columnOrdinal + ".").ToJson();
            if (!board.Columns[columnOrdinal].Tasks.ContainsKey(taskId))
                return new GradingResponse<object>("Error: Column doesn't have a task with this task ID.").ToJson();

            Task task = board.Columns[columnOrdinal].Tasks[taskId];

            Response<Task> response3 = Response<Task>.FromJson(TaskService.EditTask(email, boardName, taskId, dueDate, task.Title, task.Description));
            if (response3.ErrorOccured)
                return new GradingResponse<object>(response3.ErrorMessage).ToJson();

            return "{}";
        }


        /// <summary>
        /// This method updates task title.
        /// </summary>
        /// <param name="email">Email of user. Must be logged in</param>
        /// <param name="boardName">The name of the board</param>
        /// <param name="columnOrdinal">The column ID. The first column is identified by 0, the ID increases by 1 for each column</param>
        /// <param name="taskId">The task to be updated identified task ID</param>
        /// <param name="title">New title for the task</param>
        /// <returns>An empty response, unless an error occurs (see <see cref="GradingService"/>)</returns>
        public string UpdateTaskTitle(string email, string boardName, int columnOrdinal, int taskId, string title)
        {
            Response<Board> response = Response<Board>.FromJson(BoardService.GetBoard(email, boardName));
            if (response.ErrorOccured)
                return new GradingResponse<object>(response.ErrorMessage).ToJson();
            Board board = response.DeserializeReturnValue();
            if (columnOrdinal < 0 || columnOrdinal >= board.Columns.Count)
                return new GradingResponse<object>("Error: Invalid column ID: " + columnOrdinal + ".").ToJson();
            if (!board.Columns[columnOrdinal].Tasks.ContainsKey(taskId))
                return new GradingResponse<object>("Error: Column doesn't have a task with this task ID.").ToJson();

            Task task = board.Columns[columnOrdinal].Tasks[taskId];
            Response<Task> response2 = Response<Task>.FromJson(TaskService.EditTask(email, boardName, taskId, task.DueDate, title, task.Description));
            if (response2.ErrorOccured)
                return new GradingResponse<object>(response2.ErrorMessage).ToJson();

            return "{}";
        }


        /// <summary>
        /// This method updates the description of a task.
        /// </summary>
        /// <param name="email">Email of user. Must be logged in</param>
        /// <param name="boardName">The name of the board</param>
        /// <param name="columnOrdinal">The column ID. The first column is identified by 0, the ID increases by 1 for each column</param>
        /// <param name="taskId">The task to be updated identified task ID</param>
        /// <param name="description">New description for the task</param>
        /// <returns>An empty response, unless an error occurs (see <see cref="GradingService"/>)</returns>
        public string UpdateTaskDescription(string email, string boardName, int columnOrdinal, int taskId, string description)
        {
            Response<Board> response = Response<Board>.FromJson(BoardService.GetBoard(email, boardName));
            if (response.ErrorOccured)
                return new GradingResponse<object>(response.ErrorMessage).ToJson();
            Board board = response.DeserializeReturnValue();
            if (columnOrdinal < 0 || columnOrdinal >= board.Columns.Count)
                return new GradingResponse<object>("Error: Invalid column ID: " + columnOrdinal + ".").ToJson();
            if (!board.Columns[columnOrdinal].Tasks.ContainsKey(taskId))
                return new GradingResponse<object>("Error: Column doesn't have a task with this task ID.").ToJson();

            Task task = board.Columns[columnOrdinal].Tasks[taskId];
            Response<Task> response2 = Response<Task>.FromJson(TaskService.EditTask(email, boardName, taskId, task.DueDate, task.Title, description));
            if (response2.ErrorOccured)
                return new GradingResponse<object>(response2.ErrorMessage).ToJson();

            return "{}";
        }


        /// <summary>
        /// This method advances a task to the next column
        /// </summary>
        /// <param name="email">Email of user. Must be logged in</param>
        /// <param name="boardName">The name of the board</param>
        /// <param name="columnOrdinal">The column ID. The first column is identified by 0, the ID increases by 1 for each column</param>
        /// <param name="taskId">The task to be updated identified task ID</param>
        /// <returns>An empty response, unless an error occurs (see <see cref="GradingService"/>)</returns>
        public string AdvanceTask(string email, string boardName, int columnOrdinal, int taskId)
        {
            Response<Task> response = Response<Task>.FromJson(TaskService.MoveTask(email, boardName, columnOrdinal, taskId));
            if (response.ErrorOccured)
                return new GradingResponse<object>(response.ErrorMessage).ToJson();
            return "{}";
        }


        /// <summary>
        /// This method returns a column given it's name
        /// </summary>
        /// <param name="email">Email of the user, must be logged in</param>
        /// <param name="boardName">The name of the board</param>
        /// <param name="columnOrdinal">The column ID. The first column is identified by 0, the ID increases by 1 for each column</param>
        /// <returns>A response with a list of the column's tasks, unless an error occurs (see <see cref="GradingService"/>)</returns>
        public string GetColumn(string email, string boardName, int columnOrdinal)
        {
            Response<Board> response = Response<Board>.FromJson(BoardService.GetBoard(email, boardName));
            if (response.ErrorOccured)
                return new GradingResponse<object>(response.ErrorMessage).ToJson();

            Board board = response.DeserializeReturnValue();
            if (columnOrdinal < 0 || columnOrdinal >= board.Columns.Count)
                return new GradingResponse<object>("Error: Invalid column ID.").ToJson();

            List<Task> tasksList = board.Columns[columnOrdinal].Tasks.Values.ToList();
            List<GradingTask> gradingTasks = new List<GradingTask>();
            foreach (Task task in tasksList)
            {
                gradingTasks.Add(new GradingTask(task));
            }
            return new GradingResponse<List<GradingTask>>(null, gradingTasks).ToJson();
        }


        /// <summary>
        /// This method adds a board to the specific user.
        /// </summary>
        /// <param name="email">Email of the user, must be logged in</param>
        /// <param name="name">The name of the new board</param>
        /// <returns>An empty response, unless an error occurs (see <see cref="GradingService"/>)</returns>
        public string AddBoard(string email, string name)
        {
            Response<Board> response = Response<Board>.FromJson(BoardService.CreateBoard(email, name));
            if (response.ErrorOccured)
                return new GradingResponse<object>(response.ErrorMessage).ToJson();
            return "{}";
        }


        /// <summary>
        /// This method deletes a board.
        /// </summary>
        /// <param name="email">Email of the user, must be logged in and an owner of the board.</param>
        /// <param name="name">The name of the board</param>
        /// <returns>An empty response, unless an error occurs (see <see cref="GradingService"/>)</returns>
        public string RemoveBoard(string email, string name)
        {
            Response<Board> response = Response<Board>.FromJson(BoardService.RemoveBoard(email, name));
            if (response.ErrorOccured)
                return new GradingResponse<object>(response.ErrorMessage).ToJson();
            return "{}";
        }


        /// <summary>
        /// This method returns all in-progress tasks of a user.
        /// </summary>
        /// <param name="email">Email of the user. Must be logged in</param>
        /// <returns>A response with a list of the in-progress tasks of the user, unless an error occurs (see <see cref="GradingService"/>)</returns>
        public string InProgressTasks(string email)
        {
            Response<List<Task>> response = Response<List<Task>>.FromJson(TaskService.ListInProgressTasks(email));
            if (response.ErrorOccured)
                return new GradingResponse<object>(response.ErrorMessage).ToJson();

            List<Task> inProgressTasks = response.DeserializeReturnValue();
            List<GradingTask> gradingTasks = new List<GradingTask>();
            foreach (Task task in inProgressTasks)
            {
                gradingTasks.Add(new GradingTask(task));
            }
            return new GradingResponse<List<GradingTask>>(null, gradingTasks).ToJson();
        }

        /// <summary>
        /// This method returns a list of IDs of all user's boards.
        /// </summary>
        /// <param name="email"></param>
        /// <returns>A response with a list of IDs of all user's boards, unless an error occurs (see <see cref="GradingService"/>)</returns>
        public string GetUserBoards(string email)
        {
            Response<List<Board>> response = Response<List<Board>>.FromJson(BoardService.GetUserBoards(email));
            if (response.ErrorOccured)
                return new GradingResponse<object>(response.ErrorMessage).ToJson();

            List<int> boardIDs = new List<int>();
            foreach (Board board in response.DeserializeReturnValue())
            {
                boardIDs.Add(board.BoardID);
            }
            return new GradingResponse<List<int>>(null, boardIDs).ToJson();
        }

        /// <summary>
        /// This method returns a board's name
        /// </summary>
        /// <param name="boardId">The board's ID</param>
        /// <returns>A response with the board's name, unless an error occurs (see <see cref="GradingService"/>)</returns>
        public string GetBoardName(int boardId)
        {
            Response<Board> response = Response<Board>.FromJson(BoardService.GetBoard(boardId));
            if (response.ErrorOccured)
                return new GradingResponse<object>(response.ErrorMessage).ToJson();
            Board board = response.DeserializeReturnValue();
            return new GradingResponse<string>(null, board.BoardName).ToJson();
        }

        /// <summary>
        /// This method adds a user as member to an existing board.
        /// </summary>
        /// <param name="email">The email of the user that joins the board. Must be logged in</param>
        /// <param name="boardID">The board's ID</param>
        /// <returns>An empty response, unless an error occurs (see <see cref="GradingService"/>)</returns>
        public string JoinBoard(string email, int boardID)
        {
            Response<Board> response = Response<Board>.FromJson(BoardService.JoinBoard(email, boardID));
            if (response.ErrorOccured)
                return new GradingResponse<object>(response.ErrorMessage).ToJson();
            return "{}";
        }

        /// <summary>
        /// This method removes a user from the members list of a board.
        /// </summary>
        /// <param name="email">The email of the user. Must be logged in</param>
        /// <param name="boardID">The board's ID</param>
        /// <returns>An empty response, unless an error occurs (see <see cref="GradingService"/>)</returns>
        public string LeaveBoard(string email, int boardID)
        {
            // Since our LeaveBoard functionality receives a board name and not board ID, we have to get its name first.
            Response<Board> response1 = Response<Board>.FromJson(BoardService.GetBoard(email, boardID));  
            if (response1.ErrorOccured)
                return new GradingResponse<object>(response1.ErrorMessage).ToJson();
            string boardName = response1.DeserializeReturnValue().BoardName;
            Response<Board> response2 = Response<Board>.FromJson(BoardService.LeaveBoard(email, boardName));
            if (response2.ErrorOccured)
                return new GradingResponse<object>(response2.ErrorMessage).ToJson();
            return "{}";
        }

        /// <summary>
        /// This method assigns a task to a user
        /// </summary>
        /// <param name="email">Email of the user. Must be logged in</param>
        /// <param name="boardName">The name of the board</param>
        /// <param name="columnOrdinal">The column number. The first column is 0, the number increases by 1 for each column</param>
        /// <param name="taskID">The task to be updated identified a task ID</param>        
        /// <param name="emailAssignee">Email of the asignee user</param>
        /// <returns>An empty response, unless an error occurs (see <see cref="GradingService"/>)</returns>
        public string AssignTask(string email, string boardName, int columnOrdinal, int taskID, string emailAssignee)
        {
            // Since our AssignTask functionality doesn't receive the column number, we have to check this here.
            Response<Board> response1 = Response<Board>.FromJson(BoardService.GetBoard(email, boardName));
            if (response1.ErrorOccured)
                return new GradingResponse<object>(response1.ErrorMessage).ToJson();
            Board board = response1.DeserializeReturnValue();
            if (columnOrdinal < 0 || columnOrdinal >= board.Columns.Count)
                return new GradingResponse<object>("Error: Invalid column ID: " + columnOrdinal + ".").ToJson();
            if (!board.Columns[columnOrdinal].Tasks.ContainsKey(taskID))
                return new GradingResponse<object>("Error: Column doesn't have a task with this task ID.").ToJson();

            Response<Task> response2 = Response<Task>.FromJson(TaskService.AssignTask(email, boardName, taskID, emailAssignee));
            if (response2.ErrorOccured)
                return new GradingResponse<object>(response2.ErrorMessage).ToJson();
            return "{}";
        }

        ///<summary>This method loads all persisted data.
        ///<para>
        ///<b>IMPORTANT:</b> When starting the system via the GradingService - do not load the data automatically, only through this method. 
        ///In some cases we will call LoadData when the program starts and in other cases we will call DeleteData. Make sure you support both options.
        ///</para>
        /// </summary>
        /// <returns>An empty response, unless an error occurs (see <see cref="GradingService"/>)</returns>
        public string LoadData()
        {
            Response<object> response = Response<object>.FromJson(_serviceFactory.LoadData());
            if (response.ErrorOccured)
                return new GradingResponse<object>(response.ErrorMessage).ToJson();
            return "{}";
        }

        ///<summary>This method deletes all persisted data.
        ///<para>
        ///<b>IMPORTANT:</b> 
        ///In some cases we will call LoadData when the program starts and in other cases we will call DeleteData. Make sure you support both options.
        ///</para>
        /// </summary>
        ///<returns>An empty response, unless an error occurs (see <see cref="GradingService"/>)</returns>
        public string DeleteData()
        {
            Response<object> response = Response<object>.FromJson(_serviceFactory.DeleteData());
            if (response.ErrorOccured)
                return new GradingResponse<object>(response.ErrorMessage).ToJson();
            string json = new GradingResponse<object>().ToJson();
            return json;
        }

        /// <summary>
        /// This method transfers a board ownership.
        /// </summary>
        /// <param name="currentOwnerEmail">Email of the current owner. Must be logged in</param>
        /// <param name="newOwnerEmail">Email of the new owner</param>
        /// <param name="boardName">The name of the board</param>
        /// <returns>An empty response, unless an error occurs (see <see cref="GradingService"/>)</returns>
        public string TransferOwnership(string currentOwnerEmail, string newOwnerEmail, string boardName)
        {
            Response<Board> response = Response<Board>.FromJson(BoardService.TransferOwner(currentOwnerEmail, boardName, newOwnerEmail));
            if (response.ErrorOccured)
                return new GradingResponse<object>(response.ErrorMessage).ToJson();
            return "{}";
        }
    }
}
