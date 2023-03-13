using Sms.Model;
using System;
using System.Collections.Generic;
using System.Text;

namespace Sms.DAO
{
    internal interface IUserDao
    {
        bool IsUsernameAndPasswordValid(string username, string password);
        User SaveUser(string username, string password, string role);
        User SelectUserRole(string username, string password);
    }
}
