using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using HTL.Grieskirchen.VaKEGrade.Database.Exceptions;

namespace HTL.Grieskirchen.VaKEGrade.Database
{
    public class VaKEGradeRepository
    {
        VaKEGradeEntities entities;

        public VaKEGradeRepository()
        {
            entities = new VaKEGradeEntities();
        }

        // Verschachtelte Klasse für die verzögerte Instanziierung
        class SingletonCreator
        {
            static SingletonCreator() { }
            // Privates Objekt, das durch einen privaten Konstruktor instanziiert wird
            internal static readonly
            VaKEGradeRepository uniqueInstance = new VaKEGradeRepository();
        }

        // Öffentliche statische Eigenschaft, um das Objekt zu erhalten
        public static VaKEGradeRepository Instance
        {
            get { return SingletonCreator.uniqueInstance; }
        }

        /// <summary>
        /// Commits all changes to made to the database
        /// </summary>
        public void Update() {
            entities.SaveChanges();
        }

        #region Teacher
        /// <summary>
        /// Checks if the given username already exists.
        /// </summary>
        /// <param name="username">The username to be checked</param>
        /// <returns>True if the username exists, false if it doesn't</returns>
        public bool UserExists(string username)
        {
            Teacher teacher = (from t in entities.Teachers
                               where t.UserName == username
                               select t).FirstOrDefault();
            return teacher != null;
        }

        /// <summary>
        /// Check if the given username-password combination is valid
        /// </summary>
        /// <param name="username">The username to be checked</param>
        /// <param name="password">The password to be checked</param>
        /// <returns>True if the combination is valid, false if it isn't</returns>
        public bool UserIsValid(string username, string password) {
            return GetTeacher(username,password) != null;
        }

        public bool IsUserClassTeacher(Teacher teacher) {
            return teacher.PrimaryClasses.Count > 0 || teacher.SecondaryClasses.Count > 0;
        }

        /// <summary>
        /// Returns a teacher with the given ID
        /// </summary>
        /// <param name="id">The ID of the teacher-object</param>
        /// <returns>The teacher object associated with the given ID</returns>
        public Teacher GetTeacher(int id) {
            return (from t in entities.Teachers
                    where t.ID == id
                    select t).FirstOrDefault();
        }

        /// <summary>
        /// Returns a teacher with the given username-password combination
        /// </summary>
        /// <param name="username">The username of the teacher</param>
        /// <param name="password">The password of the teacher</param>
        /// <returns>The teacher-object associated with the given username-password combination</returns>
        public Teacher GetTeacher(string username, string password)
        {
            return (from t in entities.Teachers
                               where t.UserName == username &&
                               t.Password == password
                               select t).FirstOrDefault();            
        }

        /// <summary>
        /// Returns all teacher-objects
        /// </summary>
        /// <returns>A list of all teacher-objects</returns>
        public IQueryable<Teacher> GetTeachers() {
            return entities.Teachers;
        }

        /// <summary>
        /// Adds a teacher to the database
        /// </summary>
        /// <param name="teacher">The teacher-object to be added</param>
        /// <exception cref="DuplicateUserException"></exception>
        public void AddTeacher(Teacher teacher) {
            List<Teacher> existing = (from t in entities.Teachers
                                      where t.UserName == teacher.UserName
                                      select t).ToList();
            if (existing.Count > 0) {
                teacher.UserName += existing.Count + 1;
            }
            //if (!UserExists(teacher.UserName))
            //{
                entities.AddToTeachers(teacher);
                entities.SaveChanges();
            //}
            //else {
            //    throw new DuplicateUserException("User \"" + teacher.UserName + "\" already exists!");
            //}
        }

        /// <summary>
        /// Deletes the teacher with the given ID
        /// </summary>
        /// <param name="id">The ID of the teacher-object</param>
        /// <exception cref="EntryNotFoundException"></exception>
        public void DeleteTeacher(int id)
        {
            Teacher teacher = GetTeacher(id);
            if (teacher == null)
                throw new EntryNotFoundException("User with ID \"" + id + "\" does not exist");
            if (teacher.PrimaryClasses.Count > 0){
                
                throw new ClassNotEmptyException(teacher.PrimaryClasses.First());
            }
            teacher.TeacherSubjectAssignments.Clear();

            entities.DeleteObject(teacher);
            entities.SaveChanges();
        }
        #endregion

        #region Pupil

        public Pupil GetPupil(int id) {
            return (from pupil in entities.Pupils
                    where pupil.ID == id
                    select pupil).FirstOrDefault();
        }

        public IQueryable<Pupil> GetPupils() {
            return entities.Pupils;
        }

