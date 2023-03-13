using Microsoft.Extensions.Configuration;
using Sms.DAO;
using Sms.Model;
using Sms.Security;
using System;

namespace Sms
{
    internal class Program
    {
        static void Main(string[] args)
        {
            IConfiguration config = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json")
                .Build();

            string connectionString = config.GetConnectionString("UserManagerConnection");

            IPasswordHasher passwordHasher = new PasswordHasher();
            IUserDao userDao = new UserSqlDao(connectionString, passwordHasher);
            IStudentDao studentDao = new StudentSqlDao(connectionString);
            IInstructorDao instructorDao = new InstructorSqlDao(connectionString);
            ICourseDao courseDao = new CourseSqlDao(connectionString);

            UserCli application = new UserCli(userDao, studentDao, instructorDao, courseDao);
            application.Run();
        }
    }
}
