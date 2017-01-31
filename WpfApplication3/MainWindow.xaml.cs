using ClassLibrary1;
using ClassLibrary1.Entities;
using DatabaseClassLibrary.Database;
using DatabaseManagerUtil.Database;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace WpfApplication3
{
    /// <summary>
    /// Logique d'interaction pour MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            this.btnLogMe.Click += BtnLogMe_Click;
        }

        private void BtnLogMe_Click(object sender, RoutedEventArgs e)
        {
            UserMySqlManager userManager = new UserMySqlManager();
            User loggedUser = userManager.GetByLogin(this.txtBoxLogin.Text, this.passwordBoxPassword.Password);
            
            if (loggedUser.Roles.Select(x => x.Name == "admin").FirstOrDefault())
            {
                UserCreateWindow userCreate = new UserCreateWindow();
                userCreate.Show();
                this.Close();
            }
            else if (loggedUser.Roles.Select(x => x.Name == "wpf_user").FirstOrDefault())
            {
                WindowData windowData = new WindowData();
                windowData.Show();
                this.Close();
            }
        }
    }
}
