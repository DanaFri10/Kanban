using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;
using log4net;
using IntroSE.Kanban.Backend.DataAccessLayer;

namespace IntroSE.Kanban.Backend.BusinessLayer
{
    /// <summary>
    /// BoardController class interacts with the Board system of the application.
    /// </summary>
    public class BoardController
    {
        private Dictionary<int, Board> _boards;
        private BoardMapper _mapper;

        private static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>
        /// Initializes a new instance of the BoardController class.
        /// </summary>
        public BoardController()
        {
            _boards = new Dictionary<int, Board>();
            _mapper = new BoardMapper();
            log.Info("Initialized BoardController.");
        }

        /// <summary>
        /// This method creates a new board to the user with the specified email address.
        /// </summary>
        /// <param name="userEmail">The user's email address.</param>
        /// <param name="boardName">The name of the board.</param>
        /// <returns>A new board with the specified parameters.</returns>
        /// <exception cref="ArgumentNullException">If one of the parameters is null.</exception>
        /// <exception cref="ArgumentException">If the user already has a board with the specified name.</exception>
        public Board CreateBoard(string userEmail, string boardName)
        {
            if (userEmail == null)
            {
                log.Error("Error: Invalid user email: null.");
                throw new ArgumentNullException("Error: Invalid user email: null.");
            }

            userEmail = userEmail.ToLower();

            if (boardName == null || String.IsNullOrWhiteSpace(boardName))
            {
                log.Error("Error: Invalid board name: null.");
                throw new ArgumentNullException("Error: Invalid board name: null.");
            }

            Board board = GetBoardOrNull(userEmail, boardName);

            if (board!=null)
            {
                log.Error("Error: This user already has a board with that board name.");
                throw new ArgumentException("Error: This user already has a board with that board name.");
            }

            Board newBoard=new Board(boardName, userEmail); //inserts to DB
            _boards.Add(newBoard.BoardID, newBoard);

            log.Debug($"Created board {boardName} has been given the ID {newBoard.BoardID}.");
            return newBoard;
        }

        /// <summary>
        /// This method removes the board with the specified name and board owner.
        /// </summary>
        /// <param name="userEmail">The user's email address.</param>
        /// <param name="boardName">The name of the board.</param>
        /// <exception cref="ArgumentNullException">If one of the parameters is null.</exception>
        /// /// <exception cref="ArgumentException">If the user does not have a board with the specified name
        /// or the user is not the board owner.</exception>
        public void RemoveBoard(string userEmail, string boardName)
        {
            if (userEmail == null)
            {
                log.Error("Error: Invalid user email: null.");
                throw new ArgumentNullException("Error: Invalid user email: null.");
            }

            userEmail=userEmail.ToLower();

            if (boardName == null)
            {
                log.Error("Error: Invalid board name: null.");
                throw new ArgumentNullException("Error: Invalid board name: null.");
            }

            Board board=GetBoardOrNull(userEmail, boardName);

            if (board == null)
            {
                log.Error("Error: This user does not have a board with that board name.");
                throw new ArgumentException("Error: This user does not have a board with that board name.");
            }
            if (!board.BoardOwner.Equals(userEmail))
            {
                log.Error("Error: A user that is not the board owner can not delete the board.");
                throw new ArgumentException("Error: A user that is not the board owner can not delete the board.");
            }

            board.BoardDTO.Delete();
            _boards.Remove(board.BoardID);

            log.Debug($"The board {board.BoardID} was removed.");
        }

