using System;
using System.Collections.Generic;
using System.Text;

namespace Sms.Security
{
    public interface IPasswordHasher
    {
        string ComputeHash(string clearTextPassword, byte[] salt);

        byte[] GenerateRandomSalt();
    }
}
