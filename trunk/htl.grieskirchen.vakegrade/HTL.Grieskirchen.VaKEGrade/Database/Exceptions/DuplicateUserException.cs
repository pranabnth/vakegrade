using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HTL.Grieskirchen.VaKEGrade.Database.Exceptions
{
    public class DuplicateUserException : Exception
    {
        public DuplicateUserException(string msg) : base(msg){
    
        }

    }
}