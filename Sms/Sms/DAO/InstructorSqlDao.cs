using Sms.Model;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Text;

namespace Sms.DAO
{
    public class InstructorSqlDao : IInstructorDao
    {
        private readonly string _connectionString;

        public InstructorSqlDao(string connString)
        {
            _connectionString = connString;
        }
        public Instructor createProfileDetails(Instructor instructor)
        {
            int instructorId = 0;

            try
            {
                using(SqlConnection connection = new SqlConnection(_connectionString))
                {
                    connection.Open();

                    SqlCommand cmd = new SqlCommand();

                    string sql = "INSERT INTO instructor_sms (first_name, last_name, date_of_birth, user_id) " +
                               "OUTPUT INSERTED.instructor_id " +
                               "VALUES (@firstName, @lastName, @dob, @user_id)";

                    cmd.CommandText = sql;
                    cmd.Connection = connection;

                    cmd.Parameters.AddWithValue("@firstName", instructor.FirstName);
                    cmd.Parameters.AddWithValue("@lastName", instructor.LastName);
                    cmd.Parameters.AddWithValue("@dob", instructor.DateOfBirth);
                    cmd.Parameters.AddWithValue("@user_id", instructor.UserId);

                    instructorId = Convert.ToInt32(cmd.ExecuteScalar());
                }
            }
            catch (Exception e) { Console.WriteLine($"Error inserting data:{e.Message}"); }
            return instructor;
        }
        public Instructor GetFirstLastName(string username)
        {
            Instructor instructor = new Instructor();
            try
            {
                using (SqlConnection connection = new SqlConnection(_connectionString))
                {
                    connection.Open();
                    SqlCommand cmd = new SqlCommand();
                    string sql = "SELECT s.first_name, s.last_name FROM users_sms u JOIN instructor_sms s ON u.id = s.user_id WHERE u.username = @username;";
                    cmd.CommandText = sql;
                    cmd.Parameters.AddWithValue("@username", username);
                    cmd.Connection = connection;

                    SqlDataReader reader = cmd.ExecuteReader();

                    if (reader.Read())
                    {
                        string firstName = reader.GetString(0);
                        string lastName = reader.GetString(1);
                        Console.WriteLine($"{firstName} {lastName}, You Logged in as Instructor!");
                    }
                }
            }
            catch (Exception e) { Console.WriteLine($"An error occurred while retrieving the student's name: {e.Message}"); }
            return instructor;
        }
        public IList<Instructor> GetAllInstructor()
        {
            IList<Instructor> result = new List<Instructor>();
            try
            {
                using(SqlConnection connection = new SqlConnection(_connectionString))
                {
                    connection.Open();
                    SqlCommand cmd = new SqlCommand();
                    string sql = "SELECT * FROM instructor_sms;";
                    cmd.CommandText = sql;
                    cmd.Connection = connection;

                    SqlDataReader reader = cmd.ExecuteReader();

                    while (reader.Read())
                    {
                        Instructor instructor = new Instructor();
                        instructor = CreateInstructorFromReader(reader);
                        result.Add(instructor);
                    }
                }
            }catch(Exception e) { Console.WriteLine(e.Message); }
            return result;
        }
        private Instructor CreateInstructorFromReader(SqlDataReader reader)
        {
            Instructor instructor = new Instructor();
            instructor.Id = Convert.ToInt32(reader["instructor_id"]);
            instructor.FirstName = Convert.ToString(reader["first_name"]);
            instructor.LastName = Convert.ToString(reader["last_name"]);
            instructor.DateOfBirth = Convert.ToDateTime(reader["date_of_birth"]);
            instructor.UserId = Convert.ToInt32(reader["user_id"]);

            return instructor;
        }
    }
}
