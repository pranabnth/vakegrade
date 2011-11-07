using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using HTL.Grieskirchen.VaKEGrade.Database;

namespace HTL.Grieskirchen.VaKEGrade.Controllers
{
    public class AuthentificationController : Controller
    {
        //
        // GET: /Authentification/
      

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Login(FormCollection fc) {
            string username = fc.Get("tbUsername");
            string password = fc.Get("tbPassword");
            string url = "";

            VaKEGradeRepository repository = new VaKEGradeRepository();
            Teacher teacher = repository.GetTeacher(username, password);
            if (teacher != null)
            {
                Session["User"] = teacher;
                if (username.ToLower() == "admin") {
                    Session["Role"] = "Admin";
                    url = "/Admin/"; 
                }
                else if (repository.IsUserClassTeacher(teacher))
                {
                    Session["Role"] = "ClassTeacher";
                    url = "/ClassTeacher/";
                }
                else {
                    Session["Role"] = "Teacher";
                    url = "/Teacher/";
                }               
            }
            
            return Redirect(url);
        }

        public ActionResult Logout() {
            Session["User"] = null;
            return Redirect("/Home/");
        }

    }
}