        /// <summary>
        /// This method returns the board with the specified user and name.
        /// </summary>
        /// <param name="userEmail">The user's email address.</param>
        /// <param name="boardName">The name of the board.</param>
        /// <returns>The board with the specified user and name.</returns>
        /// <exception cref="ArgumentNullException">If one of the fields is null.</exception>
        /// <exception cref="ArgumentException">If the board was not found.</exception>
        public Board GetBoard(string userEmail, string boardName)
        {
            if (userEmail == null)
                throw new ArgumentNullException("Error: Invalid user email: null.");

            userEmail = userEmail.ToLower();

            if (boardName == null)
                throw new ArgumentNullException("Error: Invalid board name: null");

            List<Board> userBoards = GetUserBoards(userEmail);

            if (userBoards.Count==0)
                throw new ArgumentException("Error: User doesn't have any boards.");

            foreach (Board board in userBoards)
            {
                if (board.BoardName.Equals(boardName))
                    return board;
            }
            throw new ArgumentException("Error: User doesn't have a board with this board name.");
        }

        /// <summary>
        /// This method returns the board with the specified user and name if found and null otherwise.
        /// </summary>
        /// <param name="userEmail">The user's email address.</param>
        /// <param name="boardName">The name of the board.</param>
        /// <returns>The board with the specified user and name if found and null otherwise.</returns>
        private Board GetBoardOrNull(string userEmail, string boardName)
        {
            Board board;
            try
            {
                board = GetBoard(userEmail, boardName);
            }
            catch (Exception ex)
            {
                board = null;
            }

            return board;
        }

        /// <summary>
        /// This method returns the board with the specified user and id.
        /// </summary>
        /// <param name="userEmail">The user's email address.</param>
        /// <param name="boardID">The id of the board.</param>
        /// <returns>The board with the specified user and id.</returns>
        /// <exception cref="ArgumentNullException">If one of the fields is null or invalid.</exception>
        /// <exception cref="ArgumentException">If the board was not found or the user is not a board member of
        /// the board with the specified board id.</exception>
        public Board GetBoard(string userEmail, int boardID)
        {
            if (userEmail == null)
                throw new ArgumentNullException("Error: Invalid user email: null.");

            userEmail = userEmail.ToLower();

            if (boardID < 0)
                throw new ArgumentNullException("Error: Invalid board id: negative");

            Board board = _boards.GetValueOrDefault(boardID, null);

            if (board == null)
                throw new ArgumentException("Error: This board does not exist.");
            if(!board.IsBoardMember(userEmail))
                throw new ArgumentException("Error: This user is not a board member of the board with the specified board id.");

            return board;
        }

        /// <summary>
        /// This method returns the board with the specified user and id if found and null otherwise.
        /// </summary>
        /// <param name="userEmail">The user's email address.</param>
        /// <param name="boardID">The id of the board.</param>
        /// <returns>The board with the specified user and id if found and null otherwise.</returns>
        private Board GetBoardOrNull(string userEmail, int boardID)
        {
            Board board;
            try
            {
                board = GetBoard(userEmail, boardID);
            }
            catch (Exception ex)
            {
                board = null;
            }

            return board;
        }

        /// <summary>
        /// This method returns the board with the specified  id.
        /// </summary>
        /// <param name="boardID">The id of the board.</param>
        /// <returns>The board with the specified id.</returns>
        /// <exception cref="ArgumentNullException">If one of the fields is null or invalid.</exception>
        /// <exception cref="ArgumentException">If the board was not found.</exception>
        public Board GetBoard(int boardID)
        {
            if (boardID < 0)
                throw new ArgumentNullException("Error: Invalid board id: negative");

            Board board = _boards.GetValueOrDefault(boardID, null);

            if (board == null)
                throw new ArgumentException("Error: This board does not exist.");
            
            return board;
        }

        /// <summary>
        /// This method returns the board with the specified id if found and null otherwise.
        /// </summary>
        /// <param name="boardID">The id of the board.</param>
        /// <returns>The board with the specified id if found and null otherwise.</returns>
        private Board GetBoardOrNull(int boardID)
        {
            Board board;
            try
            {
                board = GetBoard(boardID);
            }
            catch (Exception ex)
            {
                board = null;
            }

            return board;
        }

