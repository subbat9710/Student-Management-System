using System;
using System.Collections.Generic;
using System.Text;

namespace Sms.Model
{
    public class User
    {
        private string password;

        public User() { }
        public User(string username, string password, string role)
        {
            Username = username;
            this.password = password;
            Role = role;
        }

        public int Id { get; set; }
        public string Username { get; set; }
        public string Role { get; set; }
    }
}
