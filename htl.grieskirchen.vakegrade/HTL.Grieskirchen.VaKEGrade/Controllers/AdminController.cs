using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.IO;

namespace HTL.Grieskirchen.VaKEGrade.Controllers
{
    public class AdminController : Controller
    {
        //
        // GET: /Admin/

        public ActionResult Index()
        {
            if (IsAuthorized()) {

                return View();
            }
            ViewData["error"] = "Bitte melden sie sich am System an";
            return Redirect("/Home/");
        }

        public bool IsAuthorized() {
            return Session["User"] != null && Session["Role"].ToString() == "Admin";
        }

        
    }
}
