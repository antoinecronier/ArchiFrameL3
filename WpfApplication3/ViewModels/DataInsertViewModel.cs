using ClassLibrary1.Entities;
using DatabaseClassLibrary.Database;
using DatabaseManagerUtil.Database;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using WpfApplication3.Views;

namespace WpfApplication3.ViewModels
{
    public class DataInsertViewModel
    {
        private User LoggedUser { get; set; }
        private DataInsertView dataInsertView;
        private ObservableCollection<Data> dataCollection;

        public DataInsertViewModel(User loggedUser)
        {
            this.dataInsertView = new DataInsertView();
            this.dataInsertView.btnValidate.Click += BtnValidate_Click;
            this.dataInsertView.btnlogout.Click += Btnlogout_Click;

            LoggedUser = loggedUser;
            dataCollection = new ObservableCollection<Data>();

            this.dataInsertView.DataContext = this;
            this.dataInsertView.itemsList.ItemsSource = this.dataCollection;

            LoadListDatas();

            Application.Current.Windows.OfType<Window>().SingleOrDefault(x => x.IsActive).Content = this.dataInsertView;
        }

        private void Btnlogout_Click(object sender, RoutedEventArgs e)
        {
            LoginViewModel viewModel = new LoginViewModel();
        }

        private void LoadListDatas()
        {
            UserMySqlManager userManager = new UserMySqlManager();
            foreach (var item in userManager.GetDatas(LoggedUser).Datas)
            {
                this.dataCollection.Add(item);
            }
        }

        private async void BtnValidate_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            try
            {
                String json = this.dataInsertView.txtBoxJson.Text;
                json = json.Replace("\r\n", " ");
                json = json.Replace("\n", " ");
                json = json.Replace("\\", " ");
                JsonConvert.DeserializeObject<Object>(json);

                MySQLManager<User> userManager = new MySQLManager<User>(DataConnectionResource.LOCALMYSQL);
                User currentUser = await userManager.Get(LoggedUser.UserId);
                MySQLManager<Data> dataManager = new MySQLManager<Data>(DataConnectionResource.LOCALMYSQL);
                Data data = new Data();
                data.JsonData = this.dataInsertView.txtBoxJson.Text;
                data.User = currentUser;
                await dataManager.Insert(data);

                DataInsertViewModel viewModel = new DataInsertViewModel(currentUser);
            }
            catch (Exception ex)
            {
                this.dataInsertView.txtBoxJson.Text = ex.Message;
            }
            
        }
    }
}
