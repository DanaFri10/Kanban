using Frontend.Model;
using Frontend.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace Frontend.View
{
    /// <summary>
    /// Interaction logic for BoardView.xaml
    /// </summary>
    public partial class BoardView : Window
    {
        private BoardViewModel viewModel;

        /// <summary>
        /// Initializes a new instance of the BoardView class.
        /// </summary>
        /// <param name="board">The board shown in the page.</param>
        /// <param name="user">The currently logged in user.</param>
        public BoardView(BoardModel board, UserModel user)
        {
            InitializeComponent();
            this.viewModel = new BoardViewModel(board, user);
            this.DataContext = viewModel;
        }

        private void ReturnToBoards_Click(object sender, RoutedEventArgs e)
        {
            ListBoards boardListView = new ListBoards(viewModel.User);
            boardListView.Show();
            this.Close();
        }
    }
}
