using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using HTL.Grieskirchen.VaKEGrade.Utility;
using Trirand.Web.Mvc;
using HTL.Grieskirchen.VaKEGrade.Models;
using HTL.Grieskirchen.VaKEGrade.Database;

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
            return Session["User"] != null && (Session["Role"].ToString() == "Teacher" || Session["Role"].ToString() == "ClassTeacher") ;
            
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
            //if (IsAuthorized())
            //{
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
            //}
            //ViewData["error"] = "Bitte melden sie sich am System an";
            //return null;
        }

        public JsonResult RetrieveSubjectsOfClass(int classID)
        {
            Teacher teacher = (Teacher)Session["user"];
            SchoolClass schoolClass = VaKEGradeRepository.Instance.GetClass(classID);
            List<Database.Subject> subjects;
            if (schoolClass.PrimaryClassTeacherID == teacher.ID) {
                subjects = VaKEGradeRepository.Instance.GetSubjectsOfClass(schoolClass).ToList();
            }
            else
            {
                subjects = Database.VaKEGradeRepository.Instance.GetSubjectsOfTeacher(teacher.ID, classID).ToList<Database.Subject>();
            }
            
            List<string[]> res = new List<string[]>();

            foreach (Database.Subject sub in subjects) {

                res.Add(new string[]{sub.Name,sub.ID.ToString()});
            }

            JsonResult jres = Json(res.ToArray(), JsonRequestBehavior.AllowGet);

            return jres;
        }

        [HttpPost]
        public JsonResult RetrieveSubArea(int classID, int subjectID)
        {
            List<String> subAreas = new List<string>();
            if (subjectID != -1)
            {

                List<Database.SubjectArea> subjectAreas = Database.VaKEGradeRepository.Instance.GetSubject(subjectID).SubjectAreas.ToList<Database.SubjectArea>();



                subAreas.Add("Name"); 
                foreach (Database.SubjectArea subAr in subjectAreas)
                {
                    
                    subAreas.Add(subAr.Name);
                  
                }

            }

            return Json(subAreas.ToArray());
        }

        [HttpPost]
        public JsonResult RetrieveGrades(int classID, int subjectID)
        {
            
            


            if (IsAuthorized())
            {
                List<Database.Pupil> pupils = VaKEGrade.Database.VaKEGradeRepository.Instance.GetPupils().ToList<Database.Pupil>();
                List<Database.SubjectArea> subjectAreas = Database.VaKEGradeRepository.Instance.GetSubject(subjectID).SubjectAreas.ToList<Database.SubjectArea>();


                List<string[][]> gradeData = new List<string[][]>();
                List<string[]> stud = new List<string[]>();
               

                foreach (Database.Pupil pupil in pupils)
                {
                    if (pupil.SchoolClass.ID == classID)
                    {
                        List<Database.Grade> grades = Database.VaKEGradeRepository.Instance.GetGradesOfPupil(pupil, Database.VaKEGradeRepository.Instance.GetSubject(subjectID)).ToList<Database.Grade>();
                        stud.Clear();

                        stud.Add(new string[]{ pupil.LastName + " " + pupil.FirstName });

                        

                        foreach (Database.Grade grade in grades)
                        {
                             stud.Add(new string[]{grade.Value.ToString(),pupil.ID.ToString(),grade.SubjectAreaID.ToString()});
                        }

                        gradeData.Add(stud.ToArray());
                    }
                }


                JsonResult jres = Json(gradeData);

                return jres;

            }
            return null;
        
        
        }


        //[HttpPost]
        //public JsonResult GenerateGradeGrid(int subjectID, int classID)
        //{
        //    JQGridConf config = new JQGridConf();
            
        //    if (subjectID != -1)
        //    {

        //        List<Database.SubjectArea> subjectAreas = Database.VaKEGradeRepository.Instance.GetSubject(subjectID).SubjectAreas.ToList<Database.SubjectArea>();


        //        config.ColNames.Add("Schüler");
        //        config.ColModel.Add(new Column() { name = "student", index = "student", align = "left", width = 150 });
                
        //        foreach (Database.SubjectArea subAr in subjectAreas)
        //        {
        //            config.ColNames.Add(subAr.Name);
        //            config.ColModel.Add(new Column() { name = subAr.Name, index = subAr.ID.ToString(), align = "left", width = 40, editable = true });
        //        }

        //    }

        //    return Json(config);
        //}


        //public JsonResult RetrieveGradeData(int classID, int subjectID)
        //{
        //    if (IsAuthorized())
        //    {
        //        List<Database.Pupil> pupils = VaKEGrade.Database.VaKEGradeRepository.Instance.GetPupils().ToList<Database.Pupil>();
        //        List<Database.SubjectArea> subjectAreas = Database.VaKEGradeRepository.Instance.GetSubject(subjectID).SubjectAreas.ToList<Database.SubjectArea>();
                

        //        GridData gData = new GridData() { page = 1 };
        //        List<object> rows = new List<object>();
        //        List<object> gradeStrings = new List<object>();

        //        foreach (Database.Pupil pupil in pupils)
        //        {
        //            if (pupil.SchoolClass.ID == classID)
        //            {
        //                List<Database.Grade> grades = Database.VaKEGradeRepository.Instance.GetGradesOfPupil(pupil,Database.VaKEGradeRepository.Instance.GetSubject(subjectID)).ToList<Database.Grade>();
        //                gradeStrings.Clear();

        //                gradeStrings.Add(new{student = pupil.LastName + " " + pupil.FirstName});

        //                foreach (Database.Grade grade in grades)
        //                {
        //                    gradeStrings.Add(grade.Value.ToString());
        //                }

        //                rows.Add(new { id = pupil.ID, cell = new{student = pupil.LastName + " " + pupil.FirstName} });
        //            }
        //        }

        //        gData.records = rows.Count();
        //        gData.total = rows.Count();
        //        gData.rows = rows.ToArray();

        //        JsonResult jres = Json(gData, JsonRequestBehavior.AllowGet);

        //        return jres;

        //    }
        //    return null;
        //}

        [HttpPost]
        public string EditGrades(int value, int studId, int arId)
        {
             
             Database.VaKEGradeRepository.Instance.AssignGrade(studId, arId, value);

             return "success";
        }
    }

}
