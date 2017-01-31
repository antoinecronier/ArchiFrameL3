using DatabaseClassLibrary.EnumManager;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DatabaseClassLibrary.Database
{
    public enum DataConnectionResource : Int32
    {
        [StringValue("Server=myServer;Port=3306;Database=db_name;Uid=userName;Pwd=password")]
        GENERICMYSQL = 1,
        [StringValue("Server=127.0.0.1;Port=3306;Database="+ DatabaseConfig.DB_NAME + ";Uid=root;Pwd=''")]
        LOCALMYSQL = 2,
        [StringValue("http://localhost:63684/api/")]
        LOCALAPI = 3,
    }
}
