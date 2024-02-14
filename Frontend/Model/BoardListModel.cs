using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Frontend.Model
{
    public class BoardListModel : NotifiableModelObject
    {
        public ObservableCollection<BoardModel> Boards { get; set; }

        /// <summary>
        /// Constructor for BoardList model object, that gets the list of the boards.
        /// </summary>
        /// <param name="controller">The backend controller.</param>
        /// <param name="boards">A list of the boards.</param>
        private BoardListModel(BackendController controller, List<BoardModel> boards) : base(controller)
        {
            Boards = new ObservableCollection<BoardModel>(boards);
        }

        /// <summary>
        /// Constructor for BoardList model object, that gets the user that we want to present the boards of.
        /// </summary>
        /// <param name="controller">The backend controller.</param>
        /// <param name="user">The user model object.</param>
        public BoardListModel(BackendController controller, UserModel user) : this(controller, controller.ListBoardsOfUser(user.Email))
        {
        }

    }
}
