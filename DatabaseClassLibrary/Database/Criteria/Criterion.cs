using DatabaseClassLibrary.EnumManager;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DatabaseClassLibrary.Database
{
    public class Criterion
    {
        private DbOperator dbOperator;

        private DbVerb verb;

        private object value;

        private String dbColumn;
        
        public Criterion()
        {

        }

        public Criterion(DbVerb verb, String dbColumn, DbOperator dbOperator, object value)
        {
            this.verb = verb;
            this.dbColumn = dbColumn;
            this.dbOperator = dbOperator;
            this.value = value;
        }

        public String MySQLCompute()
        {
            return EnumString.GetStringValue(this.verb) + " " + this.dbColumn + " " + EnumString.GetStringValue(this.dbOperator) + " " + this.value.ToString();
        }
    }
}
