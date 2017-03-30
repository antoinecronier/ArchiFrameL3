using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ClassLibrary1.Entities;
using DatabaseClassLibrary.Database;

namespace DatabaseManagerUtil.Database
{
    public class RoleMySqlManager : MySQLManager<Role>
    {
        public RoleMySqlManager() : base(DataConnectionResource.LOCALMYSQL)
        {
        }

        public Role GetUsers(Role role)
        {
            this.DbSetT.Attach(role);
            this.Entry(role).Collection(x => x.Users).Load();
            return role;
        }
    }
}
