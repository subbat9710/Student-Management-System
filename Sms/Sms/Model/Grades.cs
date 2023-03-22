using System;
using System.Collections.Generic;
using System.Text;

namespace Sms.Model
{
    internal class Grades
    {
        public int GradeId { get; set; }
        public int StudentId { get; set; }
        public int CourseId { get; set; }
        public int Grade { get; set; }
        public string AssignmentName { get; set; }
    }
}
