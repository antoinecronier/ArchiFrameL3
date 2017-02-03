using ClassLibrary1.Entities;
using ConsoleUtil;
using DatabaseManagerUtil.Database;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApplication1
{
    public class UserLogger
    {
        private static UserLogger instance;
        private UserMySqlManager manager;

        private UserLogger()
        {
            manager = new UserMySqlManager();
        }

        public static UserLogger Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new UserLogger();
                }
                return instance;
            }
        }

        public void ConnectUser()
        {
            Console.WriteLine("Connect your self");
            Console.Write("Login :");
            String login = ConsoleManager.Instance.AnonymousChoice();
            Console.Write("Password :");
            String password = ConsoleManager.Instance.AnonymousChoice();

            User loggedUser = manager.GetByLogin(login, password);

            if (loggedUser.Roles.FirstOrDefault(x => x.Name == "command_line") != null)
            {
                Console.Clear();
                while (true)
                {
                    DataManager.Instance.PrintUserChoice();
                }
            }
            else
            {
                Console.WriteLine("Bad credential");
                Console.ReadLine();
                Console.Clear();
                ConnectUser();
            }
        }
    }
}
