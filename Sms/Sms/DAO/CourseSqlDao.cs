using Sms.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data.SqlClient;
using System.Text;
using System.Xml.Linq;

namespace Sms.DAO
{
    public class CourseSqlDao : ICourseDao
    {
        private readonly string _connectionString;

        public CourseSqlDao(string connString)
        {
            _connectionString = connString;
        }
        public Course GetCourse(Course course)
        {
            int newCourseId = 0;
            try
            {
                Instructor instructor = new Instructor();
                using (SqlConnection connection = new SqlConnection(_connectionString))
                {
                    connection.Open();
                    SqlCommand cmd = new SqlCommand();
                    string sql = "INSERT INTO COURSES_SMS (course_name, start_date, status, total_hours, instructor_id, description) " +
                        "OUTPUT INSERTED.course_id " +
                        "VALUES(@name, @start_date, @status, @total_hours, @instructor_id, @description);";
                    cmd.CommandText = sql;
                    cmd.Parameters.AddWithValue("@name", course.CourseName);
                    cmd.Parameters.AddWithValue("@start_date", course.StartDate);
                    cmd.Parameters.AddWithValue("@status", course.Status);
                    cmd.Parameters.AddWithValue("@total_hours", course.TotalHours);
                    cmd.Parameters.AddWithValue("@description", course.Description);
                    cmd.Parameters.AddWithValue("@instructor_id", course.InstructorId);
                    cmd.Connection = connection;

                    newCourseId = Convert.ToInt32(cmd.ExecuteScalar());
                }
            }
            catch (Exception e) { Console.WriteLine($"Error inserting data:{e.Message}"); }
            return course;
        }

        public Course CourseStatus(string username)
        {
            Course courses = new Course();
            try
            {
                using (SqlConnection connection = new SqlConnection(_connectionString))
                {
                    connection.Open();

                    SqlCommand cmd = new SqlCommand();
                    string sql = "SELECT c.status, s.course FROM COURSES_SMS c " +
                        "JOIN STUDENTS_SMS s ON s.course = c.course_name " +
                        "JOIN users_sms u ON u.user_id = s.user_id " +
                        "WHERE u.username = @username";

                    cmd.CommandText = sql;
                    cmd.Parameters.AddWithValue("@username", username);
                    cmd.Connection = connection;

                    SqlDataReader reader = cmd.ExecuteReader();

                    if (reader.Read())
                    {
                        string status = reader.GetString(0);
                        string course = reader.GetString(1);

                        if (status != null && course != null)
                        {
                            Console.WriteLine($" {course}: {status}!");
                        }
                        else
                        {
                            Console.WriteLine("Not Updated! Check back soon!");
                        }
                    }
                }
            }
            catch { }
            return courses;
        }
        public IList<Course> GetAllCourse()
        {
            IList<Course> result = new List<Course>();
            try
            {
                using (SqlConnection connection = new SqlConnection(_connectionString))
                {
                    connection.Open();
                    SqlCommand cmd = new SqlCommand();
                    string sql = "SELECT * FROM COURSES_SMS;";
                    cmd.CommandText = sql;
                    cmd.Connection = connection;

                    SqlDataReader reader = cmd.ExecuteReader();

                    while (reader.Read())
                    {
                        Course course = new Course();
                        course = CreateCourseFromReader(reader);
                        result.Add(course);
                    }
                }
            }
            catch (Exception e) { Console.WriteLine(e.Message); }
            return result;
        }
        public IList<Course> GetCoursesByUsername(string username)
        {
            IList<Course> result = new List<Course>();
            try
            {
                using (SqlConnection connection = new SqlConnection(_connectionString))
                {
                    connection.Open();
                    SqlCommand cmd = new SqlCommand();
                    string sql = "SELECT c.course_name, c.start_date, c.status, c.total_hours, c.description, c.instructor_id FROM COURSES_SMS c " +
                        "JOIN GRADES_SMS a ON a.course_id = c.course_id " +
                        "JOIN STUDENTS_SMS s ON s.student_id = a.student_id " +
                        "JOIN users_sms u ON u.user_id = s.user_id " +
                        "WHERE u.username = @username;";
                    cmd.CommandText = sql;
                    cmd.Parameters.AddWithValue("@username", username);
                    cmd.Connection = connection;

                    SqlDataReader reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        Course tempCourse = new Course();
                        tempCourse = CreateCourseFromReader(reader);
                        result.Add(tempCourse);
                    }
                }
            }
            catch (Exception e) { Console.WriteLine("Error loading course details: ", e.Message); }
            return result;
        }

        public IList<Course> GetCoursesByUsernameInstructor(string username)
        {
            IList<Course> result = new List<Course>();
            try
            {
                using (SqlConnection connection = new SqlConnection(_connectionString))
                {
                    connection.Open();
                    SqlCommand cmd = new SqlCommand();
                    string sql = "SELECT c.course_name, c.start_date, c.status, c.total_hours, c.description, c.instructor_id " +
                        "FROM COURSES_SMS c JOIN INSTRUCTORS_SMS i on i.instructor_id = c.instructor_id " +
                        "JOIN users_sms u ON u.user_id = i.user_id WHERE u.username = @username;";
                    cmd.CommandText = sql;
                    cmd.Parameters.AddWithValue("@username", username);
                    cmd.Connection = connection;

                    SqlDataReader reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        Course tempCourse = new Course();
                        tempCourse = CreateCourseFromReader(reader);
                        result.Add(tempCourse);
                    }
                }
            }
            catch (Exception e) { Console.WriteLine("Error loading course details: ", e.Message); }
            return result;
        }
        private Course CreateCourseFromReader(SqlDataReader reader)
        {
            Course temp = new Course();
            temp.CourseName = Convert.ToString(reader["course_name"]);
            temp.StartDate = Convert.ToDateTime(reader["start_date"]);
            temp.Status = Convert.ToString(reader["status"]);
            temp.TotalHours = Convert.ToInt32(reader["total_hours"]);
            temp.InstructorId = Convert.ToInt32(reader["instructor_id"]);
            temp.Description = Convert.ToString(reader["description"]);

            return temp;
        }
    }
}
