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
    /// UserDTO class represents a record of the Users table in the database.
    /// </summary>
    public class UserDTO : DTO
    {

        private string _passeword;
        public string Password { 
            get => _passeword; 
            set
            { 
                _dalController.Update(new string[] {EmailAddress},"Password", value);
                _passeword = value;
            }
        }
         
        public string EmailAddress { get; }

        private static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>
        /// Constructor for initializing a new instance of the UserDTO object, according to the given parameters.
        /// </summary>
        /// <param name="emailAddress">The user's email address.</param>
        /// <param name="password">The user's password.</param>
        public UserDTO(string emailAddress, string password) : base(new UserDalController())
        {
            EmailAddress = emailAddress;
            _passeword = password;
        }

        /// <summary>
        /// This method deletes the UserDTO from the DB.
        /// </summary>
        public override void Delete()
        {
            _dalController.Delete(new string[] { EmailAddress});
            log.Debug($"Deleted the UserDTO with email {EmailAddress} from DB.");
        }

        /// <summary>
        /// This method inserts the UserDTO to the DB.
        /// </summary>
        public override void Insert()
        {
            _dalController.Insert(new string[] { "EmailAddress", "Password" }, new string[] { EmailAddress, Password });
            log.Debug($"Inserted the UserDTO with email {EmailAddress} to the DB.");
        }
    }
}
