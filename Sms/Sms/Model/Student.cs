using System;
using System.Collections.Generic;
using System.Text;

namespace Sms.Model
{
    public class Student 
    {
        public int StudentId { get; set; } 
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateTime DateOfBirth { get; set; }
        public int CourseId { get; set; }  
        public int UserId { get; set; }
        public int CompletedHours { get; set; }

        public override string ToString()
        {
            return $"Student Id: {StudentId} - Name: {FirstName}, {LastName}";
        }
    }
}
