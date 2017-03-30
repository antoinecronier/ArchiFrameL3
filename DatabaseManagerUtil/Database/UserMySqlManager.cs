using ClassLibrary1.Entities;
using DatabaseClassLibrary.Database;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.Entity;
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

        public User GetRoles(User user)
        {
            this.DbSetT.Attach(user);
            this.Entry(user).Collection(x => x.Roles).Load();
            return user;
        }

        public async Task<User> UpdateWithChildrens(User user)
        {
            UpdateDatas(user);
            UpdateRoles(user);
            await fullDB.SaveChangesAsync();
            return user;
        }

        public void UpdateDatas(User user)
        {
            var item = fullDB.DbSetUser.Include("Datas")
                .Where(x => x.UserId == user.UserId).FirstOrDefault<User>();

            /* 2- Find deleted courses from student's course collection by 
            students' existing courses (existing data from database) minus students' 
            current course list (came from client in disconnected scenario) */
            var deletedDatas = item.Datas.Except(user.Datas,
                    data => data.Id).ToList<Data>();

            /* 3- Find Added courses in student's course collection by students' 
            current course list (came from client in disconnected scenario) minus 
            students' existing courses (existing data from database)  */
            var addedData = user.Datas.Except(item.Datas,
                    data => data.Id).ToList<Data>();

            /* 4- Remove deleted courses from students' existing course collection 
            (existing data from database)*/
            deletedDatas.ForEach(c => item.Datas.Remove(c));

            //5- Add new courses
            foreach (Data c in addedData)
            {
                /*6- Attach courses because it came from client 
                as detached state in disconnected scenario*/
                if (fullDB.Entry(c).State == EntityState.Detached)
                    fullDB.DbSetData.Attach(c);

                //7- Add course in existing student's course collection
                item.Datas.Add(c);
            }
        }

        public void UpdateRoles(User user)
        {
            var item = fullDB.DbSetUser.Include("Roles")
                .Where(x => x.UserId == user.UserId).FirstOrDefault<User>();

            /* 2- Find deleted courses from student's course collection by 
            students' existing courses (existing role from database) minus students' 
            current course list (came from client in disconnected scenario) */
            var deletedDatas = item.Roles.Except(user.Roles,
                    role => role.RoleId).ToList<Role>();

            /* 3- Find Added courses in student's course collection by students' 
            current course list (came from client in disconnected scenario) minus 
            students' existing courses (existing role from database)  */
            var addedData = user.Roles.Except(item.Roles,
                    role => role.RoleId).ToList<Role>();

            /* 4- Remove deleted courses from students' existing course collection 
            (existing role from database)*/
            deletedDatas.ForEach(c => item.Roles.Remove(c));

            //5- Add new courses
            foreach (Role c in addedData)
            {
                /*6- Attach courses because it came from client 
                as detached state in disconnected scenario*/
                if (fullDB.Entry(c).State == EntityState.Detached)
                    fullDB.DbSetRole.Attach(c);

                //7- Add course in existing student's course collection
                item.Roles.Add(c);
            }
        }
    }

    public static class Extension
    {
        public static IEnumerable<T> Except<T, TKey>(this IEnumerable<T> items, IEnumerable<T> other,
                                                                            Func<T, TKey> getKey)
        {
            return from item in items
                   join otherItem in other on getKey(item)
                   equals getKey(otherItem) into tempItems
                   from temp in tempItems.DefaultIfEmpty()
                   where ReferenceEquals(null, temp) || temp.Equals(default(T))
                   select item;

        }
    }
}
