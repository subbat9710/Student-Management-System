using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;

namespace Sms.Security
{
    public class PasswordHasher : IPasswordHasher
    {
        private const int HASH_LENGTH = 160 / 8;
        private const int SALT_LENGTH = 128 / 8;
        private const int WORK_FACTOR = 100000;

        public string ComputeHash(string clearTextPassword, byte[] salt)
        {
            Rfc2898DeriveBytes rfc2898DeriveBytes = new Rfc2898DeriveBytes(clearTextPassword, salt, WORK_FACTOR);
            byte[] passwordHash = rfc2898DeriveBytes.GetBytes(HASH_LENGTH);

            byte[] saltHash = new byte[SALT_LENGTH + HASH_LENGTH];

            Array.Copy(salt, 0, saltHash, 0, SALT_LENGTH);
            Array.Copy(passwordHash, 0, saltHash, SALT_LENGTH, HASH_LENGTH);

            return Convert.ToBase64String(saltHash);
        }

        public byte[] GenerateRandomSalt()
        {
            byte[] salt = new byte[SALT_LENGTH];
            using (RandomNumberGenerator rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(salt);
            }
            return salt;
        }
    }
}
