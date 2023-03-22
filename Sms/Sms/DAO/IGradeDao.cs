using Sms.Model;
using System;
using System.Collections.Generic;
using System.Text;

namespace Sms.DAO
{
    internal interface IGradeDao
    {
        Grades GetGrade(Grades grade);
    }
}
