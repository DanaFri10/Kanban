using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IntroSE.Kanban.Backend.ServiceLayer
{
    public class User
    {
        public string EmailAddress { get; set; }
        public bool IsLoggedIn { get; set; }

        public User() { }
        public User(string emailAddress, bool isLoggedIn)
        {
            EmailAddress = emailAddress;
            IsLoggedIn = isLoggedIn;
        }

        public User(BusinessLayer.User user)
        {
            EmailAddress = user.EmailAddress;
            IsLoggedIn = user.IsLoggedIn;
        }
    }
}
