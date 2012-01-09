using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Trirand.Web.Mvc;


namespace HTL.Grieskirchen.VaKEGrade.Utility
{
    public class JQGridConf 
    {
        List<Column> colModel = new List<Column>();

        public List<Column> ColModel
        {
            get { return colModel; }
            set { colModel = value; }
        }
        List<string> colNames = new List<string>();

        public List<String> ColNames
        {
            get { return colNames; }
            set { colNames = value; }
        }

    }
}
