using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using HTL.Grieskirchen.VaKEGrade.Database;

namespace HTL.Grieskirchen.VaKEGrade.Utility
{
    public class DataGenerator
    {
        VaKEGrade.Database.VaKEGradeRepository repository = new Database.VaKEGradeRepository();

        string[] mFirstNames = new string[] { "Florian", "Matthias", "Daniel", "David", "Andreas", "Martin", "Michael", "Philipp", "Fabian", "Jakob", "Maximilian", "Dominik", "Benjamin", "Josef", "Peter", "Hubert", "Herbert", "Rupert", "Georg", "Lukas", "Wolfgang", "Max", "Roman", "Heinz", "Thorben", "Thorsten", "Paul", "Patrick", "Christian", "Wilhelm", "Gerhard", "Erich", "Engelbert", "Rüdiger", "Konrad" };

        string[] wFirstNames = new string[] { "Bianca", "Marianne", "Maria", "Susanne", "Michaela", "Andrea", "Pia", "Amelie", "Anna", "Eva", "Elisabeth", "Johanna", "Sonja", "Christine", "Christina", "Mathilde", "Helene", "Jaqueline", "Claudia", "Rebecka", "Gertraud", "Waltraud", "Natalie", "Natascha", "Margarethe", "Sarah", "Roswitha", "Daniela", "Paula", "Tina", "Lucia"};

        string[] lastNames = new string[] { "Huber", "Bauer", "Müller", "Maier", "Schmied", "Strasser", "Lotz", "Wiesinger", "Berger", "Gundacker", "Stadlmaier", "Ebetshuber", "Mayr", "Hofer", "Hauser", "Köfler", "Kreilinger", "Plakolb", "Holzberger", "Hochhauser", "Mitterbucher", "Eder", "Wenzl", "Moser", "Ebner", "Wendt", "Kreuzhuber", "Kaiser", "Schwarz", "Hartholz", "Wagner", "Scheuringer", "Schallhammer", "Holler", "Weigert", "Sageder", "Trost", "Nimeth", "Rachbauer", "Pühringer", "Scherber", "Rotwald", "Schmidthammer", "Schmidbauer", "Reifetshammer" };

        public void GenerateData() {
            //AssignBranchSubjects();
            //GenerateTeachers();
            //GenerateClasses();
            AssignTeacherSubjects();
            //GeneratePupils();
            
        }

        public void GenerateTeachers()
        {
            

            Random random;
            Teacher teacher;
            for (int i = 0; i < 20; i++)
            {
                random = new Random();
                teacher = new Teacher();
                if (random.Next(0, 2) == 0)
                {
                    teacher.FirstName = mFirstNames[random.Next(0, mFirstNames.Length)];
                }
                else
                {
                    teacher.FirstName = wFirstNames[random.Next(0, wFirstNames.Length)];
                }
                teacher.LastName = lastNames[random.Next(0, lastNames.Length)];
                teacher.UserName = teacher.FirstName[0] + teacher.LastName;
                teacher.Password = "abc123!";
                repository.AddTeacher(teacher);
            }
        }

        public void GenerateClasses() {
            Random random;
            SchoolClass schoolClass;
            List<Branch> b = repository.GetBranches().ToList();

            for (int i = 0; i < 12; i++)
            {
                random = new Random();
                schoolClass = new SchoolClass();
                schoolClass.Level = random.Next(1,5);
                schoolClass.Name = ((char)random.Next(97, 106)).ToString();
                schoolClass.Branch = b[random.Next(0, b.Count)];
                List<Teacher> pt = repository.GetTeachers().Where(x=>x.PrimaryClasses.Count == 0).ToList();
                schoolClass.PrimaryClassTeacher = pt[random.Next(0, pt.Count)];
                if (random.Next(0, 100) <= 20) {
                    pt = repository.GetTeachers().Where(x => x.PrimaryClasses.Count == 0).ToList();
                    schoolClass.SecondaryClassTeacher = pt[random.Next(0, pt.Count)];
                }
                repository.AddClass(schoolClass);
            }
        }

        public void GeneratePupils() {
            string[] religions = new string[] { "römisch-katholisch", "muslimisch", "ohne Bekenntnis", "Zeuge Jehova", "buddhistisch", "evangelisch" };

            List<SchoolClass> sc = repository.GetClasses().ToList();

            Random random;
            Pupil pupil;
            for (int i = 0; i < 100; i++)
            {
                random = new Random();
                pupil = new Pupil();
                if (random.Next(0, 2) == 0)
                {
                    pupil.Gender = "m";
                    pupil.FirstName = mFirstNames[random.Next(0, mFirstNames.Length)];
                }
                else
                {
                    pupil.Gender = "w";
                    pupil.FirstName = wFirstNames[random.Next(0, wFirstNames.Length)];
                }
                pupil.LastName = lastNames[random.Next(0, lastNames.Length)];
                pupil.Religion = religions[random.Next(0, religions.Length)];
                pupil.Birthdate = new DateTime(DateTime.Now.AddYears(-random.Next(11, 15)).Year, random.Next(1, 13), random.Next(1, 28));
                List<SchoolClass> pc = repository.GetClasses().Where(x=>x.Level==DateTime.Now.Year-pupil.Birthdate.Year-10).ToList();

                pupil.SchoolClass = pc[random.Next(0, pc.Count)];
                repository.AddPupil(pupil);

                foreach(Subject subject in pupil.SchoolClass.Branch.BranchSubjectAssignments.Where(x=>x.Level==pupil.SchoolClass.Level).Select(x=>x.Subject).Distinct().ToList()){
                    foreach (SubjectArea area in subject.SubjectAreas.ToList()) {
                        repository.AssignGrade(pupil.ID, area.ID, random.Next(1, 6));
                    }
                }
            }
        }

        public void AssignBranchSubjects()
        {

            List<Subject> subjects;
            Random random;
            foreach (Branch branch in repository.GetBranches().ToList()) {
                for (int i = 1; i < 5; i++) {
                    subjects = repository.GetSubjects().Where(x=>!x.IsVoluntary&&!x.IsBinding).ToList();
                    for (int j = 0; j < 8; j++)
                    {
                        random = new Random();
                        int pos = random.Next(0, subjects.Count);
                        repository.AssignSubject(branch, subjects[pos], i);
                        subjects.RemoveAt(pos);
                    }
                }
            }
        }

        public void AssignTeacherSubjects()
        {
            int maxClasses = 5;
            int currentClasses = 0;
            List<Subject> subjects;
            List<SchoolClass> schoolClasses;
            Random random;
            foreach (Teacher teacher in repository.GetTeachers().ToList())
            {
                random = new Random();
                currentClasses = 0;
                subjects = repository.GetSubjects().Where(x => !x.IsVoluntary && !x.IsBinding).ToList();
                int subjectCount = random.Next(1, 4);
                for (int i = 0; i < subjectCount; i++ )
                {
                    int pos = random.Next(0, subjects.Count);
                    Subject subject = subjects[pos];
                    schoolClasses = subject.BranchSubjectAssignments.Select(x => x.Branch).SelectMany(x => x.SchoolClasses).ToList();

                    int classCount = random.Next(1,3);
                    for (int j = 0; j < classCount && currentClasses < 5; j++)
                    {
                        int pos2 = random.Next(0, schoolClasses.Count);
                        repository.AssignSubject(teacher, subject, schoolClasses[pos2]);
                        schoolClasses.RemoveAt(pos2);
                        currentClasses++;
                    }
                    subjects.RemoveAt(pos);
                }                       
            }
        }
    }
}