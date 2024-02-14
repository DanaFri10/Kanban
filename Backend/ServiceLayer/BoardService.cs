using IntroSE.Kanban.Backend.BusinessLayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;
using log4net;

namespace IntroSE.Kanban.Backend.ServiceLayer
{
    /// <summary>
    /// Class BoardService allows the Presentation Layer to interact with
    /// the Board system of the application.
    /// It supports operations such as creating, deleting and getting user Boards,
    /// creating, editing, getting and moving Tasks between different board Columns, and more.
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
    public class BoardService
    {
        private BoardController _boardController;
        private UserController _userController;

        private static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        internal BoardService(BoardController boardController, UserController userController) 
        {
            this._boardController = boardController;
            this._userController = userController;
            log.Info("Initialized BoardService.");
        }

        /// <summary>
        /// This method creates a new board to the user with the specified email address.
        /// </summary>
        /// <param name="userEmail">The board creator's email address.</param>
        /// <param name="boardName">The name of the new board we want to create.</param>
        /// <returns>A Json representation of a response containing an error message, if occured.</returns>
        public string CreateBoard(string userEmail, string boardName)
        {
            try
            {
                if (!_userController.IsLoggedIn(userEmail)){
                    string errorMessage = "Error: User must be logged in to create boards.";
                    log.Error(errorMessage);
                    return new Response<Board>(errorMessage).ToJson();
                }

                BusinessLayer.Board b = _boardController.CreateBoard(userEmail, boardName);
                Board boardSer = new Board(b);
                
                log.Debug($"User {userEmail} has created the board {boardName}.");
                return new Response<Board>(boardSer).ToJson();
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                return new Response<Board>(ex.Message).ToJson();
            }

        }

        /// <summary>
        /// This method deletes the board with the specified board name,
        /// from the user with the specified email address. 
        /// </summary>
        /// <param name="userEmail">The board owner's email address.</param>
        /// <param name="boardName">The name of the board we want to remove.</param>
        /// <returns>A Json representation of a response containing an error message, if occured.</returns>
        public string RemoveBoard(string userEmail, string boardName)
        {
            try
            {
                if (!_userController.IsLoggedIn(userEmail))
                {
                    string errorMessage = "Error: User must be logged in to remove boards.";
                    log.Error(errorMessage);
                    return new Response<Board>(errorMessage).ToJson();
                }

                _boardController.RemoveBoard(userEmail, boardName);
                log.Debug($"User {userEmail} has removed the board {boardName}.");
                return new Response<Board>().ToJson();
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                return new Response<Board>(ex.Message).ToJson();
            }
        }

        /// <summary>
        /// This method returns a board object representing the specified user's board with the given name.
        /// The returned object will be serialized as a Json string value, inside the response json.
        /// </summary>
        /// <param name="userEmail">The board member's email address.</param>
        /// <param name="boardName">The name of the board we want to return.</param>
        /// <returns>
        /// A Json representation of a response containing an error message, if occured,
        /// and a Json representation of the board object as a return value, if it exists in the system.
        /// The Board Json is in the following form:
        /// {
        ///    "BoardName": string,
        ///    "Columns": List of Column Json objects
        /// }
        /// The Column Json is in the following form:
        /// {
        ///     "Tasks": Dictionary of Task Json objects,
        ///     "ColumnNumber": int,
        ///     "TasksLimit": int
        /// }
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
        public string GetBoard(string userEmail, string boardName)
        {
            try
            {
                if (!_userController.IsLoggedIn(userEmail))
                {
                    string errorMessage = "Error: User must be logged in to get boards.";
                    log.Error(errorMessage);
                    return new Response<Board>(errorMessage).ToJson();
                }

                BusinessLayer.Board b = _boardController.GetBoard(userEmail, boardName);
                Board boardSer = new Board(b);
                return new Response<Board>(boardSer).ToJson();
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                return new Response<Board>(ex.Message).ToJson();
            }
        }

        /// <summary>
        /// This method returns a board object representing the specified user's board with the given id.
        /// The returned object will be serialized as a Json string value, inside the response json.
        /// </summary>
        /// <param name="userEmail">The board member's email address.</param>
        /// <param name="boardID">The id of the board we want to return.</param>
        /// <returns>
        /// A Json representation of a response containing an error message, if occured,
        /// and a Json representation of the board object as a return value, if it exists in the system.
        public string GetBoard(string userEmail, int boardID)
        {
            try
            {
                if (!_userController.IsLoggedIn(userEmail))
                {
                    string errorMessage = "Error: User must be logged in to get boards.";
                    log.Error(errorMessage);
                    return new Response<Board>(errorMessage).ToJson();
                }

                BusinessLayer.Board b = _boardController.GetBoard(userEmail, boardID);
                Board boardSer = new Board(b);
                return new Response<Board>(boardSer).ToJson();
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                return new Response<Board>(ex.Message).ToJson();
            }
        }

        /// <summary>
        /// This method returns a board object representing the specified board with the given id.
        /// The returned object will be serialized as a Json string value, inside the response json.
        /// </summary>
        /// <param name="boardID">The id of the board we want to return.</param>
        /// <returns>
        /// A Json representation of a response containing an error message, if occured,
        /// and a Json representation of the board object as a return value, if it exists in the system.
        public string GetBoard(int boardID)
        {
            try
            {
                BusinessLayer.Board b = _boardController.GetBoard(boardID);
                Board boardSer = new Board(b);
                return new Response<Board>(boardSer).ToJson();
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                return new Response<Board>(ex.Message).ToJson();
            }
        }

