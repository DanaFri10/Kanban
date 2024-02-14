using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Mail;

using IntroSE.Kanban.Backend.DataAccessLayer;
using log4net;
using System.Reflection;

namespace IntroSE.Kanban.Backend.BusinessLayer
{
    /// <summary>
    /// User class represents a user in the Kanban system.
    /// </summary>
    public class User
    {
        public string EmailAddress { get; private set; }
        public bool IsLoggedIn { get; private set; }
        private string _password;
        private string Password { get => _password; set { UserDTO.Password = value; _password = value; } }
        public UserDTO UserDTO { get; }

        private static readonly int MIN_PASSWORD_LENGTH = 6;
        private static readonly int MAX_PASSWORD_LENGTH = 20;

        private static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>
        /// Initializes a new instance of the User class, with the given email address and password.
        /// </summary>
        /// <param name="emailAddress">The user's email address.</param>
        /// <param name="password">The user's password.</param>
        /// <exception cref="InvalidOperationException">If the given email address or password is not valid.</exception>
        public User(string emailAddress, string password)
        {
            if (emailAddress == null)
            {
                log.Error("Error: The email address is null.");
                throw new ArgumentNullException("Error: The email address is null.");
            }
            if (!CheckEmailAddress(emailAddress))
            {
                log.Error("Error: The email address is invalid.");
                throw new InvalidOperationException("Error: The email address is invalid.");
            }
            if (!CheckValidPassword(password))
            {
                log.Error("Error: The password is invalid.");
                throw new InvalidOperationException("Error: The password is invalid.");
            }

            EmailAddress = emailAddress;
            _password = password;
            IsLoggedIn = true; // Updated to true, need to update about 7 tests that are now failing as a result.
            UserDTO = new UserDTO(EmailAddress, password);
            UserDTO.Insert();

            log.Debug($"A new user was created with the following email address: {EmailAddress}.");
        }

        /// <summary>
        /// Initializes a new instance of the User class, with the given boaruserDTOdDTO.
        /// </summary>
        /// <param name="userDTO">The userDTO.</param>
        /// <exception cref="ArgumentNullException">If the userDTO is null.</exception>
        public User(UserDTO userDTO)
        {
            if (userDTO == null)
            {
                log.Error("Error: The userDTO is null.");
                throw new ArgumentNullException("Error: The userDTO is null.");
            }

            UserDTO = userDTO;
            EmailAddress = userDTO.EmailAddress;
            _password = userDTO.Password;
            IsLoggedIn = false;

            log.Debug($"A new user was created with the following email address: {EmailAddress}.");
        }

        /// <summary>
        /// Logging a user into the system.
        /// </summary>
        /// <param name="password">The password the user put as input when trying to log in.</param>
        /// <returns>True if login succeeded, and false if the given password was incorrect, unless an error has occured.</returns>
        /// <exception cref="InvalidOperationException">If the user is already logged in.</exception>
        public bool Login(string password) 
        {
            if (IsLoggedIn)
            {
                log.Error("Error: This user is already logged in.");
                throw new InvalidOperationException("Error: This user is already logged in.");
            }
            if (_password.Equals(password))
                IsLoggedIn = true;

            log.Debug($"The user {EmailAddress} tried to log in.");

            return IsLoggedIn;
        }

        /// <summary>
        /// Logging a user out of the system.
        /// </summary>
        /// <exception cref="InvalidOperationException">If the user is already logged out.</exception>
        public void Logout()
        {
            if (!IsLoggedIn)
            {
                log.Error("Error: This user is already logged out.");
                throw new InvalidOperationException("Error: This user is already logged out.");
            }
            IsLoggedIn = false;
            log.Debug($"The user {EmailAddress} logged out.");
        }

        /// <summary>
        /// This method changes a user's password, if the old password was correct
        /// </summary>
        /// <param name="oldPassword">The user's old password.</param>
        /// <param name="newPassword">The password the user wants to change to.</param>
        /// <exception cref="InvalidOperationException">If the user is not logged in, the old password is incorrect or the new password is not valid.</exception>
        public void ChangePassword(string oldPassword, string newPassword)
        {
            if (!IsLoggedIn)
            {
                log.Error("Error: A user that is not logged in can not change password.");
                throw new InvalidOperationException("Error: A user that is not logged in can not change password.");
            }
            if (!_password.Equals(oldPassword))
            {
                log.Error("Error: Old password is incorrect.");
                throw new InvalidOperationException("Error: Old password is incorrect.");
            }
            if (!CheckValidPassword(newPassword))
            {
                log.Error("Error: New password is invalid, a valid password contains 6-20 characters, at least one digit, one uppercase letter and one lowercase letter.");
                throw new InvalidOperationException("Error: New password is invalid, a valid password contains 6-20 characters, at least one digit, one uppercase letter and one lowercase letter.");
            }
            
            Password = newPassword;
            log.Debug($"The user {EmailAddress} changed his password.");
        }

        /// <summary>
        /// Checks if a password is valid - has 6-20 characters and contains at least one digit, uppercase letter and lowercase letter
        /// </summary>
        /// <param name="password">The password we want to check.</param>
        /// <returns>True if the password is valid and false otherwise.</returns>
        private bool CheckValidPassword(string password)
        {
            return password != null && password.Length >= MIN_PASSWORD_LENGTH && password.Length <= MAX_PASSWORD_LENGTH
                && password.Any(char.IsLower) && password.Any(char.IsUpper) && password.Any(char.IsDigit);
        }

        /// <summary>
        /// Checks if a email address is valid.
        /// </summary>
        /// <param name="email">The email address to check.</param>
        /// <returns>True if the emails address is valid and false otherwise.</returns>
        private bool CheckEmailAddress(string email)
        {
            try
            {
                MailAddress mail = new MailAddress(email);
                Regex r = new Regex(@"^([0-9a-zA-Z]([-\.\w]*[0-9a-zA-Z])*@([0-9a-zA-Z][-\w]*[0-9a-zA-Z]\.)+[a-zA-Z]{2,9})$");
                return !String.IsNullOrEmpty(email) && r.IsMatch(email) && new EmailAddressAttribute().IsValid(email);
            }
            catch (Exception e)
            {
                return false;
            }
        }
    } 
}
