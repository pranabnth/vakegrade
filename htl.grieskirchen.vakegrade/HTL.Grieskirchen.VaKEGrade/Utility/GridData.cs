using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace HTL.Grieskirchen.VaKEGrade.Utility
{
    public class GridData : Controller
    {
        public int total { get; set; }
        public int page { get; set; }
        public int records { get; set; }
        public RowData[] rows;

    }
}
