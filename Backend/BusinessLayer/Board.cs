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
    /// Class Board represents a board in the Kanban system.
    /// </summary>
    public class Board
    {
        public static readonly int BACKLOG_COLUMN = 0;
        public static readonly int IN_PROGRESS_COLUMN = 1;
        public static readonly int DONE_COLUMN = 2;
        public static readonly int COUNT_COLUMNS = 3;
        public int BoardID { get; }

        private string _boardOwner;
        public string BoardOwner { get=> _boardOwner; private set { BoardDTO.BoardOwner = value; _boardOwner = value; } }
        public string BoardName { get; }
        private List<Column> _columns;
        private HashSet<string> _boardMembers;
        public BoardDTO BoardDTO { get;  }

        private static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>
        /// Initializes a new instance of the Board class, with the given board name and board owner.
        /// </summary>
        /// <param name="boardName">Name of the board.</param>
        /// <param name="boardOwner">Owner of the board.</param>
        /// <exception cref="ArgumentNullException">If one of the fields is null.</exception>
        public Board(string boardName, string boardOwner)
        {
            if (boardName == null)
            {
                log.Error("Error: The board name is null.");
                throw new ArgumentNullException("Error: The board name is null.");
            }
            if (boardOwner == null)
            {
                log.Error("Error: The board owner is null.");
                throw new ArgumentNullException("Error: The board owner is null.");
            }

            boardOwner=boardOwner.ToLower();

            BoardName = boardName;
            _boardOwner = boardOwner;
            
            BoardDTO=new BoardDTO(boardName, boardOwner);
            BoardDTO.Insert();
            BoardID = BoardDTO.BoardID;
            
            _columns = new List<Column>();
            for(int i = BACKLOG_COLUMN; i < COUNT_COLUMNS; i++)
            {
                Column c = new Column(BoardID, i);
                _columns.Add(c);
                BoardDTO.Columns.Add(c.ColumnDTO);
            }

            _boardMembers = new HashSet<string>();
            AddMember(boardOwner);

            log.Debug($"A new board was created with the following parameters: boardName={boardName}, boardOwner={boardOwner}.");
        }

        /// <summary>
        /// Initializes a new instance of the Board class, according to the given BoardDTO object.
        /// </summary>
        /// <param name="boardDTO">The given BoardDTO object.</param>
        /// <exception cref="ArgumentNullException">If the given BoardDTO object is null.</exception>
        public Board(BoardDTO boardDTO)
        {
            if (boardDTO == null)
            {
                log.Error("Error: The boardDTO is null.");
                throw new ArgumentNullException("Error: The boardDTO is null.");
            }

            this.BoardDTO = boardDTO;
            BoardID = BoardDTO.BoardID;
            _boardOwner = BoardDTO.BoardOwner;
            BoardName = BoardDTO.BoardName;

            _columns= new List<Column>();
            foreach (ColumnDTO column in boardDTO.Columns)
                _columns.Add(new Column(column));

            _boardMembers=new HashSet<string>();
            foreach (UserBoardDTO user in boardDTO.BoardMembers.Values)
                _boardMembers.Add(user.UserEmail);
        }

        /// <summary>
        /// Creates a new Task with the given parameters, and inserts it into the first Column of the board.
        /// </summary>
        /// <param name="userEmail">The user's email address.</param>
        /// <param name="dueDate">The due date of the task.</param>
        /// <param name="title">The title of the task.</param>
        /// <param name="description">The description of the task.</param>
        /// <returns>The instance of the created Task, containing the generated TaskID.</returns>
        /// <exception cref="Exception">If the first column has already reached its tasks limit.</exception>
        public Task CreateTask(string userEmail, DateTime dueDate, string title, string description)
        {
            if(!IsBoardMember(userEmail))
            {
                log.Error("Error: Only a board member can create a task.");
                throw new ArgumentException("Error: Only a board member can create a task.");
            }

            Task t = new Task(dueDate, title, description, BoardID); //will check input validity
            try
            {
                _columns[BACKLOG_COLUMN].AddTask(t); // Could throw an exception if reached the column limit.
            }
            catch (Exception ex)
            {
                t.TaskDTO.Delete();
                throw ex;
            }
            return t;
        }

        /// <summary>
        /// Advances the task with the given task ID in the given Column number,
        /// to the next Column of the board, if possible.
        /// </summary>
        /// <param name="userEmail">The user's email address.</param>
        /// <param name="currentColumnNumber">The number of the column containing the task.</param>
        /// <param name="taskID">The unique ID of the task in the board.</param>
        /// <exception cref="ArgumentException">If the given column number is invalid,
        /// if the given column is the last column of the board, if the task doesn't exist in this column, 
        /// if the user trying to move the task is not the task assignee.</exception>
        /// <exception cref="Exception">If the next column has already reached its column limit.</exception>
        public void MoveTask(string userEmail, int currentColumnNumber, int taskID)
        {

            if(userEmail==null)
            {
                log.Error("Error: The user email is null.");
                throw new ArgumentNullException("Error: The user email is null.");
            }

            userEmail = userEmail.ToLower();

            if (!IsBoardMember(userEmail))
            {
                log.Error("Error: Only a board member can create a task.");
                throw new ArgumentException("Error: Only a board member can create a task.");
            }
            if (currentColumnNumber < BACKLOG_COLUMN || currentColumnNumber >= COUNT_COLUMNS)
            {
                log.Error("Error: Invalid column number: " + currentColumnNumber + ".");
                throw new ArgumentException("Error: Invalid column number: " + currentColumnNumber + ".");
            }
            if (currentColumnNumber == DONE_COLUMN)
            {
                log.Error("Error: Cannot move task from column \"Done\".");
                throw new ArgumentException("Error: Cannot move task from column \"Done\".");
            }
            if (!_columns[currentColumnNumber].TaskExists(taskID))
            {
                log.Error("Error: Given task ID: " + taskID + " doesn't exist in column: " + currentColumnNumber + ".");
                throw new ArgumentException("Error: Given task ID: " + taskID + " doesn't exist in column: " + currentColumnNumber + ".");
            }
            Task task = _columns[currentColumnNumber].GetTask(taskID);
            if (!task.IsAssignee(userEmail))
            {
                log.Error("Error: Only the assignee user can move a task.");
                throw new ArgumentException("Error: Only the assignee user can move a task.");
            }
            
            task.ColumnNumber++;
            try
            {
                _columns[currentColumnNumber + 1].AddTask(task); // Could throw exception if reached the column limit.
                _columns[currentColumnNumber].RemoveTask(taskID);
            }
            catch (Exception ex)
            {
                task.ColumnNumber--;
                throw ex;
            }

            log.Debug($"The task {taskID} was moved from column number {currentColumnNumber} to the nexu column.");
        }

        /// <summary>
        /// Returns the board column with the given column number.
        /// </summary>
        /// <param name="columnNumber">The number of the column.</param>
        /// <returns>The board column with the given column number.</returns>
        /// <exception cref="ArgumentException">If the given column number is invalid.</exception>
        public Column GetColumn(int columnNumber)
        {
            if (columnNumber < BACKLOG_COLUMN || columnNumber >= _columns.Count)
                throw new ArgumentException("Error: Invalid column number: " + columnNumber + ".");
            return _columns[columnNumber];
        }

        /// <summary>
        /// This method edits the details of the task with the specified taskID.
        /// </summary>
        /// <param name="userEmail">The user's email address.</param>
        /// <param name="taskID">The task's id.</param>
        /// <param name="dueDate">The new due date.</param>
        /// <param name="title">The new title.</param>
        /// <param name="description">The new description.</param>
        /// <exception cref="Exception">If the task was not found or trying to edit a task is the last column.</exception>
        /// <exception cref="ArgumentException">If a user who is not the assignee user tries to edit the task.</exception>
        public void EditTask(string userEmail, int taskID, DateTime dueDate, string title, string description)
        {
            if (userEmail == null)
            {
                log.Error("Error: The user email is null.");
                throw new ArgumentNullException("Error: The user email is null.");
            }

            Task task = null;
            for (int i = BACKLOG_COLUMN; i < Board.COUNT_COLUMNS; i++)
            {
                if (_columns[i].TaskExists(taskID))
                {
                    if (i == DONE_COLUMN)
                    {
                        log.Error("Error: A task in the last column can not be editted.");
                        throw new Exception("Error: A task in the last column can not be editted.");
                    }
                    task = _columns[i].GetTask(taskID);

                   
                    task.EditTask(userEmail, dueDate, title, description);
                    return;
                }

            }
            if (task == null)
            {
                log.Error("Error: The task was not found.");
                throw new Exception("Error: The task was not found.");
            }

        }
        /// <summary>
        /// This method returns the task with the specified taskID.
        /// </summary>
        /// <param name="taskID">The task's id.</param>
        /// <returns>The task with the specified taskID.</returns>
        /// <exception cref="Exception">If the task was not found.</exception>
        public Task GetTask(int taskID)
        {
            for (int i = BACKLOG_COLUMN; i < _columns.Count; i++)
            {
                if (_columns[i].TaskExists(taskID))
                {
                    return _columns[i].GetTask(taskID);
                }

            }
            throw new Exception($"Error: The task {taskID} was not found, in board {BoardName}");

        }

        /// <summary>
        /// This method returns the task with the specified id and null otherwise.
        /// </summary>
        /// <param name="taskID">The task's id.</param>
        /// <returns>The task with the specified id and null otherwise.</returns>
        private Task GetTaskOrNull(int taskID)
        {
            Task task;
            try
            {
                task = GetTask(taskID);
            }
            catch (Exception ex)
            {
                task = null;
            }

            return task;
        }

        /// <summary>
        /// This method adds a new member to the board.
        /// </summary>
        /// <param name="userEmail">The email of the new board member.</param>
        /// <exception cref="ArgumentNullException">If the user email is null.</exception>
        /// <exception cref="ArgumentException">If the user is already a board member.</exception>
        public void AddMember(string userEmail)
        {
            if (userEmail == null)
            {
                log.Error("Error: Invalid user email: null.");
                throw new ArgumentNullException("Error: Invalid user email: null.");
            }

            userEmail=userEmail.ToLower();

            if (IsBoardMember(userEmail))
            {
                log.Error("Error: This user is already a board member.");
                throw new ArgumentException("Error: This user is already a board member.");
            }

            BoardDTO.InsertMember(userEmail);

            _boardMembers.Add(userEmail);

            log.Debug($"The user {userEmail} was added as a board memebr to board {BoardName}.");
        }

        /// <summary>
        /// This method removes a member from the board.
        /// </summary>
        /// <param name="userEmail">The email of the board member we want to remove.</param>
        /// <exception cref="ArgumentNullException">If the user email is null.</exception>
        /// <exception cref="ArgumentException">If the user is the board owner or not a board member.</exception>
        public void RemoveMember(string userEmail)
        {
            if (userEmail == null)
            {
                log.Error("Error: Invalid user email: null.");
                throw new ArgumentNullException("Error: Invalid user email: null.");
            }

            userEmail = userEmail.ToLower();

            if (userEmail.Equals(BoardOwner))
            {
                log.Error("Error: The board owner can not leave the board.");
                throw new ArgumentException("Error: The board owner can not leave the board.");
            }
            if (!IsBoardMember(userEmail))
            {
                log.Error("Error: The user is not a board member.");
                throw new ArgumentException("Error: The user is not a board member.");
            }

            BoardDTO.RemoveMember(userEmail);
            _boardMembers.Remove(userEmail);

            // All of the user's assigned tasks that are not done, become unassigned.
            for (int i = BACKLOG_COLUMN; i < DONE_COLUMN; i++)
                _columns[i].UnassignTasks(userEmail);


            log.Debug($"The user {userEmail} was removed as a board memebr to board {BoardName}.");
        }

        /// <summary>
        /// This method transfers the board ownership to another board member.
        /// </summary>
        /// <param name="oldOwner">The email of the old board owner.</param>
        /// <param name="newOwner">The email of the new board owner.</param>
        /// <exception cref="ArgumentNullException">If the user email is null.</exception>
        /// <exception cref="ArgumentException">If the old user is not the board owner or the new ownner is not a board member.</exception>
        public void TransferOwner(string oldOwner, string newOwner)
        {
            if (oldOwner == null | newOwner == null)
            {
                log.Error("Error: Invalid user email: null.");
                throw new ArgumentNullException("Error: Invalid user email: null.");
            }

            oldOwner = oldOwner.ToLower();
            newOwner = newOwner.ToLower();

            if (oldOwner != BoardOwner)
            {
                log.Error("Error: The old user is not the board owner.");
                throw new ArgumentException("Error: The old user is not the board owner.");
            }
            if (!IsBoardMember(newOwner))
            {
                log.Error("Error: The new user is not a board member.");
                throw new ArgumentException("Error: The new user is not a board member.");
            }

            this.BoardOwner = newOwner;

            log.Debug($"The ownership of the board {BoardName} was transfered from {oldOwner} to {newOwner}.");
        }

        /// <summary>
        /// This method assigns a task to a board member.
        /// </summary>
        /// <param name="taskID">The task's id.</param>
        /// <param name="userEmail">The email address of the user assigning the task.</param>
        /// <param name="newAssignee">The email address of the user the task is assigned to.</param>
        /// <exception cref="ArgumentNullException">If one of the user's email is null.</exception>
        public void AssignTask(int taskID, string userEmail, string newAssignee)
        {
            if (userEmail == null || newAssignee == null)
            {
                log.Error("Error: Invalid user email: null.");
                throw new ArgumentNullException("Error: Invalid user email: null.");
            }

            userEmail = userEmail.ToLower();
            newAssignee = newAssignee.ToLower();

            Task task = GetTaskOrNull(taskID); //checks if task exists
            if(task==null)
            {
                log.Error("Error: The task does not exist.");
                throw new ArgumentNullException("Error: The task does not exist.");
            }
            if (!IsBoardMember(userEmail))
            {
                log.Error("Error: User must be a board member to assign tasks.");
                throw new ArgumentNullException("Error: User must be a board member to assign tasks.");
            }
            if (!IsBoardMember(newAssignee))
            {
                log.Error("Error: Must assign task to a board member.");
                throw new ArgumentNullException("Error: Must assign task to a board member.");
            }

            task.ChangeAssignee(userEmail, newAssignee);

            log.Debug($"The task {taskID} was assigned to {newAssignee} by {userEmail}.");
        }

        /// <summary>
        /// This methods checks if the user is a board member.
        /// </summary>
        /// <param name="userEmail">The user's email.</param>
        /// <returns>True if the user is a board member and false otherwise.</returns>
        public bool IsBoardMember(string userEmail)
        {
            if(userEmail!=null)
                userEmail = userEmail.ToLower();
            return _boardMembers.Contains(userEmail);
        }
    }
}
