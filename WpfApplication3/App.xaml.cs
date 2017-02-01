using ClassLibrary1.Entities;
using DatabaseClassLibrary.Database;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using WpfApplication3.ViewModels;
using WpfApplication3.Views;

namespace WpfApplication3
{
    /// <summary>
    /// Logique d'interaction pour App.xaml
    /// </summary>
    public partial class App : Application
    {
        public App()
        {
            Window mainWindow = new Window();
            mainWindow.Content = new LoginView();
            mainWindow.Loaded += MainWindow_Loaded;
            mainWindow.Show();
        }

        private void MainWindow_Loaded(object sender, RoutedEventArgs e)
        {
            LoginViewModel model = new LoginViewModel();
        }
    }
}
