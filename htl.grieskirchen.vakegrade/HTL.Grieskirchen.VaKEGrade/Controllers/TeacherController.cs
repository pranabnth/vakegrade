using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace HTL.Grieskirchen.VaKEGrade.Controllers
{
    public class TeacherController : Controller
    {
        //
        // GET: /Teacher/

        public ActionResult Index()
        {
            if (Session["User"] != null)
            {
                return View();
            }
            ViewData["error"] = "Bitte melden sie sich am System an";
            return Redirect("/Home/");
        }

        public bool IsAuthorized()
        {
            return Session["User"] != null && Session["Role"].ToString() == "Teacher";
        }
    }
}
