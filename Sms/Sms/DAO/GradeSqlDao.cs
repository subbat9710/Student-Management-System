using Sms.Model;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Text;

namespace Sms.DAO
{
    internal class GradeSqlDao : IGradeDao
    {
        private readonly string _connectionString;

        public GradeSqlDao(string connString)
        {
            _connectionString = connString;
        }

        public Grades GetGrade(Grades grade)
        {
            int newGradeId = 0;
            try
            {
                using (SqlConnection connection = new SqlConnection(_connectionString))
                {
                    connection.Open();
                    SqlCommand cmd = new SqlCommand();
                    string sql = "INSERT INTO GRADES_SMS (student_id, course_id, grade, assignment_name) " +
                        "OUTPUT INSERTED.grade_id " +
                        "VALUES (@student_id, @course_id, @grade, @assignment_name) " +
                        "SELECT SCOPE_IDENTITY(); ";
                    cmd.CommandText = sql;
                    cmd.Parameters.AddWithValue("@student_id", grade.StudentId);
                    cmd.Parameters.AddWithValue("@course_id", grade.CourseId);
                    cmd.Parameters.AddWithValue("@grade", grade.Grade);
                    cmd.Parameters.AddWithValue("@assignment_name", grade.AssignmentName);
                    cmd.Connection = connection;

                    newGradeId = Convert.ToInt32(cmd.ExecuteScalar());
                    grade.GradeId = newGradeId;
                }
            }
            catch (Exception e) { Console.WriteLine($"Error inserting student grade data: {e.Message}"); }
            return grade;
        }

        public Grades StudentGrade(string username)
        {
            Grades grades = new Grades();
            try
            {
                using (SqlConnection connection = new SqlConnection(_connectionString))
                {
                    connection.Open();

                    SqlCommand cmd = new SqlCommand();
                    string sql = "SELECT sum(grade) AS total_sum FROM GRADES_SMS g " +
                        "JOIN STUDENTS_SMS s ON s.student_id = g.student_id " +
                        "JOIN USERS_SMS u ON u.user_id = s.user_id WHERE u.username = @username;";

                    cmd.CommandText = sql;
                    cmd.Parameters.AddWithValue("@username", username);
                    cmd.Connection = connection;

                    SqlDataReader reader = cmd.ExecuteReader();

                    if (reader.Read())
                    {
                        int totalGrade = reader.GetInt32(0);

                        if (totalGrade != 0)
                        {
                            if(totalGrade >= 80)
                            {
                                Console.WriteLine($"Your current Grade is: A");
                            }
                            else if(totalGrade >= 60 && totalGrade <= 79)
                            {
                                Console.WriteLine($"Your current Grade is: B");
                            }
                            else if(totalGrade >= 40 && totalGrade <= 59)
                            {
                                Console.WriteLine($"Your current Grade is: C");
                            }
                            else
                            {
                                Console.WriteLine($"You need to try hard");
                            }
                        }
                        else
                        {
                            Console.WriteLine("Not Updated! Check back soon!");
                        }
                    }
                }
            }
            catch { }
            return grades;
        }
    }
}
