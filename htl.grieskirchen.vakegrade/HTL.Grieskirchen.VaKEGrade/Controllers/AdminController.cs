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

                        foreach (string temp in data)
                        {
                            if (temp != "")
                            {
                                string[] pupil = temp.Split(';');
                                int classID = -1;
                                foreach (Database.SchoolClass curClass in classes)
                                {
                                    if (curClass.Level + "" + curClass.Name == pupil[2])
                                    {
                                        classID = curClass.ID;
                                        break;
                                    }
                                }
                                if (classID == -1)
                                {
                                    Database.SchoolClass newClass = new Database.SchoolClass();
                                    newClass.Name = pupil[2].Substring(1, 1);
                                    newClass.Level = Convert.ToInt32(pupil[2].Substring(0, 1));


                                    VaKEGrade.Database.VaKEGradeRepository.Instance.AddClass(newClass);
                                    classID = newClass.ID;

                                }

                                VaKEGrade.Database.VaKEGradeRepository.Instance.AddPupil(new Database.Pupil() { FirstName = pupil[0], LastName = pupil[1], Birthdate = Convert.ToDateTime(pupil[4]), ClassID = classID, Religion = pupil[3], Gender = pupil[5] });
                            }
                        }
                    }
                }
                return RedirectToAction("Index");

            }
            ViewData["error"] = "Bitte melden sie sich am System an";
            return Redirect("/Home/");

        }




        public bool IsAuthorized()
        {
            //return Session["User"] != null && Session["Role"].ToString() == "Admin";
            return true;
        }


        public JsonResult RetrieveAllStudents()
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


        public JsonResult RetrieveAllClasses()
        {

            if (IsAuthorized())
            {
                List<Database.SchoolClass> classes = VaKEGrade.Database.VaKEGradeRepository.Instance.GetClasses().ToList<Database.SchoolClass>();
                GridData gData = new GridData() { page = 1 };
                List<RowData> rows = new List<RowData>();

                foreach (Database.SchoolClass curClass in classes)
                {
                    rows.Add(new RowData() { id = curClass.ID, cell = new string[] { curClass.Level + "" + curClass.Name } });
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

        public JsonResult RetrieveClassList()
        {
            List<Database.SchoolClass> classes = VaKEGrade.Database.VaKEGradeRepository.Instance.GetClasses().ToList<Database.SchoolClass>();
            GridData gData = new GridData() { page = 1 };
            List<RowData> rows = new List<RowData>();

            foreach (Database.SchoolClass sclass in classes)
            {
                rows.Add(new RowData() { id = sclass.ID, cell = new string[] { sclass.Level.ToString(), sclass.Name, sclass.Teacher.LastName } });
            }

            gData.records = rows.Count();
            gData.total = rows.Count();
            gData.rows = rows.ToArray();

            JsonResult jres = Json(gData, JsonRequestBehavior.AllowGet);

            return jres;

        }

        public string AddEditClass(int level, string label, string teacher, string branch)
        {
            Database.SchoolClass newclass = new Database.SchoolClass();
            newclass.Level = level;
            newclass.Name = label;
            string log = null;

            string namefirst = teacher.Split(' ')[0];
            string namesec = teacher.Split(' ')[1];

            var teachers = VaKEGrade.Database.VaKEGradeRepository.Instance.GetTeachers();

            newclass.PrimaryClassTeacherID = (from c in teachers
                                              where (c.LastName == namefirst && c.FirstName == namesec) || (c.LastName == namesec && c.FirstName == namefirst)
                                              select c.ID).First();



            var branches = VaKEGrade.Database.VaKEGradeRepository.Instance.GetBranches();
            newclass.BranchID = (from c in branches
                                 where c.Name == branch
                                 select c.ID).First();
            if (newclass.BranchID == null)
            {
                log = "Der Angegebene Zweig existiert nicht";
            }

            VaKEGrade.Database.VaKEGradeRepository.Instance.AddClass(newclass);

            return log;
        }



        public JsonResult RetrieveAllSubjects()
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

        public JsonResult RetrieveBranchList()
        {
            if (IsAuthorized())
            {
                //List<Database.Branch> braches = VaKEGrade.Database.VaKEGradeRepository.Instance.GetBranches().ToList<Database.Branch>();
                //GridData gData = new GridData() { page = 1 };
                //List<RowData> rows = new List<RowData>();



                //foreachn (Database.Branch branch in branches)
                //{
                //    List<Database.Subject> subjects = branch.BranchSubjectAssignments.Select(x => x.Subject).Distinct().ToList();
                //    //rows.Add(new RowData() { id = branch.ID, cell = new string[] { branch., pupil.FirstName, pupil.Birthdate.ToString(), pupil.Gender } });
                //}

                //gData.records = rows.Count();
                //gData.total = rows.Count();
                //gData.rows = rows.ToArray();

                //JsonResult jres = Json(gData, JsonRequestBehavior.AllowGet);

                //return jres;

                List<Database.Branch> branches = VaKEGrade.Database.VaKEGradeRepository.Instance.GetBranches().ToList<Database.Branch>();


                List<string[]> res = new List<string[]>();

                foreach (Database.Branch branch in branches)
                {
                    List<Database.Subject> subjects = branch.BranchSubjectAssignments.Select(x => x.Subject).Distinct().ToList();
                    
                    List<string> buf = new List<string>();
                    buf.Add(branch.Name);
                    buf.Add(branch.ID.ToString());

                    foreach (Database.Subject sub in subjects) { 
                        buf.Add(sub.Name);
                    }

                    res.Add(buf.ToArray<string>());
                }

                JsonResult jres = Json(res.ToArray(), JsonRequestBehavior.AllowGet);

                return jres;

            }
            ViewData["error"] = "Bitte melden sie sich am System an";
            return null;
        }


        public JsonResult RetrieveSubjects()
        {
            if (IsAuthorized())
            {
                List<Database.Subject> subjects = VaKEGrade.Database.VaKEGradeRepository.Instance.GetSubjects().ToList<Database.Subject>();
                GridData gData = new GridData() { page = 1 };
                List<RowData> rows = new List<RowData>();



                foreach (Database.Subject subject in subjects)
                {
                    //List<Database.Subject> subjects = branch.BranchSubjectAssignments.Select(x => x.Subject).Distinct().ToList();
                    rows.Add(new RowData() { id = subject.ID, cell = new string[] { subject.Name, subject.IsVoluntary.ToString(), subject.SPFs.Count.ToString() } });
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