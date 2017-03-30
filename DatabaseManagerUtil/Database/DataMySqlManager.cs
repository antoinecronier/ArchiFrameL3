using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ClassLibrary1.Entities;
using DatabaseClassLibrary.Database;

namespace DatabaseManagerUtil.Database
{
    public class DataMySqlManager : MySQLManager<Data>
    {
        public DataMySqlManager() : base(DataConnectionResource.LOCALMYSQL)
        {
        }

        public Data GetUser(Data data)
        {
            this.DbSetT.Attach(data);
            this.Entry(data).Reference(x => x.User).Load();
            return data;
        }
    }
}
