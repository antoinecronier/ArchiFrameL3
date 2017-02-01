using ClassLibrary1.Entities;
using DatabaseClassLibrary.Database;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DatabaseManagerUtil.Database
{
    public class UserMySqlManager : MySQLManager<User>
    {
        public UserMySqlManager() : base(DataConnectionResource.LOCALMYSQL)
        {
        }

        public User GetByLogin(string text, string password)
        {
            User user;
            try
            {
                user = this.DbSetT
                            .Where(x => x.Login == text)
                            .Where(x => x.Password == password)
                            .First();
                this.Entry(user).Collection(x => x.Roles).Load();
            }
            catch (Exception)
            {
                user = new User();
            }
            
            return user;
        }

        public User GetDatas(User user)
        {
            this.DbSetT.Attach(user);
            this.Entry(user).Collection(x => x.Datas).Load();
            return user;
        }
    }
}
