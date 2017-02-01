using ClassLibrary1.Entities;
using DatabaseClassLibrary.Database;
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

namespace WpfApplication3
{
    /// <summary>
    /// Logique d'interaction pour UserCreate.xaml
    /// </summary>
    public partial class UserCreateWindow : Window
    {
        private User userCreate;

        public User UserCreate
        {
            get { return userCreate; }
            set { userCreate = value; }
        }

        MySQLManager<Role> roleManager = new MySQLManager<Role>(DataConnectionResource.LOCALMYSQL);
        MySQLManager<User> userManager = new MySQLManager<User>(DataConnectionResource.LOCALMYSQL);
        List<Role> roles;
        public UserCreateWindow()
        {
            InitializeComponent();
            this.userCreate = new User();
            this.DataContext = this;
            LoadRoles();
            this.btnValidate.Click += BtnValidate_Click;
        }

        private async void BtnValidate_Click(object sender, RoutedEventArgs e)
        {
            List<Role> userRoles = new List<Role>();
            foreach (object childStackPanel in this.stackpanelRoles.Children)
            {
                if (childStackPanel is StackPanel)
                {
                    foreach (object child in (childStackPanel as StackPanel).Children)
                    {
                        if (child is CheckBox)
                        {
                            if ((child as CheckBox).IsChecked == true)
                            {
                                Role role = roles.FirstOrDefault(x => x.Name == (child as CheckBox).Name.Replace("checkbox",""));
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

            UserCreateWindow userCreate = new UserCreateWindow();
            userCreate.Show();
            this.Close();
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
                this.stackpanelRoles.Children.Add(stp);
            }
        }
    }
}