        public void AddPupil(Pupil pupil) {
            entities.AddToPupils(pupil);
            AddMockGrades(pupil);
            entities.SaveChanges();
        }

        private void AddMockGrades(Pupil pupil) {
            foreach (BranchSubjectAssignment bsa in pupil.SchoolClass.Branch.BranchSubjectAssignments.Where(x=>x.Level==pupil.SchoolClass.Level)) { 
                foreach(SubjectArea sa in bsa.Subject.SubjectAreas){
                    AssignNewGrade(pupil, sa, 0);
                }
            }
        }

        public void DeletePupil(int id) {
            Pupil pupil = GetPupil(id);
            if (pupil == null)
                throw new EntryNotFoundException("Pupil with ID \"" + id + "\" does not exist");
            pupil.VoluntarySubjectAssignements.Clear();
            pupil.Grades.Clear();
            pupil.SPFs.Clear();
            entities.DeleteObject(pupil);
            entities.SaveChanges();
        }

        #endregion

        #region SchoolClass

        public void AddClass(SchoolClass schoolClass){
            entities.AddToSchoolClasses(schoolClass);
            entities.SaveChanges();
        }

        public SchoolClass GetClass(int id) {
            return (from schoolclass in entities.SchoolClasses
                    where schoolclass.ID == id
                    select schoolclass).FirstOrDefault();
        }

        public SchoolClass GetClass(string name)
        {
            return (from schoolclass in entities.SchoolClasses
                    where schoolclass.Name == name
                    select schoolclass).FirstOrDefault();
        }

        //public SchoolClass GetClassOfClassTeacher(Teacher teacher) {
          
        //}

        public IEnumerable<SchoolClass> GetClassesOfTeacher(Teacher teacher) {
            return (from tsa in teacher.TeacherSubjectAssignments
                    select tsa.SchoolClass).Distinct();
        }

        public IQueryable<SchoolClass> GetClasses() {
            return entities.SchoolClasses;
        }

        public void DeleteClass(int id)
        {
            SchoolClass schoolClass = GetClass(id);
            if (schoolClass == null)
                throw new EntryNotFoundException("Class with ID \"" + id + "\" does not exist");
            schoolClass.Pupils.Clear();
            entities.DeleteObject(schoolClass);
            entities.SaveChanges();
        }

        #endregion

        #region Subject

        public Subject GetSubject(int id) {
            return (from subject in entities.Subjects
                    where subject.ID == id
                    select subject).FirstOrDefault();
        }

        public Subject GetSubject(string name)
        {
            return (from subject in entities.Subjects
                    where subject.Name == name
                    select subject).FirstOrDefault();
        }

        public IQueryable<Subject> GetSubjects() {
            return entities.Subjects;
        }

        public IEnumerable<Subject> GetSubjectsOfTeacher(int teacherID, int schoolClassID) {
            Teacher teacher = GetTeacher(teacherID);
            return from tsa in teacher.TeacherSubjectAssignments
                   where tsa.ClassID == schoolClassID
                   select tsa.Subject;
        }

        public void AddSubject(Subject subject) {
            entities.AddToSubjects(subject);
            entities.SaveChanges();
        }

        public void DeleteSubject(int id) {
            Subject subject = GetSubject(id);
            if(subject == null)
                throw new EntryNotFoundException("Subject with ID \"" + id + "\" does not exist");
            foreach (SubjectArea area in subject.SubjectAreas.ToList()) {
                area.Grades.Clear();
                entities.DeleteObject(area);
            }
            subject.TeacherSubjectAssignments.Clear();
            subject.VoluntarySubjectAssignements.Clear();
            subject.BindingSubjectAssignments.Clear();
            entities.DeleteObject(subject);
            entities.SaveChanges();
        }
                
        #endregion

        #region SPF

        public SPF GetSPF(int id) {
            return (from spf in entities.SPFs
                    where spf.ID == id
                    select spf).FirstOrDefault();
        }

        public IEnumerable<SPF> GetSPFs()
        {
            return entities.SPFs;
        }

        public IEnumerable<Utility.WebSPF> GetFormattedSPFs(int pupilID)
        {
            Pupil pupil = GetPupil(pupilID);
            return (from spf in pupil.SPFs
                    select new Utility.WebSPF { ID = spf.ID, SubjectID = spf.SubjectID, PupilID = spf.PupilID, Level = spf.Level, SubjectName = spf.Subject.Name });
        }

        #endregion

        #region Branch

        public Branch GetBranch(int id) {
            return (from branch in entities.Branches
                    where branch.ID == id
                    select branch).FirstOrDefault();
        }

        public IQueryable<Branch> GetBranches() {
            return entities.Branches;
        }

