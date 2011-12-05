using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using HTL.Grieskirchen.VaKEGrade.Utility;

namespace HTL.Grieskirchen.VaKEGrade.Controllers
{
    public class TeacherController : Controller
    {
        //
        // GET: /Teacher/

        public ActionResult Index()
        {
            if (IsAuthorized())
            {

                List<Database.SchoolClass> classes = Database.VaKEGradeRepository.Instance.GetClassesOfTeacher((Database.Teacher)Session["User"]).ToList();
                return View(classes);
            }
            ViewData["error"] = "Bitte melden sie sich am System an";
            return Redirect("/Home/");
        }

        public bool IsAuthorized()
        {
            return Session["User"] != null && Session["Role"].ToString() == "Teacher";
            //return true;
        }

        public JsonResult RetrieveClasses()
        {
            return null;
            //if (IsAuthorized())
            //{
            //    //List<Database.SchoolClass> classes = VaKEGrade.Database.VaKEGradeRepository.Instance.GetClasses().ToList<Database.SchoolClass>();
            //    GridData gData = new GridData() { page = 1 };
            //    List<RowData> rows = new List<RowData>();
            //    Database.SchoolClass schoolClassTemp;

            //    foreach (Database.TeacherSchoolClassAssignment teacher_class in ((Database.Teacher)Session["User"]).TeacherSchoolClassAssignments)
            //    {
            //        schoolClassTemp = teacher_class.SchoolClass;
            //        rows.Add(new RowData() { id = schoolClassTemp.ID, cell = new string[] { schoolClassTemp.Level.ToString(), schoolClassTemp.Name } });
            //    }

            //    gData.records = rows.Count();
            //    gData.total = rows.Count();
            //    gData.rows = rows.ToArray();

            //    JsonResult jres = Json(gData, JsonRequestBehavior.AllowGet);

            //    return jres;
            //}
            //ViewData["error"] = "Bitte melden sie sich am System an";
            //return null;
        }

        public JsonResult RetrieveGradeData()
        {
            if (IsAuthorized())
            {
                List<Database.Pupil> pupils = VaKEGrade.Database.VaKEGradeRepository.Instance.GetPupils().ToList<Database.Pupil>();
                GridData gData = new GridData() { page = 1 };
                List<RowData> rows = new List<RowData>();

                foreach (Database.Pupil pupil in pupils)
                {
                    rows.Add(new RowData() { id = pupil.ID, cell = new string[] { pupil.LastName, pupil.FirstName, pupil.Birthdate.ToString(), pupil.Gender } });
                }

                gData.records = rows.Count();
                gData.total = rows.Count();
                gData.rows = rows.ToArray();

                JsonResult jres = Json(gData, JsonRequestBehavior.AllowGet);

                return jres;
            }
            ViewData["error"] = "Bitte melden sie sich am System an";
            return null;
        }

        public JsonResult RetrieveSubjects()
        {
            if (IsAuthorized())
            {
                List<Database.Pupil> pupils = VaKEGrade.Database.VaKEGradeRepository.Instance.GetPupils().ToList<Database.Pupil>();
                GridData gData = new GridData() { page = 1 };
                List<RowData> rows = new List<RowData>();

                foreach (Database.Pupil pupil in pupils)
                {
                    rows.Add(new RowData() { id = pupil.ID, cell = new string[] { pupil.LastName, pupil.FirstName, pupil.Birthdate.ToString(), pupil.Gender } });
                }

                gData.records = rows.Count();
                gData.total = rows.Count();
                gData.rows = rows.ToArray();

                JsonResult jres = Json(gData, JsonRequestBehavior.AllowGet);

                return jres;
            }
            ViewData["error"] = "Bitte melden sie sich am System an";
            return null;
        }

        public JsonResult RetrieveSubjectsOfClass(int classID)
        {
            return null;
            //if (IsAuthorized())
            //{
            //    List<Database.Subject> subjects = ((Database.Subject)Session["User"]).TeacherSubjectAssignments;
            //    GridData gData = new GridData() { page = 1 };
            //    List<RowData> rows = new List<RowData>();

            //    foreach (Database.Pupil pupil in pupils)
            //    {
            //        rows.Add(new RowData() { id = pupil.ID, cell = new string[] { pupil.LastName, pupil.FirstName, pupil.Birthdate.ToString(), pupil.Gender } });
            //    }

            //    gData.records = rows.Count();
            //    gData.total = rows.Count();
            //    gData.rows = rows.ToArray();

            //    JsonResult jres = Json(gData, JsonRequestBehavior.AllowGet);

            //    return jres;
            //}
            //ViewData["error"] = "Bitte melden sie sich am System an";
            //return null;
        }
    }

}
