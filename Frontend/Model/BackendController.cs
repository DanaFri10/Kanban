using System;
using System.Collections.Generic;
using System.Windows;
using System.Collections.ObjectModel;
using IntroSE.Kanban.Backend.ServiceLayer;

namespace Frontend.Model
{
    public class BackendController
    {
        private ServiceFactory Service { get; set; }
        public BackendController(ServiceFactory service)
        {
            this.Service = service;
        }

        public BackendController()
        {
            this.Service = new ServiceFactory();
            Service.LoadData();
        }

        public UserModel Login(string userEmail, string password)
        {
            String jsonLogin = Service.UserService.Login(userEmail, password);
            Response<UserModel> user = Response<UserModel>.FromJson(jsonLogin);
            if (user.ErrorOccured)
            {
                throw new Exception(user.ErrorMessage);
            }
            return new UserModel(this, userEmail);
        }
        
        public void Logout(string userEmail)
        {
            String jsonLogout = Service.UserService.Logout(userEmail);
            Response<UserModel> res = Response<UserModel>.FromJson(jsonLogout);
            if (res.ErrorOccured)
            {
                throw new Exception(res.ErrorMessage);
            }
        }

        internal UserModel Register(string userEmail, string password)
        {
            String json = Service.UserService.CreateUser(userEmail, password);
            Response<UserModel> res = Response<UserModel>.FromJson(json);
            if (res.ErrorOccured)
            {
                throw new Exception(res.ErrorMessage);
            }
            else
            {
                json = Service.UserService.GetUser(userEmail);
                Response<UserModel> user = Response<UserModel>.FromJson(json);
                if (res.ErrorOccured)
                {
                    throw new Exception(res.ErrorMessage);
                }
                return new UserModel(this, userEmail);
            }
        }

        /// <summary>
        /// This method gets a user email and returns a list of it's boards.
        /// </summary>
        /// <param name="userEmail">The user's email.</param>
        /// <returns>A list of the user's boards.</returns>
        /// <exception cref="Exception">If an error occured.</exception>
        internal List<BoardModel> ListBoardsOfUser(string userEmail)
        {
            string json = Service.BoardService.GetUserBoards(userEmail);
            Response<List<Board>> res = Response<List<Board>>.FromJson(json);
            if (res.ErrorOccured)
            {
                throw new Exception(res.ErrorMessage);
            }
            try
            { 
                List<Board> boards = res.DeserializeReturnValue();
                List<BoardModel> userBoards = new List<BoardModel>();
                foreach (Board board in boards)
                    userBoards.Add(BoardModelFromBoard(board));
                return userBoards;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        /// <summary>
        /// This method gets a board id and returns the amount of tasks in the board.
        /// </summary>
        /// <param name="BoardID">The board's id.</param>
        /// <returns>The amount of tasks in the board.</returns>
        /// <exception cref="Exception">If an error occured.</exception>
        internal int GetTasksCount(int BoardID)
        {
            String json = Service.BoardService.GetBoard(BoardID);
            Response<Board> res = Response<Board>.FromJson(json);
            if (res.ErrorOccured)
            {
                throw new Exception(res.ErrorMessage);
            }
            try
            {
                Board board = res.DeserializeReturnValue();
                int count = 0;
                foreach (Column c in board.Columns)
                    count += c.Tasks.Count;
                return count;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        /// <summary>
        /// This method converts a Board object into a BoardModel object, containing all the board's columns.
        /// </summary>
        /// <param name="board">The board received from the service layer.</param>
        /// <returns>A BoardModel object representing the specified board.</returns>
        private BoardModel BoardModelFromBoard(Board board)
        {
            List<ColumnModel> columnModels = new List<ColumnModel>();
            board.Columns.ForEach(column => columnModels.Add(ColumnModelFromColumn(column)));
            return new BoardModel(this, board.BoardID, board.BoardName, board.BoardOwner, GetTasksCount(board.BoardID), new ObservableCollection<ColumnModel>(columnModels));
        }

        /// <summary>
        /// This method converts a Column object into a ColumnModel object, containing all the column's tasks.
        /// </summary>
        /// <param name="column">The column received from the service layer.</param>
        /// <returns>A ColumnModel object representing the specified column.</returns>
        public ColumnModel ColumnModelFromColumn(Column column)
        {
            List<TaskModel> taskModels = new List<TaskModel>();
            foreach (Task task in column.Tasks.Values)
                taskModels.Add(TaskModelFromTask(task));
            return new ColumnModel(this, column.ColumnNumber, column.TasksLimit, new ObservableCollection<TaskModel>(taskModels));
        }

        /// <summary>
        /// This method converts a Task object into a TaskModel object, containing all the task information.
        /// </summary>
        /// <param name="task">The task received from the service layer.</param>
        /// <returns>A TaskModel object representing the specified task.</returns>
        public TaskModel TaskModelFromTask(Task task)
        {
            return new TaskModel(this, task.TaskID, task.CreationTime, task.Title, task.Description, task.DueDate, task.AssigneeUser);
        }
    }
}
