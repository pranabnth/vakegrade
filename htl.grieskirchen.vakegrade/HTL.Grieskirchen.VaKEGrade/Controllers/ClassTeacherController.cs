using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Trirand.Web.Mvc;
using HTL.Grieskirchen.VaKEGrade.Models;
using HTL.Grieskirchen.VaKEGrade.Database;

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
                GeneratePupilGrid();
                return View(classes);
            }
            ViewData["error"] = "Bitte melden sie sich am System an";
            return Redirect("/Home/");
        }

        public GridModel GeneratePupilGrid() {
            GridModel model = new GridModel();

            model.PupilGrid.DataUrl = Url.Action("RetrieveAllStudents");
            model.PupilGrid.EditUrl = Url.Action("EditStudent");
            model.PupilGrid.ClientSideEvents.RowSelect = "editRow";
            model.PupilGrid.ID = "PupilsGrid";

            model.PupilGrid.HierarchySettings.HierarchyMode = HierarchyMode.Parent;
            model.PupilGrid.HierarchySettings.ReloadOnExpand = true;
            model.PupilGrid.HierarchySettings.SelectOnExpand = true;
            model.PupilGrid.HierarchySettings.ExpandOnLoad = true;
            model.PupilGrid.HierarchySettings.PlusIcon = "ui-icon-plus";
            model.PupilGrid.HierarchySettings.MinusIcon = "ui-icon-minus";
            model.PupilGrid.HierarchySettings.OpenIcon = "ui-icon-carat-1-sw";

            model.SpfGrid.ID = "SPFGrid";
            model.SpfGrid.DataUrl = Url.Action("RetrieveSPFs");
            model.SpfGrid.HierarchySettings.HierarchyMode = HierarchyMode.Child;


            Session["PupilGModel"] = model.PupilGrid;
            Session["SPFGModel"] = model.SpfGrid;
            return model;
        }

        public bool IsAuthorized()
        {
            return Session["User"] != null && Session["Role"].ToString() == "ClassTeacher";
        }

        public JsonResult RetrieveAllStudents() {
            if (IsAuthorized()) {
                Teacher user = VaKEGradeRepository.Instance.GetTeacher(((Teacher)Session["User"]).ID);
                Session["User"] = user;
                Database.SchoolClass schoolClass = user.PrimaryClasses.First();
                return GeneratePupilGrid().PupilGrid.DataBind(schoolClass.Pupils.AsQueryable());
                
            }
            return null;
        }

        public JsonResult RetrieveSPFs(string parentRowID)
        {
            if (IsAuthorized())
            {
                Teacher user = VaKEGradeRepository.Instance.GetTeacher(((Teacher)Session["User"]).ID);
                Session["User"] = user;
                return GeneratePupilGrid().SpfGrid.DataBind(VaKEGradeRepository.Instance.GetFormattedSPFs(Convert.ToInt32(parentRowID)));
            }
            return null;
        }

        public void EditStudent(Database.Pupil editedPupil)
        {           
            var pupilModel = GeneratePupilGrid().PupilGrid;

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
                // since we are adding a new Order, create a new istance
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
}
