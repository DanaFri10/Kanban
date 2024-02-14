using Frontend.Model;
using Frontend.View;
using IntroSE.Kanban.Backend.ServiceLayer;
using IntroSE.Kanban.Frontend;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Frontend.ViewModel
{
    public class BoardListViewModel : NotifiableObject
    {
        private Model.BackendController controller;
        public BoardListModel BoardList { get; private set; }

        public UserModel User { get; }

        private BoardModel _selectedBoard;
        public BoardModel SelectedBoard { get { return _selectedBoard; } set {_selectedBoard = value; } }

        /// <summary>
        /// Constructor for BoardList ViewModel object
        /// </summary>
        /// <param name="user">The User model object.</param>
        public BoardListViewModel(UserModel user)
        {
            this.controller = user.Controller;
            User = user;
            BoardList = new BoardListModel(controller, user);
        }

        /// <summary>
        /// logout user
        /// </summary>
        /// <returns></returns>
        internal bool Logout()
        {
            try
            {
                controller.Logout(User.Email);
                return true;
            }
            catch (Exception e)
            {
                return false;
            }
        }
    }
}
