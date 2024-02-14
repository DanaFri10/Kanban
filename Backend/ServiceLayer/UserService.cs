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
    /// Class UserService allows the Presentation Layer to interact with
    /// the User system of the application.
    /// It supports operations such as creating, deleting and getting users,
    /// logging in and out of the system, changing the user's password.
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
    public class UserService
    {
        private UserController _userController;

        private static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>
        /// A user service constructor.
        /// </summary>
        public UserService(UserController uc)
        {
            _userController = uc;
            log.Info("Initialized UserService.");
        }

        /// <summary>
        /// This method creates a new user to the system, if the email and password are valid.
        /// </summary>
        /// <param name="email">The user email address, used as the username for logging in to the system.</param>
        /// <param name="password">The user password.</param>
        /// <returns>Returns a Json representation of a response containing an error message, if occured.</returns>
        public string CreateUser(string email, string password)
        {
            try
            {
                _userController.CreateUser(email, password);
                return new Response<User>().ToJson();
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                return new Response<User>(ex.Message).ToJson();
            }
        }

        /// <summary>
        /// This method logs in the user with the specified email address,
        /// if the user exists and the password is correct.
        /// </summary>
        /// <param name="email">The user email address, used as the username for logging in to the system.</param>
        /// <param name="password">The user password.</param>
        /// <returns>
        /// Returns a Json representation of a response containing an error message, if occured. 
        /// and a Json representation of a User object as a return value.
        /// The User Json is in the following form:
        /// {
        ///     "EmailAddress": string,
        ///     "IsLoggedIn": bool
        /// }
        /// </returns>
        public string Login(string email, string password)
        {
            try
            {
                User user = new User(_userController.Login(email, password));
                return new Response<User>(user).ToJson();
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                return new Response<User>(ex.Message).ToJson();
            }
        }

        /// <summary>
        /// This method logs out the user with the specified email address, if exists.
        /// </summary>
        /// <param name="email">The user email address.</param>
        /// <returns>Returns a Json representation of a response containing an error message, if occured.</returns>
        public string Logout(string email)
        {
            try
            {
                _userController.Logout(email);
                return new Response<User>().ToJson();
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                return new Response<User>(ex.Message).ToJson();
            }
        }

        /// <summary>
        /// This method changes the password of the user with the specified email address,
        /// if the old password is correct.
        /// </summary>
        /// <param name="email">The user email address.</param>
        /// <param name="oldPassword">The user's old password.</param>
        /// <param name="newPassword">The user's new password, which needs to be updated.</param>
        /// <returns>A Json representation of a response containing an error message, if occured.</returns>
        public string ChangePassword(string email, string oldPassword, string newPassword)
        {
            try
            {
                _userController.ChangePassword(email, oldPassword, newPassword);
                return new Response<User>().ToJson();
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                return new Response<User>(ex.Message).ToJson();
            }
        }

        /// <summary>
        /// This method checks if there exists a user with the specified email address.
        /// </summary>
        /// <param name="email">The user's email address.</param>
        /// <returns>A Json representation of a response containing an error message, if occured,
        /// and a boolean return value, 'true' if the user exists and 'false' otherwise.</returns>
        public string UserExists(string email)
        {
            try
            {
                bool result = _userController.UserExists(email);
                return new Response<bool>(result).ToJson();
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                return new Response<bool>(ex.Message).ToJson();
            }
        }

        /// <summary>
        /// This method returns a user object correlated to the specified user email, if exists.
        /// The returned object will be serialized as a Json string value, inside the response json.
        /// </summary>
        /// <param name="email">The user's email address.</param>
        /// <returns>
        /// A Json representation of a response containing an error message, if occured,
        /// and a Json representation of a User object as a return value, if it exists in the system.
        /// The User Json is in the following form:
        /// {
        ///     "EmailAddress": string,
        ///     "IsLoggedIn": bool
        /// }
        /// as explained in the class diagram.
        /// </returns>
        public string GetUser(string email)
        {
            try
            {
                User result = new User(_userController.GetUser(email)); // will throw an exception if the user does not exist.
                return new Response<User>(result).ToJson();
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                return new Response<User>(ex.Message).ToJson();
            }
        }
    }
}

