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

        public void Update() {
            entities.SaveChanges();
        }

        public bool UserExists(string username)
        {
            Teacher teacher = (from t in entities.Teachers
                               where t.UserName == username
                               select t).FirstOrDefault();
            return teacher != null;
        }

        public bool UserIsValid(string username, string password) {
            Teacher teacher = (from t in entities.Teachers
                               where t.UserName == username &&
                               t.Password == password
                               select t).FirstOrDefault();
            return teacher != null;
        }

        public Teacher GetTeacher(int id) {
            return (from t in entities.Teachers
                    where t.ID == id
                    select t).FirstOrDefault();
        }

        public Teacher GetTeacher(string username, string password)
        {
            return (from t in entities.Teachers
                               where t.UserName == username &&
                               t.Password == password
                               select t).FirstOrDefault();            
        }

        public IQueryable<Teacher> GetTeachers() {
            return entities.Teachers;
        }

        public void AddTeacher(Teacher teacher) {
            if (!UserExists(teacher.UserName))
            {
                entities.AddToTeachers(teacher);
                entities.SaveChanges();
            }
            else {
                throw new DuplicateUserException("User \"" + teacher.UserName + "\" already exists!");
            }
        }

        public void DeleteTeacher(int id) {
            Teacher teacher = (from t in entities.Teachers
                               where t.ID == id
                               select t).FirstOrDefault();
            entities.DeleteObject(teacher);
            entities.SaveChanges();
        }

    }
}
