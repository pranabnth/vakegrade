using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using HTL.Grieskirchen.VaKEGrade.Utility;
using Trirand.Web.Mvc;
using HTL.Grieskirchen.VaKEGrade.Models;

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
                
               
                return View();
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

            
            List<Database.SchoolClass> classes = new List<Database.SchoolClass>();
            List<string[]> res = new List<string[]>();
                bool existing;
                foreach (Database.TeacherSubjectAssignment teacher_class in ((Database.Teacher)Session["User"]).TeacherSubjectAssignments)
                {
                    existing = false;
                    foreach (Database.SchoolClass toComp in classes)
                    {
                        if (toComp.ID == teacher_class.SchoolClass.ID)
                        {
                            existing = true;
                            break;
                        }


                    }
                    if (!existing)
                    {
                        classes.Add(teacher_class.SchoolClass);
                        res.Add(new string[] { teacher_class.SchoolClass.Level + "" + teacher_class.SchoolClass.Name, teacher_class.SchoolClass.ID.ToString() });
                    }


                }
                    

            JsonResult jres = Json(res.ToArray(), JsonRequestBehavior.AllowGet);

            return jres;
          
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
            
            List<Database.Subject> subjects = Database.VaKEGradeRepository.Instance.GetSubjectsOfTeacher(((Database.Teacher)Session["user"]).ID, classID).ToList<Database.Subject>();

            
            List<string[]> res = new List<string[]>();

            foreach (Database.Subject sub in subjects) {

                res.Add(new string[]{sub.Name,sub.ID.ToString()});
            }

            JsonResult jres = Json(res.ToArray(), JsonRequestBehavior.AllowGet);

            return jres;
        }


        [HttpPost]
        public JsonResult GenerateGradeGrid(int subjectID, int classID)
        {
            JQGridConf config = new JQGridConf();
            
            if (subjectID != -1)
            {

                List<Database.SubjectArea> subjectAreas = Database.VaKEGradeRepository.Instance.GetSubject(subjectID).SubjectAreas.ToList<Database.SubjectArea>();

                
                foreach (Database.SubjectArea subAr in subjectAreas)
                {
                    config.ColNames.Add(subAr.Name);
                    config.ColModel.Add(new Column() { Name = subAr.Name, Index = subAr.Name, Align = "left", Width = 40 });
                }

            }

            return Json(config);
        }


        public JsonResult RetrieveGradeData()
        {
            if (IsAuthorized())
            {
                return Json("bbbrr");
            }
            return null;
        }
    }

}
