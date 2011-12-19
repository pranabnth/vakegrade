using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HTL.Grieskirchen.VaKEGrade.Utility
{
    public class WebSPF : Database.SPF
    {
        string subjectName;

        public string SubjectName
        {
            get { return subjectName; }
            set { subjectName = value; }
        }

    }
}