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
    [Table("data")]
    public class Data : EntityBase
    {
        private int id;

        [Key]
        public int Id
        {
            get { return id; }
            set { id = value; }
        }

        private String jsonData;

        public String JsonData
        {
            get { return jsonData; }
            set { jsonData = value;
                OnPropertyChanged("JsonData");
            }
        }
    }
}
