using DatabaseManagerUtil.Database;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApplication1
{
    public class Program
    {
        public static void Main(string[] args)
        {
            UserLogger.Instance.ConnectUser();

            Console.ReadLine();
        }
    }
}
