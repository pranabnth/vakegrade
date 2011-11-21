using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.IO;
using HTL.Grieskirchen.VaKEGrade.Utility;


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


                        string s = System.Text.UnicodeEncoding.UTF8.GetString(config);
                        string[] data = s.Split(Environment.NewLine.ToCharArray());
                        List<Database.SchoolClass> classes = VaKEGrade.Database.VaKEGradeRepository.Instance.GetClasses().ToList<Database.SchoolClass>();

                        foreach (string temp in data) {
                            if (temp != "") {
                                string[] pupil = temp.Split(';');
                                int classID = -1;
                                foreach (Database.SchoolClass curClass in classes) {
                                    if (curClass.Level+""+curClass.Name == pupil[2])
                                    {
                                        classID = curClass.ID;
                                        break;
                                    }
                                }
                                if(classID == -1){
                                   Database.SchoolClass newClass = new Database.SchoolClass();
                                   newClass.Name = pupil[2].Substring(1, 1);
                                   newClass.Level = Convert.ToInt32(pupil[2].Substring(0,1));
                                   

                                   VaKEGrade.Database.VaKEGradeRepository.Instance.AddClass(newClass);
                                    classID = newClass.ID;
                                   
                                }

                                VaKEGrade.Database.VaKEGradeRepository.Instance.AddPupil(new Database.Pupil(){FirstName = pupil[0], LastName = pupil[1], Birthdate = Convert.ToDateTime(pupil[4]), ClassID = classID, Religion = pupil[3], Gender=pupil[5]});
                            }
                        }
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
                List<Database.Pupil> pupils = VaKEGrade.Database.VaKEGradeRepository.Instance.GetPupils().ToList<Database.Pupil>();
                GridData gData = new GridData(){page = 1};
                List<RowData> rows = new List<RowData>();

                foreach (Database.Pupil pupil in pupils)
                {
                    rows.Add(new RowData(){id = pupil.ID, cell = new string[]{pupil.LastName, pupil.FirstName, pupil.Birthdate.ToString(), pupil.Gender}});
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


    }

    

    
}
