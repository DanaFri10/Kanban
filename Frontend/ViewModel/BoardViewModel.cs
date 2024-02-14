using Frontend.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Frontend.ViewModel
{
    internal class BoardViewModel
    {
        public BackendController Controller { get; private set; }
        public BoardModel Board { get; private set; }
        public UserModel User { get; }

        /// <summary>
        /// Initializes a new instance of the BoardViewModel class.
        /// </summary>
        /// <param name="board">The board shown in the page.</param>
        /// <param name="user">The currently logged in user.</param>
        public BoardViewModel(BoardModel board, UserModel user)
        {
            this.Controller = board.Controller;
            this.Board = board;
            this.User = user;
        }
    }
}
