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
    /// UserMapper class allows for loading and deleting all the user data from the database.
    /// </summary>
    public class UserMapper
    {
        private UserDalController _dalController;
        private static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>
        /// Initializes a new instance of the UserMapper class.
        /// </summary>
        public UserMapper()
        {
            _dalController = new UserDalController();
        }


        /// <summary>
        /// This method loads all User data from the database when the project starts.
        /// </summary>
        /// <returns>A List of UserDTOs containing all the User data from the database.</returns>
        public List<UserDTO> LoadAllUsers()
        {
            List<DTO> DTOs = _dalController.Select();
            List<UserDTO> userDTOs = new List<UserDTO>();
            foreach(DTO dto in DTOs) //convert DTO to UserDTO
            {
                userDTOs.Add((UserDTO)dto);
            }
            log.Debug($"Loaded all users from DB.");
            return userDTOs;
        }


        /// <summary>
        /// This method deletes all the User data from the database.
        /// </summary>
        public void DeleteAllUsers()
        {
            _dalController.DeleteAll();
            log.Debug($"Deleted all users from DB.");
        }
    }
}
