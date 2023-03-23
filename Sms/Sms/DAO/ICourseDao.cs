using Sms.Model;
using System;
using System.Collections.Generic;
using System.Text;

namespace Sms.DAO
{
    internal interface ICourseDao
    {
        Course GetCourse(Course course);
        Course CourseStatus(string username);
        IList<Course> GetAllCourse();
        IList<Course> GetCoursesByUsername(string username);
        IList<Course> GetCoursesByUsernameInstructor(string username);

    }
}
