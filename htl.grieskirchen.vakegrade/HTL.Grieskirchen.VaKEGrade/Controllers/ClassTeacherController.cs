using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Trirand.Web.Mvc;

namespace HTL.Grieskirchen.VaKEGrade.Controllers
{
    public class ClassTeacherController : Controller
    {
        //
        // GET: /ClassTeacher/
       
        public ActionResult Index()
        {
            
            if (Session["User"] != null)
            {
                Database.Teacher teacher = (Database.Teacher)Session["User"];
                Utility.GridData gData = new Utility.GridData();
                List<Database.SchoolClass> classes = Database.VaKEGradeRepository.Instance.GetClassesOfTeacher(teacher).ToList();
              
                return View(classes);
            }
            ViewData["error"] = "Bitte melden sie sich am System an";
            return Redirect("/Home/");
        }

        public bool IsAuthorized()
        {
            return Session["User"] != null && Session["Role"].ToString() == "ClassTeacher";
        }

        public JsonResult RetrieveAllStudents() {
            if (IsAuthorized()) {

                Database.SchoolClass schoolClass = ((Database.Teacher)Session["User"]).PrimaryClasses.First();
                Utility.GridData gData = new Utility.GridData();
                
                List<Utility.RowData> rows = new List<Utility.RowData>();

                foreach (Database.Pupil pupil in schoolClass.Pupils.OrderBy(x => x.LastName))
                {
                    rows.Add(new Utility.RowData() { id = pupil.ID, cell = new string[] { pupil.LastName, pupil.FirstName, pupil.Religion, pupil.Birthdate.ToString(), pupil.Gender } });
                }

                gData.records = rows.Count();
                gData.total = rows.Count();
                gData.rows = rows.ToArray();
                 
                JsonResult jres = Json(gData, JsonRequestBehavior.AllowGet);
                
                return jres;
            }
            return null;
        }

    }
}
