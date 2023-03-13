using Sms.Model;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Runtime.CompilerServices;
using System.Text;

namespace Sms.DAO
{
    public class StudentSqlDao : IStudentDao
    {
        private readonly string _connectionString;
        
        public StudentSqlDao(string connString)
        {
            _connectionString = connString;
        }

        public Student createProfileDetails(Student student) 
        {
            int newStudentId = 0;

            try
            {
                using (SqlConnection connection = new SqlConnection(_connectionString))
                {
                    User user = new User();
                    connection.Open();
                    SqlCommand cmd = new SqlCommand();
                    string sql = "INSERT INTO student_sms (first_name, last_name, date_of_birth, course_id, user_id, completed_hours) " +
                        "OUTPUT INSERTED.student_id " +
                        "VALUES(@firstName, @lastName, @dob, @courseid, @user_id, @completed_hours); " +
                        "SELECT SCOPE_IDENTITY();";

                    cmd.CommandText = sql;
                    cmd.Connection = connection;
                    cmd.Parameters.AddWithValue("@firstName", student.FirstName);
                    cmd.Parameters.AddWithValue("@lastName", student.LastName);
                    cmd.Parameters.AddWithValue("@dob", student.DateOfBirth);
                    cmd.Parameters.AddWithValue("@courseid", student.CourseId);
                    cmd.Parameters.AddWithValue("@user_id", student.UserId);
                    cmd.Parameters.AddWithValue("@completed_hours", student.CompletedHours);

                    newStudentId = Convert.ToInt32(cmd.ExecuteScalar());
                    student.StudentId = newStudentId;
                }
            }
            catch (Exception e){ Console.WriteLine($"Error inserting data:{e.Message}"); }
            return student;
        }
        public IList<Student> GetAllStudent()
        {
            IList<Student> result = new List<Student>();

            try
            {
                using(SqlConnection connection = new SqlConnection(_connectionString))
                {
                    connection.Open();
                    SqlCommand cmd = new SqlCommand();
                    string sql = "SELECT * FROM student_sms;";
                    cmd.CommandText = sql;
                    cmd.Connection = connection;

                    SqlDataReader reader = cmd.ExecuteReader();

                    while (reader.Read())
                    {
                        Student std = new Student();
                        std = CreateStudentFromReader(reader);
                        result.Add(std);
                    }
                }
            }
            catch (Exception e) { Console.WriteLine($"Error Retrieving Data: {e.Message}"); }
            return result;
        }
        public Student GetFirstLastName(string username)
        {
            Student student = new Student();
            try
            {
                using (SqlConnection connection = new SqlConnection(_connectionString))
                {
                    connection.Open();
                    SqlCommand cmd = new SqlCommand();
                    string sql = "SELECT s.first_name, s.last_name FROM users_sms u JOIN student_sms s ON u.id = s.user_id WHERE u.username = @username;";
                    cmd.CommandText = sql;
                    cmd.Parameters.AddWithValue("@username", username);
                    cmd.Connection = connection;

                    SqlDataReader reader = cmd.ExecuteReader();

                    if (reader.Read())
                    {
                        string firstName = reader.GetString(0);
                        string lastName = reader.GetString(1);
                        Console.WriteLine($"{firstName} {lastName}, You Logged in as Student!");
                    }
                }
            }
            catch(Exception e) { Console.WriteLine($"An error occurred while retrieving the student's name: {e.Message}"); }
            return student;
        }
        public IList<Student> SearchStudentByName(string firstNameSearch, string lastNmaeSearch)
        {
            IList<Student> result = new List<Student>();
            try
            {
                using(SqlConnection connection = new SqlConnection(_connectionString))
                {
                    connection.Open();
                    SqlCommand command = new SqlCommand();
                    string sql = "SELECT * FROM student_sms WHERE first_name LIKE '%' + @first_name + '%' AND last_name LIKE '%' + @last_name + '%';";
                    command.CommandText = sql;
                    command.Connection = connection;
                    command.Parameters.AddWithValue("@first_name", firstNameSearch);
                    command.Parameters.AddWithValue("@last_name", lastNmaeSearch);

                    SqlDataReader reader = command.ExecuteReader();

                    while (reader.Read())
                    {
                        Student tempStd = new Student();
                        tempStd = CreateStudentFromReader(reader);
                        result.Add(tempStd);
                    }
                }
            } catch(Exception e) { Console.WriteLine(e.Message); }
            return result;
        }
        public bool UpdateStudent(Student updateStudent)
        {
            try
            {
                using(SqlConnection connection = new SqlConnection(_connectionString))
                {
                    connection.Open();
                    SqlCommand cmd = new SqlCommand();
                    string sql = "UPDATE student_sms SET first_name = @first_name, last_name = @last_name WHERE student_id = @student_id";
                    cmd.CommandText = sql;
                    cmd.Connection = connection;
                    cmd.Parameters.AddWithValue("@first_name", updateStudent.FirstName);
                    cmd.Parameters.AddWithValue("@last_name", updateStudent.LastName);
                    cmd.Parameters.AddWithValue("@student_id", updateStudent.StudentId);

                    return cmd.ExecuteNonQuery() == 1;
                }
            }catch(Exception e) { Console.WriteLine($"Error Updating Student: {e.Message}"); }
            return false;
        }
        public bool RemoveStudent(int studentId)
        { 
            try
            {
                using(SqlConnection connection = new SqlConnection(_connectionString))
                {
                    connection.Open();
                    SqlCommand cmd = new SqlCommand();
                    string sql = "DELETE FROM student_sms WHERE student_id = @student_id;";
                    cmd.CommandText = sql;
                    cmd.Connection = connection;
                    cmd.Parameters.AddWithValue("@student_id", studentId);

                    return cmd.ExecuteNonQuery() == 1;
                }

            }catch(Exception e) { Console.WriteLine("Error deleting student record:", e.Message); }
            return false;
        }
        private Student CreateStudentFromReader(SqlDataReader reader)
        {
            Student student = new Student();
            student.StudentId = Convert.ToInt32(reader["student_id"]);
            student.FirstName = Convert.ToString(reader["first_name"]);
            student.LastName = Convert.ToString(reader["last_name"]);
            student.DateOfBirth = Convert.ToDateTime(reader["date_of_birth"]);
            student.UserId = Convert.ToInt32(reader["user_id"]);
            student.CourseId = Convert.ToInt32(reader["course_id"]);
            student.CompletedHours = Convert.ToInt32(reader["completed_hours"]);

            return student;
        }
    }
}
