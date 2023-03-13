using System;
using System.Collections.Generic;
using System.Text;

namespace Sms.Model
{
    public class Course
    {
        public int CourseId { get; set; }
        public string CourseName { get; set; }
        public DateTime StartDate { get; set; }
        public string Status { get; set; } 
        public int TotalHours { get; set; }
        public string Description { get; set; }
        public int InstructorId { get; set; }

        public override string ToString()
        {
            return $"Course: {CourseName}\nStart Date: {StartDate}\nCourse Status: {Status}\nTotal Hours: {TotalHours}\nDescription: {Description}\n";
        }
    }
}