        /// <summary>
        /// This method sets a new limit to maximum number of Tasks in the specified Column of the board.
        /// </summary>
        /// <param name="userEmail">The board member's email address.</param>
        /// <param name="boardName">The name of the board we want to limit.</param>
        /// <param name="columnNumber">The number of the column in the board.</param>
        /// <param name="tasksLimit">The limit of the tasks in the column.</param>
        /// <returns>A Json representation of a response containing an error message, if occured.</returns>
        public string LimitColumn(string userEmail, string boardName, int columnNumber, int tasksLimit)
        {
            try
            {
                if (!_userController.IsLoggedIn(userEmail))
                {
                    string errorMessage = "Error: User must be logged in to limit board columns.";
                    log.Error(errorMessage);
                    return new Response<Board>(errorMessage).ToJson();
                }

                _boardController.LimitColumn(userEmail, boardName, columnNumber, tasksLimit);
                log.Debug($"User {userEmail} has limited column {columnNumber} in board {boardName} to a limit of {tasksLimit} tasks.");
                return new Response<Board>().ToJson();
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                return new Response<Board>(ex.Message).ToJson();
            }
        }

        /// <summary>
        /// This method adds the user into the board with the specified board id.
        /// </summary>
        /// <param name="userEmail">The email address of the user who requested to join the board.</param>
        /// <param name="boardID">The id of the board the user requested to join.</param>
        /// <returns>A Json representation of a response containing an error message, if occured,
        /// and a Json representation of the board object as a return value.</returns>
        public string JoinBoard(string userEmail, int boardID)
        {
            try
            {
                if (!_userController.IsLoggedIn(userEmail))
                {
                    string errorMessage = "Error: User must be logged in to join a board.";
                    log.Error(errorMessage);
                    return new Response<Board>(errorMessage).ToJson();
                }

                Board board = new Board(_boardController.JoinBoard(userEmail, boardID));
                log.Debug($"User {userEmail} has joined board {boardID}.");
                return new Response<Board>(board).ToJson();
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                return new Response<Board>(ex.Message).ToJson();
            }
        }

        /// <summary>
        /// This method removes the user from the board with the specified board name.
        /// </summary>
        /// <param name="userEmail">The email address of the board member who requested to leave.</param>
        /// <param name="boardName">The name of the board the user requested to leave.</param>
        /// <returns>A Json representation of a response containing an error message, if occured.</returns>
        public string LeaveBoard(string userEmail, string boardName)
        {
            try
            {
                if (!_userController.IsLoggedIn(userEmail))
                {
                    string errorMessage = "Error: User must be logged in to leave a board.";
                    log.Error(errorMessage);
                    return new Response<Board>(errorMessage).ToJson();
                }

                _boardController.LeaveBoard(userEmail, boardName);
                log.Debug($"User {userEmail} has left board {boardName}.");
                return new Response<Board>().ToJson();
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                return new Response<Board>(ex.Message).ToJson();
            }
        }

        /// <summary>
        /// This method transfers the ownership of a board to another board member.
        /// </summary>
        /// <param name="userEmail">The email address of the current board owner, who requested to transfer the ownership.</param>
        /// <param name="boardName">The name of the board the user requested to transfer its ownership.</param>
        /// <param name="newOwner">The email address of the new board owner.</param>
        /// <returns>A Json representation of a response containing an error message, if occured.</returns>
        public string TransferOwner(string userEmail, string boardName, string newOwner)
        {
            try
            {
                if (!_userController.IsLoggedIn(userEmail))
                {
                    string errorMessage = "Error: User must be logged in to transfer board ownership.";
                    log.Error(errorMessage);
                    return new Response<Board>(errorMessage).ToJson();
                }

                if(!_userController.UserExists(newOwner))
                {
                    string errorMessage = "Error: User must exist in order to be the new board owner.";
                    log.Error(errorMessage);
                    return new Response<Board>(errorMessage).ToJson();
                }

                _boardController.TransferOwner(userEmail, boardName, newOwner);
                log.Debug($"User {userEmail} has transfered the ownership of board {boardName} to user {newOwner}.");
                return new Response<Board>().ToJson();
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                return new Response<Board>(ex.Message).ToJson();
            }
        }

        /// <summary>
        /// This function returns all the boards of the specified user.
        /// </summary>
        /// <param name="userEmail">The user's email address.</param>
        /// <returns>A Json representation of a response containing an error message, if occured</returns>
        public string GetUserBoards(string userEmail)
        {
            try
            {
                if (!_userController.IsLoggedIn(userEmail))
                {
                    string errorMessage = "Error: The user has to be logged in to get his boards.";
                    log.Error(errorMessage);
                    return new Response<Board>(errorMessage).ToJson();
                }

                List<Board> userBoards = new List<Board>();
                foreach(BusinessLayer.Board board in _boardController.GetUserBoards(userEmail))
                {
                    userBoards.Add(new Board(board));
                }
                return new Response<List<Board>>(userBoards).ToJson();
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                return new Response<Board>(ex.Message).ToJson();
            }
        }
    }
}
