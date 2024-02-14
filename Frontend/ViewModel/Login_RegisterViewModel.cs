using Frontend.Model;
using System;

namespace IntroSE.Kanban.Frontend.ViewModel
{
    class Login_RegisterViewModel : NotifiableObject
    {
        public BackendController Controller { get; private set; }

        private string _username;
        public string Username
        {
            get => _username;
            set
            {
                this._username = value;
                RaisePropertyChanged("Username");
            }
        }
        private string _password;
        public string Password
        {
            get => _password;
            set
            {
                this._password = value;
                RaisePropertyChanged("Password");
            }
        }
        private string _message;
        public string Message
        {
            get => _message;
            set
            {
                this._message = value;
                RaisePropertyChanged("Message");
            }
        }

        /// <summary>
        /// login user
        /// </summary>
        /// <returns></returns>
        public UserModel Login()
        {
            Message = "";
            try
            {
                Message = "Login successfully";
                return Controller.Login(Username, Password);
            }
            catch (Exception e)
            {
                Message = e.Message;
                return null;
            }
        }

        /// <summary>
        /// register new user
        /// </summary>
        /// <returns></returns>
        public UserModel Register()
        {
            Message = "";
            try
            {
                Message = "Registered successfully";
                return Controller.Register(Username, Password);
            }
            catch (Exception e)
            {
                Message = e.Message;
                return null;
            }
        }

        public Login_RegisterViewModel()
        {
            this.Controller = new BackendController();
            this.Username = "achiya@bgu.ac.il";
            this.Password = "aA1234!";
        }
    }
}
