using Sms.Model;
using Sms.Security;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Text;

namespace Sms.DAO
{
    public class UserSqlDao : IUserDao 
    {
        private readonly string _connectionString;
        private readonly IPasswordHasher _passwordHasher;

        public UserSqlDao()
        {
        }

        public UserSqlDao(string connectionString, IPasswordHasher passwordHasher)
        {
            _connectionString = connectionString;
            _passwordHasher = passwordHasher;
        }

        public bool IsUsernameAndPasswordValid(string username, string password)
        {
            using(SqlConnection connection = new SqlConnection(_connectionString))
            {
                connection.Open();

                SqlCommand cmd = connection.CreateCommand();
                cmd.CommandText = "SELECT password, salt FROM users_sms WHERE username = '" + username + "'";

                SqlDataReader reader = cmd.ExecuteReader();

                if (reader.Read())
                {
                    string storedPassword = (string)reader["password"];
                    string storedSalt = (string)reader["salt"];
                    string computedHash = _passwordHasher.ComputeHash(password, Convert.FromBase64String(storedSalt));

                    return computedHash.Equals(storedPassword);
                }

                return false;
            }
        }

        public User SaveUser(string username, string password, string role)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                byte[] salt = _passwordHasher.GenerateRandomSalt();
                string hashedPassword = _passwordHasher.ComputeHash(password, salt);

                connection.Open();

                //check if username already exist in the database
                SqlCommand cmd = new SqlCommand();
                string sql = "SELECT COUNT(*) FROM users_sms WHERE username = @username;";
                cmd.CommandText = sql;
                cmd.Parameters.AddWithValue("@username", username);
                cmd.Connection = connection;

                int count = 0;

                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        count = reader.GetInt32(0);
                    }
                }

                if (count > 0)
                {
                    Console.WriteLine($"Username: '{username}' already exist! Please try with different username!");
                    return null;
                }

                //save the user to the database
                SqlCommand command = connection.CreateCommand();
                command.CommandText = @"INSERT INTO users_sms (username, password, salt, role)
                                        VALUES (@username, @password, @salt, @role);
                                        SELECT SCOPE_IDENTITY();";

                command.Parameters.AddWithValue("@username", username);
                command.Parameters.AddWithValue("@password", hashedPassword);
                command.Parameters.AddWithValue("@salt", Convert.ToBase64String(salt));
                command.Parameters.AddWithValue("@role", role);
                int id = Convert.ToInt32(command.ExecuteScalar());

                return new User
                {
                    Id = id,
                    Username = username,
                    Role = role
                };
            }
        }
        public User SelectUserRole(string username, string password)
        {
            User user = null;
            try
            {
                using(SqlConnection connection = new SqlConnection(_connectionString))
                {
                    connection.Open();
                    SqlCommand cmd = new SqlCommand();
                    string sql = "SELECT role FROM users_sms WHERE username = @username";
                    cmd.CommandText = sql;
                    cmd.Connection = connection;
                    cmd.Parameters.AddWithValue("@username", username);
                    cmd.Parameters.AddWithValue("@password", password);

                    SqlDataReader reader = cmd.ExecuteReader();

                    if (reader.Read())
                    {
                        string role = reader.GetString(0);
                        user = new User(username, password, role);
                    }
                }
            }
            catch(Exception e) { Console.WriteLine(e.Message); }
            return user;
        }
    }
}
