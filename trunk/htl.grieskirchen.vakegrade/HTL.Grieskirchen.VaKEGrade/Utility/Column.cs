using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace HTL.Grieskirchen.VaKEGrade.Utility
{
    public class Column
    {


        string name;

        public string Name
        {
            get { return name; }
            set { name = value; }
        }
        string index;

        public string Index
        {
            get { return index; }
            set { index = value; }
        }
        int width;

        public int Width
        {
            get { return width; }
            set { width = value; }
        }
        string align;

        public string Align
        {
            get { return align; }
            set { align = value; }
        }

    }
}
