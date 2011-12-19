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
                GenerateGrids();
                return View(classes);
            }
            ViewData["error"] = "Bitte melden sie sich am System an";
            return Redirect("/Home/");
        }


        /// <summary>
        /// Generates all needed grids
        /// </summary>
        /// <returns>Returns a GridModel containing the needed Grids</returns>
        public GridModel GenerateGrids() {
            GridModel model = new GridModel();

            model.PupilGrid.DataUrl = Url.Action("RetrieveAllStudents");
            model.PupilGrid.EditUrl = Url.Action("EditStudent");
            model.PupilGrid.ClientSideEvents.RowSelect = "editPupilRow";
            model.PupilGrid.ClientSideEvents.SubGridRowExpanded = "showSPFSubGrid";

            //model.PupilGrid.ID = "PupilsGrid";

            model.PupilGrid.HierarchySettings.HierarchyMode = HierarchyMode.Parent;
            model.PupilGrid.HierarchySettings.ReloadOnExpand = true;
            model.PupilGrid.HierarchySettings.SelectOnExpand = true;
            model.PupilGrid.HierarchySettings.ExpandOnLoad = false;
            model.PupilGrid.HierarchySettings.PlusIcon = "ui-icon-plus";
            model.PupilGrid.HierarchySettings.MinusIcon = "ui-icon-minus";
            model.PupilGrid.HierarchySettings.OpenIcon = "ui-icon-carat-1-sw";

            //model.SpfGrid.ID = "SPFGrid";
            model.SpfGrid.DataUrl = Url.Action("RetrieveSPFs");
            model.SpfGrid.EditUrl = Url.Action("EditSPF");
            model.SpfGrid.ClientSideEvents.RowSelect = "editSPFRow";
            model.SpfGrid.HierarchySettings.HierarchyMode = HierarchyMode.Child;


            Session["PupilGModel"] = model.PupilGrid;
            Session["SPFGModel"] = model.SpfGrid;
            return model;
        }

        /// <summary>
        /// Checks if the current user is authorized
        /// </summary>
        /// <returns>True if the user is authorized, otherwise returns false</returns>
        public bool IsAuthorized()
        {
            return Session["User"] != null && Session["Role"].ToString() == "ClassTeacher";
        }

        public JsonResult RetrieveAllStudents() {
            if (IsAuthorized()) {
                Teacher user = VaKEGradeRepository.Instance.GetTeacher(((Teacher)Session["User"]).ID);
                Session["User"] = user;
                Database.SchoolClass schoolClass = user.PrimaryClasses.First();
                return GenerateGrids().PupilGrid.DataBind(schoolClass.Pupils.AsQueryable());
                
            }
            return null;
        }

        /// <summary>
        /// Returns all SPFs of a student
        /// </summary>
        /// <param name="parentRowID">The ID of the student</param>
        /// <returns>A JsonResult containing the student's SPFs</returns>
        public JsonResult RetrieveSPFs(string parentRowID)
        {
            if (IsAuthorized())
            {
                Teacher user = VaKEGradeRepository.Instance.GetTeacher(((Teacher)Session["User"]).ID);
                Session["User"] = user;
                Pupil pupil = VaKEGradeRepository.Instance.GetPupil(Convert.ToInt32(parentRowID));
                Session["SelPupilID"] = pupil.ID;
                return GenerateGrids().SpfGrid.DataBind(VaKEGradeRepository.Instance.GetFormattedSPFs(Convert.ToInt32(parentRowID)));
            }
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
                var pupilModel = GenerateGrids().PupilGrid;

                if (pupilModel.AjaxCallBackMode == AjaxCallBackMode.EditRow)
                {
                    Database.Pupil pupilToUpdate = Database.VaKEGradeRepository.Instance.GetPupil(editedPupil.ID);

                    pupilToUpdate.FirstName = editedPupil.FirstName;
                    pupilToUpdate.LastName = editedPupil.LastName;
                    pupilToUpdate.Birthdate = editedPupil.Birthdate;
                    pupilToUpdate.Religion = editedPupil.Religion;
                    pupilToUpdate.Gender = editedPupil.Gender;

                    Database.VaKEGradeRepository.Instance.Update();
                }
                if (pupilModel.AjaxCallBackMode == AjaxCallBackMode.AddRow)
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
                if (pupilModel.AjaxCallBackMode == AjaxCallBackMode.DeleteRow)
                {
                    VaKEGradeRepository.Instance.DeletePupil(editedPupil.ID);
                }
            }
        }
        /// <summary>
        /// Method called by the SPFGrid to add/update/delete an SPF
        /// </summary>
        /// <param name="editedSPF">The SPF to be added/updated/deleted</param>
        public void EditSPF(Utility.WebSPF editedSPF)
        {
            
            if (IsAuthorized())
            {
                var spfModel = GenerateGrids().SpfGrid;

                if (spfModel.AjaxCallBackMode == AjaxCallBackMode.EditRow)
                {
                    SPF spfToUpdate = VaKEGradeRepository.Instance.GetSPF(editedSPF.ID);

                    spfToUpdate.PupilID = editedSPF.PupilID;
                    spfToUpdate.SubjectID = Convert.ToInt32(editedSPF.SubjectName);
                    spfToUpdate.Level = editedSPF.Level;

                    VaKEGradeRepository.Instance.Update();
                }
                if (spfModel.AjaxCallBackMode == AjaxCallBackMode.AddRow)
                {
                     VaKEGradeRepository.Instance.AssignSPF(Convert.ToInt32(Session["SelPupilID"]), Convert.ToInt32(editedSPF.SubjectName), editedSPF.Level);
                }
                if (spfModel.AjaxCallBackMode == AjaxCallBackMode.DeleteRow)
                {
                    VaKEGradeRepository.Instance.DeleteSPF(editedSPF.ID);
                }
            }
        }

        public void GenerateCertificates() {

            Document document = new Document();
            FileStream stream = new FileStream("C:\\Users\\Philipp\\Desktop\\Certificates.pdf", FileMode.Create);
            PdfWriter writer = PdfWriter.GetInstance(document, stream);
            document.Open();
            document.AddCreationDate();
            Teacher teacher = VaKEGradeRepository.Instance.GetTeacher(((Teacher)Session["User"]).ID);
            Session["User"] = teacher;
            foreach (Pupil pupil in teacher.PrimaryClasses.First().Pupils) {
                document.Add(new Paragraph("Schüler: " + pupil.FirstName + " " + pupil.LastName));
                document.Add(ImageGenerator.GenerateImage(pupil));
            }

            document.Close();
        }
    }
}
