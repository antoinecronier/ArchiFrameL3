using DatabaseClassLibrary.AsyncManager;
using DatabaseClassLibrary.EnumManager;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DatabaseClassLibrary.Database
{
    public class MysqlOpenHelper
    {
        #region Singleton
        private static volatile MysqlOpenHelper instance;
        private static object syncRoot = new Object();

        private MysqlOpenHelper() { }

        public static MysqlOpenHelper Instance
        {
            get
            {
                if (instance == null)
                {
                    lock (syncRoot)
                    {
                        if (instance == null)
                            instance = new MysqlOpenHelper();
                    }
                }

                return instance;
            }
        }
        #endregion

        #region Attributs
        private String connectionString;
        private MySqlConnection connection;
        private CommandWorker commandWorker;
        #endregion

        #region Properties
        public String ConnectionString
        {
            get { return connectionString; }
            private set { connectionString = value; }
        }
        #endregion

        #region Methods
        public void Init(ConnectionStringEnum connectionStringEnum)
        {
            this.ConnectionString = EnumString.GetStringValue(connectionStringEnum);
            this.commandWorker = new CommandWorker("MysqlOpenHelper");
            this.commandWorker.Start();
        }

        private void Connect()
        {
            this.connection = new MySqlConnection(this.ConnectionString);
            this.connection.OpenAsync();
        }

        private void Disconnect()
        {
            this.connection.CloseAsync();
        }
        
        #endregion

    }
}
