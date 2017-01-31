using ClassLibrary1.Entities;
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
    [Table("role")]
    public class Role : EntityBase
    {
        private int id;

        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int RoleId
        {
            get { return id; }
            set { id = value; }
        }

        private String name;

        public String Name
        {
            get { return name; }
            set { name = value;
                OnPropertyChanged("Name");
            }
        }

        private ICollection<User> users;

        public ICollection<User> Users
        {
            get { return users; }
            set { users = value; }
        }

        public Role()
        {
            this.Users = new HashSet<User>();
        }
    }
}
