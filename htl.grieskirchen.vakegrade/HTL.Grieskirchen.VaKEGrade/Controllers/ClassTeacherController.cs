using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Trirand.Web.Mvc;
using HTL.Grieskirchen.VaKEGrade.Models;
using HTL.Grieskirchen.VaKEGrade.Database;
using iTextSharp.text;
using iTextSharp.text.pdf;
using HTL.Grieskirchen.VaKEGrade.Utility;
using System.IO;

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
        //    ordersGrid.EditUrl = Url.Action("EditRowInline_EditRow");
                //GenerateGrids();
                return View(classes);
            }
            ViewData["error"] = "Bitte melden sie sich am System an";
            return Redirect("/Home/");
        }

        /// <summary>
        /// Checks if the current user is authorized
        /// </summary>
        /// <returns>True if the user is authorized, otherwise returns false</returns>
        public bool IsAuthorized()
        {
            return Session["User"] != null && Session["Role"].ToString() == "ClassTeacher";
        }

        [HttpPost]
        public JsonResult RetrieveAllStudents() {
            if (IsAuthorized()) {
                Teacher user = VaKEGradeRepository.Instance.GetTeacher(((Teacher)Session["User"]).ID);
                Session["User"] = user;
                List<Database.Pupil> pupils = user.PrimaryClasses.First().Pupils.ToList();
                
                GridData gData = new GridData() { page = 1 };
                List<RowData> rows = new List<RowData>();

                foreach (Database.Pupil pupil in pupils)
                {
                    rows.Add(new RowData() { id = pupil.ID, cell = new string[] { pupil.LastName, pupil.FirstName, pupil.Religion, pupil.Birthdate.ToShortDateString(), pupil.Gender } });
                }

                gData.records = rows.Count();
                gData.total = rows.Count();
                gData.rows = rows.ToArray();
                return Json(gData, JsonRequestBehavior.AllowGet);
            }
            return null;
        }

        [HttpPost]
        public string RetrieveAllStudentsHTML()
        {
            if (IsAuthorized())
            {
                Teacher user = VaKEGradeRepository.Instance.GetTeacher(((Teacher)Session["User"]).ID);
                Session["User"] = user;
                List<Database.Pupil> pupils = user.PrimaryClasses.First().Pupils.ToList();

                string html = "";
                foreach (Pupil pupil in pupils) {
                    html += "<tr><td><input id=\"chkPrint" + pupil.ID + "\" type=\"checkbox\" value=\""+pupil.ID+"\"/></td><td>"+pupil.LastName + " " + pupil.FirstName + "</td></tr>"; 
                }
                return html;
            }
            return null;
        }

        /// <summary>
        /// Returns all SPFs of a student
        /// </summary>
        /// <param name="parentRowID">The ID of the student</param>
        /// <returns>A JsonResult containing the student's SPFs</returns>
        [HttpPost]
        public JsonResult RetrieveSPFs(int pupilID)
        {
            if (IsAuthorized())
            {
                Teacher user = VaKEGradeRepository.Instance.GetTeacher(((Teacher)Session["User"]).ID);
                Session["User"] = user;
                Pupil pupil = VaKEGradeRepository.Instance.GetPupil(pupilID);
                //Session["SelPupilID"] = pupil.ID;
                if (pupil != null)
                {
                    List<Database.SPF> spfs = pupil.SPFs.ToList();
                    GridData gData = new GridData() { page = 1 };
                    List<RowData> rows = new List<RowData>();
                    Session["pupilID"] = pupilID;

                    foreach (SPF spf in spfs)
                    {
                        rows.Add(new RowData() { id = spf.ID, cell = new string[] { spf.Subject.Name, spf.Level.ToString() } });
                    }

                    gData.records = rows.Count();
                    gData.total = rows.Count();
                    gData.rows = rows.ToArray();

                    return Json(gData, JsonRequestBehavior.AllowGet);
                }
                return null;
            }
            return null;
        }

        public string RetrieveAllSubjects()
        {
            if (IsAuthorized())
            {
                Teacher user = VaKEGradeRepository.Instance.GetTeacher(((Teacher)Session["User"]).ID);
                Session["User"] = user;
                List<Subject> subjects = VaKEGradeRepository.Instance.GetSubjectsOfClass(user.PrimaryClasses.First()).ToList();
                string content = "<select>";
                foreach (Subject subject in subjects)
                {
                   content += "<option value='"+subject.ID+"'>"+subject.Name+"</option>";
                }
                content += "</select>";
                return content;
            }
            ViewData["error"] = "Bitte melden sie sich am System an";
            return null;
        }

        /// <summary>
        /// Method called by jqGrid to add/update/delete a student
        /// </summary>
        /// <param name="editedPupil">The student to be added/updated/deleted</param>
        public void EditStudent(Database.Pupil editedPupil)
        {
            if (IsAuthorized())
            {
                //var pupilModel = GenerateGrids().PupilGrid;
                string oper = Request.Params.Get("oper");
                if (oper == "edit")
                {
                    Database.Pupil pupilToUpdate = Database.VaKEGradeRepository.Instance.GetPupil(editedPupil.ID);

                    pupilToUpdate.FirstName = editedPupil.FirstName;
                    pupilToUpdate.LastName = editedPupil.LastName;
                    pupilToUpdate.Birthdate = editedPupil.Birthdate;
                    pupilToUpdate.Religion = editedPupil.Religion;
                    pupilToUpdate.Gender = editedPupil.Gender;

                    Database.VaKEGradeRepository.Instance.Update();
                }
                if (oper == "add")
                {
                    Pupil newPupil = new Pupil();
                    newPupil.FirstName = editedPupil.FirstName;
                    newPupil.LastName = editedPupil.LastName;
                    newPupil.Religion = editedPupil.Religion;
                    newPupil.Birthdate = editedPupil.Birthdate;
                    newPupil.Gender = editedPupil.Gender;
                    newPupil.SchoolClass = ((Teacher)Session["User"]).PrimaryClasses.First();
                    VaKEGradeRepository.Instance.AddPupil(newPupil);
                }
                if (oper == "del")
                {
                    VaKEGradeRepository.Instance.DeletePupil(editedPupil.ID);
                }
            }
        }


        /// <summary>
        /// Method called by the SPFGrid to add/update/delete an SPF
        /// </summary>
        /// <param name="editedSPF">The SPF to be added/updated/deleted</param>
        public void EditSPF(SPF editedSPF, Pupil pupil, string extraparam)
        {
            string oper = Request.Params.Get("oper");
            string pupilID = Session["pupilID"].ToString();
            if (IsAuthorized())
            {
                //var spfModel = GenerateGrids().SpfGrid;

                if (oper == "edit")
                {
                    SPF spfToUpdate = VaKEGradeRepository.Instance.GetSPF(editedSPF.ID);

                    spfToUpdate.SubjectID = editedSPF.SubjectID;
                   
                    spfToUpdate.Level = editedSPF.Level;

                    VaKEGradeRepository.Instance.Update();
                }
                if (oper == "add")
                {
                     VaKEGradeRepository.Instance.AssignSPF(Convert.ToInt32(pupilID), Convert.ToInt32(editedSPF.SubjectID), editedSPF.Level);
                }
                if (oper == "del")
                {
                    VaKEGradeRepository.Instance.DeleteSPF(editedSPF.ID);
                }
            }
        }

        public FileStreamResult GenerateCertificates()
        {

            Teacher teacher = VaKEGradeRepository.Instance.GetTeacher(((Teacher)Session["User"]).ID);
            Session["User"] = teacher;
            SchoolClass schoolClass = teacher.PrimaryClasses.First();
            string schoolYear;
            if (DateTime.Now.Month <= 8)
            {
                schoolYear = DateTime.Now.AddYears(-1).Year + "/" + DateTime.Now.Year;
            }
            else
            {
                schoolYear = DateTime.Now.Year + "/" + DateTime.Now.AddYears(1).Year;
            }
            HttpContext.Response.AddHeader("content-disposition",
            "attachment; filename=Zeugnisse_"+ schoolYear +"_"+ schoolClass.Level + schoolClass.Name +".pdf");
                        
            return new FileStreamResult(CertificateGenerator.GeneratePDF(teacher, schoolClass, schoolYear), "application/pdf");
        }


        public FileStreamResult GenerateSpecificCertificates(string studentIds)
        {
            string[] ids = studentIds.Split(new string[]{","}, StringSplitOptions.RemoveEmptyEntries);

            List<Pupil> pupils = new List<Pupil>();
            foreach (string id in ids) {
                pupils.Add(VaKEGradeRepository.Instance.GetPupil(int.Parse(id)));
            }

            Teacher teacher = VaKEGradeRepository.Instance.GetTeacher(((Teacher)Session["User"]).ID);
            Session["User"] = teacher;
            SchoolClass schoolClass = teacher.PrimaryClasses.First();
            string schoolYear;
            if (DateTime.Now.Month <= 8)
            {
                schoolYear = DateTime.Now.AddYears(-1).Year + "/" + DateTime.Now.Year;
            }
            else {
                schoolYear = DateTime.Now.Year + "/" + DateTime.Now.AddYears(1).Year;
            };
            HttpContext.Response.AddHeader("content-disposition",
            "attachment; filename=Zeugnisse_" + schoolYear + "_" + studentIds.Remove(studentIds.Length-1).Replace(',','_')+ ".pdf");

            return new FileStreamResult(CertificateGenerator.GeneratePDF(teacher, schoolClass, pupils, schoolYear), "application/pdf");
        }
    }
}
