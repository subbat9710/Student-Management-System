using Sms.Model;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Text;

namespace Sms.DAO
{
    internal class GradeSqlDao : IGradeDao
    {
        private readonly string _connectString;

        public GradeSqlDao(string connString)
        {
            _connectString = connString;
        }

        public Grades GetGrade(Grades grade)
        {
            int newGradeId = 0;
            try
            {
                using (SqlConnection connection = new SqlConnection(_connectString))
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
            catch (Exception e) { Console.WriteLine($"Error inserting data: {e.Message}"); }
            return grade;
        }
    }
}
