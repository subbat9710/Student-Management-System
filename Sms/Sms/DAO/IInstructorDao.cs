using Sms.Model;
using System;
using System.Collections.Generic;
using System.Text;

namespace Sms.DAO
{
    public interface IInstructorDao
    {
        Instructor createProfileDetails(Instructor instructor);
        Instructor GetFirstLastName(string username);
        IList<Instructor> GetAllInstructor();

    }
}
