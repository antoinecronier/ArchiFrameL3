using ClassLibrary1.Entities;
using DatabaseClassLibrary.Database;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using WpfApplication3.Views;

namespace WpfApplication3.ViewModels
{
    public class UserManagerViewModel
    {
        private UserManagerView userManagerView;
        private User userCreate;

        public User UserCreate
        {
            get { return userCreate; }
            set { userCreate = value; }
        }

        MySQLManager<Role> roleManager = new MySQLManager<Role>(DataConnectionResource.LOCALMYSQL);
        MySQLManager<User> userManager = new MySQLManager<User>(DataConnectionResource.LOCALMYSQL);
        List<Role> roles;
        ObservableCollection<User> userCollection;

        public UserManagerViewModel()
        {
            this.userManagerView = new UserManagerView();
            this.userManagerView.DataContext = this;

            Application.Current.Windows.OfType<Window>().SingleOrDefault(x => x.IsActive).Content = this.userManagerView;

            this.userCreate = new User();
            this.userCollection = new ObservableCollection<User>();

            this.userManagerView.itemsList.ItemsSource = this.userCollection;

            LoadRoles();
            LoadUsers();

            this.userManagerView.btnValidate.Click += BtnValidate_Click;
            this.userManagerView.btnLogout.Click += BtnLogout_Click;
        }

        private async void LoadUsers()
        {
            foreach (var item in (await userManager.Get()).ToList())
            {
                this.userCollection.Add(item);
            }
        }

        private void BtnLogout_Click(object sender, RoutedEventArgs e)
        {
            LoginViewModel viewModel = new LoginViewModel();
        }

        private async void BtnValidate_Click(object sender, RoutedEventArgs e)
        {
            List<Role> userRoles = new List<Role>();
            foreach (object childStackPanel in this.userManagerView.stackpanelRoles.Children)
            {
                if (childStackPanel is StackPanel)
                {
                    foreach (object child in (childStackPanel as StackPanel).Children)
                    {
                        if (child is CheckBox)
                        {
                            if ((child as CheckBox).IsChecked == true)
                            {
                                Role role = roles.FirstOrDefault(x => x.Name == (child as CheckBox).Name.Replace("checkbox", ""));
                                if (role != null)
                                {
                                    userRoles.Add(role);
                                }
                            }
                        }
                    }
                }
            }
            this.userCreate.Roles = userRoles;
            await userManager.Insert(this.userCreate);
            MessageBox.Show("Data Inserted");
            UserManagerViewModel viewModel = new UserManagerViewModel();
        }

        private async void LoadRoles()
        {
            roles = (await roleManager.Get()).ToList();
            foreach (var item in roles)
            {
                StackPanel stp = new StackPanel();
                stp.Orientation = Orientation.Horizontal;
                Label lbl = new Label();
                lbl.Content = item.Name;
                stp.Children.Add(lbl);
                CheckBox checkbox = new CheckBox();
                checkbox.Name = "checkbox" + item.Name;
                stp.Children.Add(checkbox);
                this.userManagerView.stackpanelRoles.Children.Add(stp);
            }
        }
    }
}
