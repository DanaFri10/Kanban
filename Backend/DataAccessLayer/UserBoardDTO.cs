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
    /// UserBoardDTO class represents a record of the UsersBoards table in the database.
    /// </summary>
    public class UserBoardDTO : DTO
    {
        public int BoardID { get; }
        public string UserEmail { get; }

        private static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>
        /// Constructor for initializing a new instance of the UserBoardDTO object, that gets the board id and user email.
        /// </summary>
        /// <param name="boardId">The board Id.</param>
        /// <param name="userEmail">The user's email.</param>
        public UserBoardDTO(int boardId, string userEmail) : base(new UserBoardDalController())
        {
            BoardID = boardId;
            UserEmail = userEmail;
            log.Debug($"Created new UserBoard with the following parameters: boardId={boardId}, userEmail={userEmail}.");

        }

        /// <summary>
        /// This method deletes the UserBoardDTO from the DB.
        /// </summary>
        public override void Delete()
        {
            _dalController.Delete(new string[] { BoardID.ToString(),UserEmail });
            log.Debug($"Deleted the UserBoardDTO with email {UserEmail} from DB.");
        }

        /// <summary>
        /// This method inserts the UserBoardDTO to to DB.
        /// </summary>
        public override void Insert()
        {
            _dalController.Insert(new string[] { "BoardID", "UserEmail" }, new string[] { BoardID.ToString(), UserEmail });
            log.Debug($"Inserted the UserBoardDTO with email {UserEmail} to the DB.");
        }
    }
}
