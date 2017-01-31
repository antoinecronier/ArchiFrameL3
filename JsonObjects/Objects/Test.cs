using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JsonObjects.Objects
{
    public class Test
    {
        private int myVar;

        public int MyProperty
        {
            get { return myVar; }
            set { myVar = value; }
        }

        private String myVar1;

        public String MyProperty1
        {
            get { return myVar1; }
            set { myVar1 = value; }
        }

        private Boolean myVar2;

        public Boolean MyProperty2
        {
            get { return myVar2; }
            set { myVar2 = value; }
        }

        private int myVar3;

        public int MyProperty3
        {
            get { return myVar3; }
            set { myVar3 = value; }
        }
    }
}
