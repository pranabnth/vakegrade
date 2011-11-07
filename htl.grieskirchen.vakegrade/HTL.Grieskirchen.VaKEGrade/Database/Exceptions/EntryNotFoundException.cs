using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HTL.Grieskirchen.VaKEGrade.Database.Exceptions
{
    public class EntryNotFoundException : Exception
    {
        public EntryNotFoundException(string msg) : base(msg) { 
        
        }

    }
}