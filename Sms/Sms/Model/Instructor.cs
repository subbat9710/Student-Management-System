using System;
using System.Collections.Generic;
using System.Text;

namespace Sms.Model
{
    public class Instructor
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateTime DateOfBirth { get; set; }
        public int UserId { get; set; }

        public override string ToString()
        {
            return $"{LastName}, {FirstName}";
        }
    }
}