        /// <summary>
        /// This methods returns a list of all the tasks this user is assigned to, from all boards. 
        /// </summary>
        /// <param name="userEmail">The user's email address.</param>
        /// <returns>returns a list of all the tasks this user is assigned to, from all boards.</returns>
        /// <exception cref="ArgumentNullException">If one of the fields is null.</exception>
        public List<Task> ListInProgressTasks(string userEmail)
        {
            if (userEmail == null)
                throw new ArgumentNullException("Error: Invalid user email: null.");

            userEmail = userEmail.ToLower();

            List<Board> userBoards = GetUserBoards(userEmail);

            List<Task> tasks = new List<Task>();
            foreach(Board board in userBoards)
            {
                tasks.AddRange(board.GetColumn(Board.IN_PROGRESS_COLUMN).GetTasksOfAssignee(userEmail));
            }

            log.Info($"User {userEmail} got a list of all his tasks in progress.");
            return tasks;
        }

        /// <summary>
        /// This method returns a list of all the boards the user is a member of.
        /// </summary>
        /// <param name="userEmail">The user's email address.</param>
        /// <returns>A list of all the boards the user is a member of.</returns>
        /// <exception cref="ArgumentNullException">If one of the fields is null.</exception>
        public List<Board> GetUserBoards(string userEmail)
        {
            if (userEmail == null)
                throw new ArgumentNullException("Error: Invalid user email: null.");

            userEmail = userEmail.ToLower();

            List<Board> boards = new List<Board>();
            foreach(Board board in _boards.Values)
                if(board.IsBoardMember(userEmail))
                    boards.Add(board);
            log.Info($"User {userEmail} got a list of all his boards.");
            return boards;
        }

        /// <summary>
        /// This methods adds a new board member and returns the board.
        /// </summary>
        /// <param name="userEmail">The user's email address.</param>
        /// <param name="boardID">The id of the board.</param>
        /// <returns>The board the user joined.</returns>
        /// <exception cref="ArgumentNullException">If one of the fields is null or invalid.</exception>
        /// <exception cref="ArgumentException">If the board does not exist or the user is already a board member.</exception>
        public Board JoinBoard(string userEmail, int boardID)
        {
            if (userEmail == null)
            {
                log.Error("Error: Invalid user email: null.");
                throw new ArgumentNullException("Error: Invalid user email: null.");
            }

            userEmail = userEmail.ToLower();

            if (boardID < 0)
            {
                log.Error("Error: Invalid board id: negative.");
                throw new ArgumentNullException("Error: Invalid board id: negative.");
            }

            Board board = GetBoardOrNull(boardID);
            if (board==null)
            {
                log.Error("Error: This board does not exist.");
                throw new ArgumentException("Error: This board does not exist.");
            }

            List<Board> boards = GetUserBoards(userEmail);
            foreach (Board b in boards)
            {
                if(b.BoardID == boardID)
                {
                    log.Error("Error: This user is already a board member.");
                    throw new ArgumentException("Error: This user is already a board member.");
                }
                if(b.BoardName.Equals(board.BoardName))
                {
                    log.Error("Error: This user already has a board with this name.");
                    throw new ArgumentException("Error: This user already has a board with this name.");
                }
            }

            board.AddMember(userEmail); //inserts to DB

            log.Debug($"The user {userEmail} joined board {boardID}.");

            return board;
        }

        /// <summary>
        /// This method loads the boards to the board controller.
        /// </summary>
        public void LoadData()
        {
            List<BoardDTO>boardDTOs=_mapper.LoadAllBoards();
            foreach (BoardDTO boardDTO in boardDTOs)
            {
                Board board = new Board(boardDTO);
                _boards.Add(board.BoardID, board);
            }
            log.Debug($"Loaded all boards to board controller.");
        }

        /// <summary>
        /// This methods limit a column for the specified user and board.
        /// </summary>
        /// <param name="userEmail">The user's email address.</param>
        /// <param name="boardName">The board name.</param>
        /// <param name="columnNumber">The column number.</param>
        /// <param name="tasksLimit">The limit of the column.</param>
        public void LimitColumn(string userEmail, string boardName, int columnNumber, int tasksLimit)
        {
            GetBoard(userEmail, boardName).GetColumn(columnNumber).LimitColumn(tasksLimit);
        }

