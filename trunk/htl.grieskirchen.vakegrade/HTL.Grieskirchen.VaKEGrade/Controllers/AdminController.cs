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
            
            return View();
        }

        [HttpPost]
        public ActionResult RecieveStudentConfig() 
        {
            foreach (string inputTagName in Request.Files)
            {
                HttpPostedFileBase file = Request.Files[inputTagName];
                if (file.ContentLength > 0)
                {
                    
                    byte[] config = new byte[file.ContentLength];
                    file.InputStream.Read(config, 0, file.ContentLength);

                    
                    String s = System.Text.UnicodeEncoding.UTF8.GetString(config);
                }
            }

            return RedirectToAction("Index");
    
        }
    }
}
