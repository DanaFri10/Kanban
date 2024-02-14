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
    /// BoardDTO class represents a record of the Boards table in the database.
    /// </summary>
    public class BoardDTO : DTO
    {

        public static readonly int COUNT_COLUMNS = 3;
        public int BoardID { get; private set; }

        private string _boardName;
        public string BoardName {
            get => _boardName; 
            set
            {
                _dalController.Update(new string[] { BoardID.ToString() }, "BoardName", value);
                _boardName = value;
            }
        }

        private string _boardOwner;
        public string BoardOwner {
            get => _boardOwner; 
            set
            {
                _dalController.Update(new string[] { BoardID.ToString() }, "BoardOwner", value);
                _boardOwner = value;            
            }
        }
        public List<ColumnDTO> Columns { get; }
        public Dictionary<string, UserBoardDTO> BoardMembers { get; set; }

        private static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>
        /// Constructor for loading data from DB, that gets the board id, board name, board owner.
        /// </summary>
        /// <param name="boardId">The board id.</param>
        /// <param name="boardName">The board name.</param>
        /// <param name="boardOwner">The board owner.</param>
        public BoardDTO(int boardId, string boardName, string boardOwner) : base(new BoardDalController())
        {
            BoardID = boardId;
            _boardName = boardName;
            _boardOwner = boardOwner; 
            
            // Load all columns from database
            ColumnDalController columnDalController = new ColumnDalController();
            List<DTO> DTOs = columnDalController.Select(new string[] { "BoardID" }, new string[] { BoardID.ToString() });
            Columns = new List<ColumnDTO>();
            foreach (DTO dto in DTOs)
            {
                Columns.Add((ColumnDTO)dto);
            }
            
            // Load all board members from database
            UserBoardDalController userBoardDalController = new UserBoardDalController();
            DTOs = userBoardDalController.Select(new string[] {"BoardID"}, new string[] {BoardID.ToString()});
            BoardMembers = new Dictionary<string, UserBoardDTO>();
            foreach (DTO dto in DTOs)
            {
                UserBoardDTO userBoard = (UserBoardDTO)dto;
                BoardMembers.Add(userBoard.UserEmail, userBoard);
            }

            log.Debug($"Created new BoardDTO with the following parameters: boardId={boardId}, boardName={boardName}, boardOwner={boardOwner}.");
        }

        /// <summary>
        /// Constructor for initializing a new instance of the BoardDTO class,
        /// according to the given board name and board owner.
        /// </summary>
        /// <param name="boardName">The board name.</param>
        /// <param name="boardOwner">The board owner.</param>
        public BoardDTO(string boardName, string boardOwner) : base(new BoardDalController())
        {
            BoardID = -1;
            _boardName = boardName;
            _boardOwner = boardOwner;
            Columns = new List<ColumnDTO>();
            BoardMembers = new Dictionary<string, UserBoardDTO>();
            log.Debug($"Created new BoardDTO with the following parameters: boardName={boardName}, boardOwner={boardOwner}.");
        }

        /// <summary>
        /// This method deletes the boardDTO from the DB.
        /// </summary>
        public override void Delete()
        {
            _dalController.Delete(new string[] { BoardID.ToString() });
            foreach(UserBoardDTO userBoardDTO in BoardMembers.Values)
            {
                userBoardDTO.Delete();
            }
            foreach (ColumnDTO columnDTO in Columns)
            {
                columnDTO.Delete();
            }

            log.Debug($"Deleted the boardDTO with id {BoardID} from DB.");
        }

        /// <summary>
        /// This method inserts the boardDTO to the DB.
        /// </summary>
        public override void Insert()
        {
            _dalController.Insert(new string[] { "BoardName" , "BoardOwner" }, new string[] { BoardName , BoardOwner });
            BoardID = _dalController.GetMaxValue("BoardID");
            log.Debug($"Inserted the boardDTO with id {BoardID} to the DB.");
        }

        /// <summary>
        /// This method inserts the user as a board member to the DB.
        /// </summary>
        /// <param name="userEmail">The user's email.</param>
        /// <exception cref="ArgumentException">If the user is already a board member.</exception>
        public void InsertMember(string userEmail)
        {
            if (BoardMembers.ContainsKey(userEmail))
            {
                log.Error("Error: This user is already a board member.");
                throw new ArgumentException("Error: This user is already a board member.");
            }
            UserBoardDTO userBoard = new UserBoardDTO(BoardID, userEmail);
            userBoard.Insert();
            BoardMembers.Add(userEmail, userBoard);
            log.Debug($"Inserted user {userEmail} as a board member to board {BoardID}.");
        }

        /// <summary>
        /// This method removes the user with the given email from the DB.
        /// </summary>
        /// <param name="userEmail">The user's email.</param>
        /// <exception cref="ArgumentException">If the user is not a board member.</exception>
        public void RemoveMember(string userEmail)
        {
            UserBoardDTO userBoard = BoardMembers.GetValueOrDefault(userEmail, null);
            if (userBoard == null)
            {
                log.Error("Error: This user is not a board member.");
                throw new ArgumentException("Error: This user is not a board member.");
            }
            userBoard.Delete();
            BoardMembers.Remove(userEmail);
            log.Debug($"Removed user {userEmail} as a board member from board {BoardID}.");
        }
    }
}
