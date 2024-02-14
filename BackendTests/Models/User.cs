using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IntroSE.Kanban.BackendTests
{
    public class User
    {
        public string EmailAddress { get; }
        public bool IsLoggedIn { get; }
        public User(string emailAddress, bool isLoggedIn)
        {
            EmailAddress = emailAddress;
            IsLoggedIn = isLoggedIn;
        }
    }
}
