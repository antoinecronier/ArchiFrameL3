using ClassLibrary1.Entities.Base;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClassLibrary1.Entities
{
    [Table("user")]
    public class User : EntityBase
    {
        private int id;

        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int UserId
        {
            get { return id; }
            set { id = value; }
        }

        private String firstname;

        public String Firstname
        {
            get { return firstname; }
            set { firstname = value;
                OnPropertyChanged("Firstname");
            }
        }

        private String lastname;

        public String Lastname
        {
            get { return lastname; }
            set { lastname = value;
                OnPropertyChanged("Lastname");
            }
        }

        private String login;

        public String Login
        {
            get { return login; }
            set { login = value;
                OnPropertyChanged("Login");
            }
        }

        private String password;

        public String Password
        {
            get { return password; }
            set { password = value;
                OnPropertyChanged("Password");
            }
        }

        private ICollection<Role> roles;

        public ICollection<Role> Roles
        {
            get { return roles; }
            set { roles = value; }
        }

        private ICollection<Data> datas;

        public ICollection<Data> Datas
        {
            get { return datas; }
            set { datas = value; }
        }

        public User()
        {
            this.roles = new HashSet<Role>();
            this.datas = new HashSet<Data>();
        }
    }
}
