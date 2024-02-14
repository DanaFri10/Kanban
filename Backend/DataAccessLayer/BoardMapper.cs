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
    /// BoardMapper class allows for loading and deleting all the board data from the database.
    /// </summary>
    public class BoardMapper
    {
        private BoardDalController _dalController;
        private static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>
        /// Initializes a new instance of the BoardMapper class.
        /// </summary>
        public BoardMapper()
        {
            _dalController = new BoardDalController();
        }

        /// <summary>
        /// This method loads all Board data from the database when the project starts.
        /// </summary>
        /// <returns>
        /// A List of BoardDTOs containing all the Board data from the database,
        /// including Board members, Columns and Tasks.
        /// </returns>
        public List<BoardDTO> LoadAllBoards()
        {
            List<DTO> DTOs = _dalController.Select();
            List<BoardDTO> boardsDTOs = new List<BoardDTO>();
            foreach (DTO dto in DTOs) //convert DTO to BoardDTO
            {
                boardsDTOs.Add((BoardDTO)dto);
            }
            log.Debug($"Loaded all boards from DB.");
            return boardsDTOs;
        }

        /// <summary>
        /// This method deletes all the Board data from the database, including Board members, Columns and Tasks.
        /// </summary>
        public void DeleteAllBoards()
        {
            new TaskDalController().DeleteAll();
            new ColumnDalController().DeleteAll();
            new UserBoardDalController().DeleteAll();
            _dalController.DeleteAll();
            _dalController.DeleteSqliteSequence();
            log.Debug($"Deleted all boards from DB.");
        }
    }
}
