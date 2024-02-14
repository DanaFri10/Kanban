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
    /// UserController saves all users in a dictionary.
    /// </summary>
    public class UserController
    {
        private Dictionary<string, User> _users;
        private UserMapper _mapper;

        private static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>
        /// Initializes a new instance of the UserController class.
        /// </summary>
        public UserController()
        {
            _users = new Dictionary<string, User>();
            _mapper = new UserMapper();
            log.Info("Initialized UserController.");
        }

        /// <summary>
        /// This method loads the User data from the database in the initial load of the system.
        /// </summary>
        public void LoadData()
        {
            foreach (UserDTO userDTO in _mapper.LoadAllUsers())
            {
                _users[userDTO.EmailAddress.ToLower()] = new User(userDTO);
            }
            log.Debug("Loaded User data from database into UserController.");
        }

        /// <summary>
        /// This method deletes all the User data from the system and from the database.
        /// </summary>
        public void DeleteData()
        {
            _mapper.DeleteAllUsers();
            _users.Clear();
            log.Debug("Deleted all User data from UserController and from the database.");
        }

        /// <summary>
        /// Creating a user and saving it in the dictionary.
        /// </summary>
        /// <param name="email">The user's email address.</param>
        /// <param name="password">The user's password.</param>
        /// <exception cref="InvalidOperationException">If the user already exists.</exception>
        public void CreateUser(string email, string password)
        {
            if (UserExists(email))
            {
                log.Error("Error: This user already exists.");
                throw new InvalidOperationException("Error: This user already exists.");
            }
            User newUser = new User(email, password); // Will throw an exception if email or password are not valid.
            _users[email.ToLower()] = newUser;
            log.Debug($"User {email} has been created.");
        }

        /// <summary>
        /// This method logs in the user with the specified email address,
        /// if the user exists and the password is correct.
        /// </summary>
        /// <param name="email">The user email address, used as the username for logging in to the system.</param>
        /// <param name="password">The user password.</param>
        /// <returns>The logged in User object, unless an error has occured.</returns>
        /// <exception cref="Exception">If the user doesn't exist or if the given password was incorrect.</exception>
        public User Login(string email, string password)
        {
            User user = GetUser(email); // Will throw an exception if the user doesn't exist.
            bool result = user.Login(password);
            if (!result)
                throw new Exception("Error: Could not log in, given password was incorrect.");
            log.Debug($"User {email} has been logged in.");
            return user;
        }

        /// <summary>
        /// This method logs out the user with the specified email address, if exists.
        /// </summary>
        /// <param name="email">The user email address.</param>
        /// <exception cref="Exception">If the user doesn't exist.</exception>
        public void Logout(string email)
        {
            GetUser(email).Logout(); // Will throw an exception if the user doesn't exist.
            log.Debug($"User {email} has been logged out.");
        }

        /// <summary>
        /// This method changes the password of the user with the specified email address,
        /// if the old password is correct.
        /// </summary>
        /// <param name="email">The user email address.</param>
        /// <param name="oldPassword">The user's old password.</param>
        /// <param name="newPassword">The user's new password, which needs to be updated.</param>
        /// <exception cref="Exception">If the user doesn't exist.</exception>
        public void ChangePassword(string email, string oldPassword, string newPassword)
        {
            GetUser(email).ChangePassword(oldPassword, newPassword); // Will throw an exception if the user doesn't exist.
            log.Debug($"User {email} has changed the password.");
        }

        /// <summary>
        /// Returns if a user exists in the system.
        /// </summary>
        /// <param name="email">The email of the user we want to check if exists.</param>
        /// <returns>True if the user exists and false otherwise.</returns>
        public bool UserExists(string email)
        {
            if (email == null)
                throw new ArgumentNullException("Error: Invalid user email: null.");
            return _users.ContainsKey(email.ToLower());
        }

        /// <summary>
        /// This method returns a user by it's email address.
        /// </summary>
        /// <param name="email">The email of the user we want to return.</param>
        /// <returns>The user with the given email address, if exists.</returns>
        /// <exception cref="InvalidOperationException">If the user does not exist.</exception>
        public User GetUser(string email)
        {
            if (!UserExists(email))
                throw new InvalidOperationException("Error: This user does not exist.");
            return _users[email.ToLower()];
        }

        /// <summary>
        /// Checks if the user is logged in.
        /// </summary>
        /// <param name="email">The email of the user we want to check if logged in.</param>
        /// <returns>True if the user is logged in and false otherwise.</returns>
        public bool IsLoggedIn(string email)
        {
            User user = GetUser(email); // Will throw an exception if the user doesn't exist.
            return user.IsLoggedIn;
        }
    }
}
