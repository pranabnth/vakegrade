using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace HTL.Grieskirchen.VaKEGrade.Utility
{
    public class Column
    {
        bool _editable;

        public bool editable
        {
            get { return _editable; }
            set { _editable = value; }
        }

        string _name;

        public string name
        {
            get { return _name; }
            set { _name = value; }
        }
        string _index;

        public string index
        {
            get { return _index; }
            set { _index = value; }
        }
        int _width;

        public int width
        {
            get { return _width; }
            set { _width = value; }
        }
        string _align;

        public string align
        {
            get { return _align; }
            set { _align = value; }
        }

    }
}
