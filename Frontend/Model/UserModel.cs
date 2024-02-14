namespace Frontend.Model
{
    public class UserModel : NotifiableModelObject
    {
        private string _email;
        public string Email
        {
            get => _email;
            set
            {
                _email = value;
                //RaisePropertyChanged("Email");
            }
        }

        /// <summary>
        /// constructor
        /// </summary>
        /// <param name="controller">BackendController</param>
        /// <param name="email">user email</param>
        public UserModel(BackendController controller, string email) : base(controller)
        {
            this.Email = email;
        }
    }
}