        public void AddBranch(Branch branch) {
            entities.AddToBranches(branch);
            entities.SaveChanges();
        }

        public void DeleteBranch(int id) {
            Branch branch = GetBranch(id);
            if (branch == null)
                throw new EntryNotFoundException("Branch with ID \"" + id + "\" does not exist");
            branch.BranchSubjectAssignments.Clear();
            entities.DeleteObject(branch);
            entities.SaveChanges();
        }

        #endregion

        #region SubjectArea

        public SubjectArea GetSubjectArea(int id) {
            return (from subjectArea in entities.SubjectAreas
                    where subjectArea.ID == id
                    select subjectArea).FirstOrDefault();
        }

        public IQueryable<SubjectArea> GetSubjectAreas() {
            return entities.SubjectAreas;
        }

        public void AddSubjectArea(SubjectArea subjectArea) {
            entities.AddToSubjectAreas(subjectArea);
            entities.SaveChanges();
        }

        public void DeleteSubjectArea(int id) {
            SubjectArea subjectArea = GetSubjectArea(id);
            if(subjectArea == null)
                throw new EntryNotFoundException("Subject Area with ID \"" + id + "\" does not exist");
            entities.DeleteObject(subjectArea);
            entities.SaveChanges();
        }            
        
        #endregion

        
        public void AssignVoluntarySubject(Pupil pupil, Subject subject) {
            entities.AddToVoluntarySubjectAssignements(new VoluntarySubjectAssignement() { Pupil = pupil, Subject = subject });
            entities.SaveChanges();
        }

        public void AssignSPF(Pupil pupil, Subject subject) {
            entities.AddToSPFs(new SPF() { Pupil = pupil, Subject = subject, Level = pupil.SchoolClass.Level });
            entities.SaveChanges();
        }

        public void AssignSPF(Pupil pupil, Subject subject, int level)
        {
            entities.AddToSPFs(new SPF() { Pupil = pupil, Subject = subject, Level = level });
            entities.SaveChanges();
        }

        public void AssignSPF(int pupilID, int subjectID, int level)
        {
            entities.AddToSPFs(new SPF() { PupilID = pupilID, SubjectID = subjectID, Level = level });
            entities.SaveChanges();
        }

        public void AssignNewGrade(Pupil pupil, SubjectArea subjectArea, int grade) {
            entities.AddToGrades(new Grade() { Pupil = pupil, SubjectArea = subjectArea, Value = grade });
            entities.SaveChanges();
        }

        public void AssignGrade(int pupilID, int subjectAreaID, int grade)
        {
            Grade grd = (from g in GetSubjectArea(subjectAreaID).Grades
                           where g.PupilID == pupilID
                           select g).FirstOrDefault();
            grd.Value = grade;
            entities.SaveChanges();
        }

        public void AssignSubject(Branch branch, Subject subject, int level) {
            entities.AddToBranchSubjectAssignments(new BranchSubjectAssignment() { Branch = branch, Subject = subject, Level = level });
            entities.SaveChanges();
        }

        public void AssignSubject(Teacher teacher, Subject subject, SchoolClass schoolClass) {
            entities.AddToTeacherSubjectAssignments(new TeacherSubjectAssignment() { Teacher = teacher, Subject = subject, SchoolClass = schoolClass});
            entities.SaveChanges();
        }

        public void AssignSchoolClass(Teacher teacher, SchoolClass schoolClass) {
            if (teacher.PrimaryClasses.Count > 0)
                throw new AlreadyAssignedException(teacher.PrimaryClasses.First(), teacher);

                teacher.PrimaryClasses.Add(schoolClass);
                entities.SaveChanges();
            
        }

        public void DeleteSPF(int id) {
            SPF spf = GetSPF(id);
            entities.DeleteObject(spf);
            entities.SaveChanges();
        }

        public IEnumerable<Subject> GetSubjectsOfClass(SchoolClass schoolClass) {
            return (from bsa in schoolClass.Branch.BranchSubjectAssignments
                    where bsa.Level == schoolClass.Level
                    select bsa.Subject);
        }

        public IEnumerable<Subject> GetSubjectsOfPupil(Pupil pupil) {
            return (from bsa in pupil.SchoolClass.Branch.BranchSubjectAssignments
                    where bsa.Level == pupil.SchoolClass.Level
                    select bsa.Subject).ToList();
        }

        public IEnumerable<Grade> GetGradesOfPupil(Pupil pupil, Subject subject) {
            return (from grade in pupil.Grades
                    where grade.SubjectArea.SubjectID == subject.ID
                    orderby grade.SubjectArea.Name descending
                    select grade);
        }
        
    }
}
