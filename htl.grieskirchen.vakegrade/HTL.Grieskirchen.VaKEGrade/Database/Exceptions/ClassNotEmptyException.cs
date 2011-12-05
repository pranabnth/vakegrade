using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HTL.Grieskirchen.VaKEGrade.Database.Exceptions
{
    public class ClassNotEmptyException : Exception
    {
        SchoolClass schoolClass;

        public SchoolClass SchoolClass
        {
            get { return schoolClass; }
        }

        public ClassNotEmptyException(SchoolClass schoolClass)
            : base("The teacher's class \""+schoolClass.Name + schoolClass.Level +"\" is still existing and contains pupils")
        {
    
        }
    }
}