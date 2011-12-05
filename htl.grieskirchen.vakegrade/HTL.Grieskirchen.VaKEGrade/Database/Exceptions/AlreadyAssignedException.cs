using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HTL.Grieskirchen.VaKEGrade.Database.Exceptions
{
    public class AlreadyAssignedException : Exception
    {
        SchoolClass schoolClass;
        Teacher teacher;

        public SchoolClass SchoolClass
        {
            get { return schoolClass; }
            set { schoolClass = value; }
        }

        public Teacher Teacher
        {
            get { return teacher; }
            set { teacher = value; }
        }
        
        public AlreadyAssignedException(SchoolClass schoolClass, Teacher teacher) : base("Assignment not possible: Teacher \""+teacher.FirstName + " " + teacher.LastName+"\" is already Classteacher of schoolClass \""+schoolClass.Name+schoolClass.Level+"\".") {
            this.schoolClass = schoolClass;
            this.teacher = teacher;
        }
    }
}