using ClassLibrary1.Entities;
using DatabaseManagerUtil.Database;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using WpfApplication3.Views;

namespace WpfApplication3.ViewModels
{
    public class LoginViewModel
    {
        private LoginView loginView;

        public LoginViewModel()
        {
            this.loginView = new LoginView();
            this.loginView.btnLogMe.Click += BtnLogMe_Click;
            this.loginView.DataContext = this;

            Application.Current.Windows.OfType<Window>().SingleOrDefault(x => x.IsActive).Content = this.loginView;
        }

        private void BtnLogMe_Click(object sender, RoutedEventArgs e)
        {
            UserMySqlManager userManager = new UserMySqlManager();
            User loggedUser = userManager
                .GetByLogin(
                    this.loginView.txtBoxLogin.Text, 
                    this.loginView.passwordBoxPassword.Password);

            if (loggedUser.Roles.Select(x => x.Name == "admin").FirstOrDefault())
            {
                UserManagerViewModel viewModel = new UserManagerViewModel();
            }
            else if (loggedUser.Roles.Select(x => x.Name == "wpf_user").FirstOrDefault())
            {
                DataInsertViewModel viewModel = new DataInsertViewModel(loggedUser);
            }
        }
    }
}
