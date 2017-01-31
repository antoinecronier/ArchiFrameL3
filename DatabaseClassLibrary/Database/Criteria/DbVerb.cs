using DatabaseClassLibrary.EnumManager;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DatabaseClassLibrary.Database
{
    public enum DbVerb
    {
        [StringValue("AND")]
        AND,
        [StringValue("OR")]
        OR,
        [StringValue("")]
        EMPTY
    }
}
