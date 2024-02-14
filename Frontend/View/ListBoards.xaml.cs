using Frontend.Model;
using Frontend.ViewModel;
using IntroSE.Kanban.Frontend.View;
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
    /// Interaction logic for ListBoards.xaml
    /// </summary>
    public partial class ListBoards : Window
    {
        private BoardListViewModel viewModel;

        /// <summary>
        /// Constructor for ListBoards view object.
        /// </summary>
        /// <param name="u">The user we want to present the list of boards to.</param>
        public ListBoards(UserModel u)
        {
            InitializeComponent();
            this.viewModel = new BoardListViewModel(u);
            this.DataContext = viewModel;
        }

        /// <summary>
        /// Clicking on a board.
        /// </summary>
        public void onClick(object sender, RoutedEventArgs e)
        {
            BoardView boardView = new BoardView(viewModel.SelectedBoard, viewModel.User);
            boardView.Show();
            this.Close();
        }

        /// <summary>
        /// clickin on logout
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Logout_Click(object sender, RoutedEventArgs e)
        {
            bool sucsess = viewModel.Logout();
            if (sucsess)
            {
                Login_RegisterPage login_RegisterPage = new Login_RegisterPage();
                login_RegisterPage.Show();
                this.Close();
            }
        }

    }
}