        /// <summary>
        /// This method enables a user to leave the specified board.
        /// </summary>
        /// <param name="userEmail">The user email.</param>
        /// <param name="boardName">The board name.</param>
        public void LeaveBoard(string userEmail, string boardName)
        {
            GetBoard(userEmail, boardName).RemoveMember(userEmail);
        }

        /// <summary>
        /// This method transfers the board ownership.
        /// </summary>
        /// <param name="userEmail">The old owner's email.</param>
        /// <param name="boardName">The board name.</param>
        /// <param name="newOwner">The new owner's email.</param>
        public void TransferOwner(string userEmail, string boardName, string newOwner)
        {
            GetBoard(userEmail, boardName).TransferOwner(userEmail, newOwner);
        }

        /// <summary>
        /// This methods creates a new task in the board.
        /// </summary>
        /// <param name="userEmail">The user's email.</param>
        /// <param name="boardName">The board name the task is created into.</param>
        /// <param name="dueDate">The due date of the task.</param>
        /// <param name="title">The title of the task</param>
        /// <param name="description">The description of the task.</param>
        /// <returns></returns>
        public Task CreateTask(string userEmail, string boardName, DateTime dueDate, string title, string description)
        {
            return GetBoard(userEmail, boardName).CreateTask(userEmail, dueDate, title, description);
        }

        /// <summary>
        /// This method moves a task to the next column.
        /// </summary>
        /// <param name="userEmail">The email of the user trying to move the task.</param>
        /// <param name="boardName">The board the task is in.</param>
        /// <param name="columnNumber">The current column number.</param>
        /// <param name="taskId">The id of the moved task.</param>
        public void MoveTask(string userEmail, string boardName, int columnNumber, int taskId)
        {
            GetBoard(userEmail, boardName).MoveTask(userEmail, columnNumber, taskId);
        }

        /// <summary>
        /// This method enables a user to edit a task.
        /// </summary>
        /// <param name="userEmail">The user's email.</param>
        /// <param name="boardName">The board name the task is created into.</param>
        /// <param name="taskId">The id of the edited task.</param>
        /// <param name="dueDate">The due date of the task.</param>
        /// <param name="title">The title of the task</param>
        /// <param name="description">The description of the task.</param>
        public void EditTask(string userEmail, string boardName, int taskId, DateTime dueDate, string title, string description)
        {
            GetBoard(userEmail, boardName).EditTask(userEmail, taskId, dueDate, title, description);
        }

        /// <summary>
        /// This method returns a task from the specified board with the task id.
        /// </summary>
        /// <param name="userEmail">The user's email.</param>
        /// <param name="boardName">The name of the board.</param>
        /// <param name="taskId">The id of the task we want to get.</param>
        /// <returns>The task.</returns>
        public Task GetTask(string userEmail, string boardName, int taskId)
        {
            return GetBoard(userEmail, boardName).GetTask(taskId);
        }

        /// <summary>
        /// This method enables the userEmail to assign the task to the newAssignee.
        /// </summary>
        /// <param name="userEmail">The email of the old assignee.</param>
        /// <param name="boardName">The name of the board.</param>
        /// <param name="taskId">The assigned task.</param>
        /// <param name="newAssignee">The email of the new assignee.</param>
        public void AssignTask(string userEmail, string boardName, int taskId, string newAssignee)
        {
            GetBoard(userEmail, boardName).AssignTask(taskId, userEmail, newAssignee);
        }


        /// <summary>
        /// This method deletes all the Board data from the system and from the database.
        /// </summary>
        public void DeleteData()
        {
            _mapper.DeleteAllBoards();
            _boards.Clear();
            log.Debug("Deleted all Board data from BoardController and from the database.");
        }
    }


}
