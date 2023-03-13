using Sms.Model;
using System;
using System.Collections.Generic;
using System.Text;

namespace Sms.DAO
{
    public interface IStudentDao 
    {
        Student createProfileDetails(Student newStudent);
        public IList<Student> GetAllStudent();
        Student GetFirstLastName(string student);
        bool UpdateStudent(Student updateStudent);
        IList<Student> SearchStudentByName(string firstNameSearch, string lastNmaeSearch);
        bool RemoveStudent(int studentId);
    }
}
