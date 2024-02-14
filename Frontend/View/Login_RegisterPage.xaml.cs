using Frontend.Model;
using Frontend.View;
using IntroSE.Kanban.Frontend.ViewModel;
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


namespace IntroSE.Kanban.Frontend.View
{
    /// <summary>
    /// Interaction logic for Login_RegisterPage.xaml
    /// </summary>
    public partial class Login_RegisterPage : Window
    {
        private Login_RegisterViewModel viewModel;
        public Login_RegisterPage()
        {
            InitializeComponent();
            this.DataContext = new Login_RegisterViewModel();
            this.viewModel = (Login_RegisterViewModel)DataContext;

        }

        /// <summary>
        /// click in login
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Login_Click(object sender, RoutedEventArgs e)
        {
            UserModel u = viewModel.Login();
            if (u != null)
            {
                ListBoards listBoardView = new ListBoards(u);
                listBoardView.Show();
                this.Close();
            }
        }

        /// <summary>
        /// click in register
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Register_Click(object sender, RoutedEventArgs e)
        {
            UserModel u = viewModel.Register();
            if (u != null)
            {
                ListBoards listBoardView = new ListBoards(u);
                listBoardView.Show();
                this.Close();
            }
        }

        /// <summary>
        /// update change on password in viewModel (PasswordBox not work with bindings)
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void PasswordBox_PasswordChanged(object sender, RoutedEventArgs e)
        {
            if (this.DataContext != null)
            { ((dynamic)this.DataContext).Password = ((PasswordBox)sender).Password; }
        }
    }
}
