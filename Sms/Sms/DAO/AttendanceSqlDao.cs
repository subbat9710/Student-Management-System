using Sms.Model;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Text;

namespace Sms.DAO
{
    internal class AttendanceSqlDao : IAttendanceDao
    {
        private readonly string _connectString;

        public AttendanceSqlDao(string connString)
        {
            _connectString = connString;
        }
        public Attendance GetAttendance(Attendance attendance)
        {
            int attendanceId= 0;
            try
            {
                using (SqlConnection connection = new SqlConnection(_connectString))
                {
                    connection.Open();
                    SqlCommand cmd = new SqlCommand();
                    string sql = "INSERT INTO ATTENDANCE_SMS (student_id, course_id, date, status) " +
                        "OUTPUT INSERTED.attendance_id " +
                        "VALUES (@student_id, @course_id, @date, @status) " +
                        "SELECT SCOPE_IDENTITY(); ";
                    cmd.CommandText = sql;
                    cmd.Parameters.AddWithValue("@student_id", attendance.StudentId);
                    cmd.Parameters.AddWithValue("@course_id", attendance.CourseId);
                    cmd.Parameters.AddWithValue("@date", attendance.Date);
                    cmd.Parameters.AddWithValue("@status", attendance.Status);
                    cmd.Connection = connection;

                    attendanceId = Convert.ToInt32(cmd.ExecuteScalar());
                    attendance.AttendanceId = attendanceId;
                }
            }
            catch (Exception e) { Console.WriteLine($"Error inserting student grade data: {e.Message}"); }
            return attendance;
        }
    }
}
