using System;
using System.Collections.Generic;
using System.Text;

namespace Sms.Model
{
    internal class Attendance
    {
        public int AttendanceId { get; set; }
        public int StudentId { get; set; }
        public int CourseId { get; set; }
        public DateTime Date { get; set; }
        public string Status { get; set; }
    }
}
