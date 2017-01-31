using ClassLibrary1;
using ClassLibrary1.Entities;
using DatabaseClassLibrary.EnumManager;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DatabaseClassLibrary.Database
{
    [DbConfigurationType(typeof(MySql.Data.Entity.MySqlEFConfiguration))]
    public class MySQLFullDB : DbContext
    {
        public DbSet<Data> DbSetData { get; set; }
        public DbSet<Role> DbSetRole { get; set; }
        public DbSet<User> DbSetUser { get; set; }

        public MySQLFullDB(DataConnectionResource dataConnectionResource)
            : base(EnumString.GetStringValue(dataConnectionResource))
        {
            switch (dataConnectionResource)
            {
                case DataConnectionResource.GENERICMYSQL:
                    break;
                case DataConnectionResource.LOCALMYSQL:
                    InitLocalMySQL();
                    break;
                case DataConnectionResource.LOCALAPI:
                    break;
                default:
                    break;
            }
        }

        public async void InitLocalMySQL()
        {
            if (this.Database.CreateIfNotExists())
            {
                MySQLManager<Role> roleManager = new MySQLManager<Role>(DataConnectionResource.LOCALMYSQL);
                MySQLManager<User> userManager = new MySQLManager<User>(DataConnectionResource.LOCALMYSQL);

                Role adminRole = new Role();
                adminRole.Name = "admin";
                await roleManager.Insert(adminRole);

                Role wpfUserRole = new Role();
                wpfUserRole.Name = "wpf_user";
                await roleManager.Insert(wpfUserRole);

                Role commandLineRole = new Role();
                commandLineRole.Name = "command_line";
                await roleManager.Insert(commandLineRole);

                User adminUser = new User();
                adminUser.Login = "admin";
                adminUser.Password = "admin";
                adminUser.Roles.Add(DbSetRole.FirstOrDefault(x => x.Name == "admin"));

                foreach (var item in adminUser.Roles)
                {
                    DbSetRole.Attach(item);
                }

                DbSetUser.Add(adminUser);
                this.SaveChanges();
            }
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>()
                .HasMany<Role>(s => s.Roles)
                .WithMany(c => c.Users)
                .Map(cs =>
                {
                    cs.MapLeftKey("User_UserId");
                    cs.MapRightKey("Role_RoleId");
                    cs.ToTable("roleusers");
                });
        }
    }
}