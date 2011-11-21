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
            if (IsAuthorized())
            {
                return View();
                
            }
            ViewData["error"] = "Bitte melden sie sich am System an";
            return Redirect("/Home/");
        }

        [HttpPost]
        public ActionResult RecieveStudentConfig() 
        {
            if (IsAuthorized())
            {
                foreach (string inputTagName in Request.Files)
                {
                    HttpPostedFileBase file = Request.Files[inputTagName];
                    if (file.ContentLength > 0)
                    {

                        byte[] config = new byte[file.ContentLength];
                        file.InputStream.Read(config, 0, file.ContentLength);


                        String s = System.Text.UnicodeEncoding.UTF8.GetString(config);
                        string[] data = s.Split(Environment.NewLine.ToCharArray());
                    }
                }
                return RedirectToAction("Index");
                
            }
            ViewData["error"] = "Bitte melden sie sich am System an";
            return Redirect("/Home/");
            
        }


            

        public bool IsAuthorized() {
            return Session["User"] != null && Session["Role"].ToString() == "Admin";
            
        }


        public JsonResult RetrieveAllStudents()
        {
            if (IsAuthorized())
            {
            JsonResult jres = Json(new { }, JsonRequestBehavior.AllowGet);

            return jres;
            }
            ViewData["error"] = "Bitte melden sie sich am System an";
            return null;
        }


    }

    
}
