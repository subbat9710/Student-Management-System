using Sms.DAO;
using Sms.Model;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Text;
using System.Transactions;

namespace Sms
{
    internal class UserCli
    {
        private readonly IUserDao _userDao;
        private readonly IStudentDao _newStudentDao;
        private readonly IInstructorDao _newInstructorDao;
        private readonly ICourseDao _courseDao;
        private readonly IGradeDao _gradeDao;
        private readonly IAttendanceDao _attendanceDao;

        public UserCli(IUserDao userDao, IStudentDao newStudentDao, IInstructorDao instructorDao, ICourseDao courseDao, IGradeDao gradeDao, IAttendanceDao attendanceDao)
        {
            _userDao = userDao;
            _newStudentDao = newStudentDao;
            _newInstructorDao = instructorDao;
            _courseDao = courseDao;
            _gradeDao = gradeDao;
            _attendanceDao = attendanceDao;
        }
        private User LoggedInUser { get; set; }
        public void Run()
        {
            while (true)
            {
                PrintMenu();
                string option = AskPrompt().ToLower();

                if (option == "1")
                {
                    LoginUser();
                }
                else if (option == "2")
                {
                    break;

                }
                else if (option == "3")
                {
                    AddNewStudent();
                }
                else
                {
                    Console.WriteLine($"{option} is not a valid option. Try again!");
                }
            }
        }
        private void AddNewStudent()
        {
            Console.WriteLine("Creating new user for our student management system: ");
            Console.Write("Username: ");
            string username = Console.ReadLine().ToLower().Trim();
            Console.Write("Password: ");
            string password = "";
            ConsoleKeyInfo key;
            do
            {
                key = Console.ReadKey(true);
                //ignore any non-character keys
                if (key.Key != ConsoleKey.Enter && key.KeyChar != '\u0000')
                {
                    password += key.KeyChar;
                    Console.Write("*");
                }
            } while (key.Key != ConsoleKey.Enter);
            Console.WriteLine();

            Console.Write("Role: ");
            string role = Console.ReadLine().ToLower().Trim();
            User user = _userDao.SaveUser(username, password, role);
            if(user != null)
            {
                Console.WriteLine($"New: {user.Role}: {user.Username} added with id: {user.Id}!");
                Console.WriteLine();
            }
            else
            {

            }
        }
        private string AskPrompt()
        {
            Console.Write("Press One to Login: ");
            return Console.ReadLine();
        }

        private void LoginUser()
        {
            Console.WriteLine();
            Console.WriteLine("Log into the Student Management System?");
            Login();
        }
        private void DisplayStudents(IList<Student> students)
        {
            Console.WriteLine();
            if(students.Count > 0)
            {
                foreach(Student std in students)
                {
                    Console.WriteLine(std);
                }
            }
            else
            {
                Console.WriteLine("\n*** No Results ***");
            }
        }
        private void ListAllstudents()
        {
            Console.WriteLine();
            IList<Student> students = _newStudentDao.GetAllStudent();
            Console.WriteLine($"Total Students in SMS: {students.Count}");
            DisplayStudents(students);
        }
        private void DisplayInstructor(IList<Instructor> instructors)
        {
            Console.WriteLine();
            if(instructors.Count > 0)
            {
                foreach(Instructor item in instructors)
                {
                    Console.WriteLine(item);
                }
            }
            else
            {
                Console.WriteLine("\n*** No Results ***");
            }
        }
        private void ListAllInstructor()
        {
            Console.WriteLine();
            IList<Instructor> instructors = _newInstructorDao.GetAllInstructor();
            Console.WriteLine($"Total Instructors in SMS: {instructors.Count}");
            DisplayInstructor(instructors);
        }
        private void DisplayAllCourse(IList<Course> courses)
        {
            Console.WriteLine();
            if(courses.Count > 0)
            {
                foreach(Course item in courses)
                {
                    Console.WriteLine(item);
                }
            }
            else
            {
                Console.WriteLine("\n*** No Results ***");
            }
        }
        private void ListAllCourses()
        {
            Console.WriteLine();
            IList<Course> courses = _courseDao.GetAllCourse();
            Console.WriteLine($"Total Courses in SMS: {courses.Count}");
            DisplayAllCourse(courses);
        }
        private void ListCourseByName(IList<Course> courses)
        {
            Console.WriteLine();
            if (courses.Count > 0)
            {
                foreach (Course item in courses)
                {
                    Console.WriteLine(item);
                }
            }
            else
            {
                Console.WriteLine("\n*** No Results ***");
            }
        }
        public void Login()
        {
            Console.Write("Username: ");
            string username = Console.ReadLine().ToLower().Trim();
            Console.Write("Password: ");
            String password = "";
            ConsoleKeyInfo key;
            do
            {
                key = Console.ReadKey(true);
                if (key.Key != ConsoleKey.Enter && key.KeyChar != '\u0000')
                {
                    password += key.KeyChar;
                    Console.Write("*");
                }
            } while (key.Key != ConsoleKey.Enter);
            Console.WriteLine();

            if (_userDao.IsUsernameAndPasswordValid(username, password))
            {
                LoggedInUser = new User();
                LoggedInUser.Username = username;

                User user = _userDao.SelectUserRole(username, password);
                if (user != null)
                {
                    if (user.Role == "admin")
                    {
                        Admin();
                    }
                    else if (user.Role == "student")
                    {
                        while (true)
                        {
                            Console.WriteLine();
                            Student student = new Student();
                            student = _newStudentDao.GetFirstLastName(username);
                            Console.WriteLine();
                            Console.WriteLine("1) Course details");
                            Console.WriteLine("2) My Current Grade ");
                            Console.WriteLine("3) Logout");
                            string studentInput = Console.ReadLine().Trim();
                            if (studentInput == "1")
                            {
                                IList<Course> courses = _courseDao.GetCoursesByUsername(username);
                                DisplayAllCourse(courses);
                            }
                            else if (studentInput == "2")
                            {
                                Console.WriteLine();
                                Grades grades = _gradeDao.StudentGrade(username);
                            }
                            else if (studentInput == "3")
                            {
                                break;
                            }
                        }

                    }
                    else if (user.Role == "instructor")
                    {
                        Console.WriteLine();
                        Instructor instructor = new Instructor();
                        instructor = _newInstructorDao.GetFirstLastName(username);

                        while (true)
                        {
                            Console.WriteLine();
                            Console.WriteLine("1) Course details");
                            Console.WriteLine("2) Grade Student");
                            Console.WriteLine("3) Attendance");
                            Console.WriteLine("4) Logout");
                            string instructorInput = Console.ReadLine().Trim();
                            if (instructorInput == "1")
                            {
                                IList<Course> courses = _courseDao.GetCoursesByUsernameInstructor(username);
                                DisplayAllCourse(courses);
                            }
                            else if (instructorInput == "2")
                            {
                                Console.Write("Enter first name to search for: ");
                                string firstNameSearch = Console.ReadLine();
                                Console.Write("Enter last name to search for: ");
                                string lastNameSearch = Console.ReadLine();
                                IList<Student> students = _newStudentDao.SearchStudentByName(firstNameSearch, lastNameSearch);
                                DisplayStudents(students);
                                if (students.Count == 0)
                                {
                                    Console.WriteLine("No student found with the given first and last name.");
                                    return;
                                }
                                else if (students.Count > 1)
                                {
                                    Console.WriteLine("Multiple students found with a given first and last name.");
                                    return;
                                }
                                else
                                {
                                    Console.WriteLine();
                                    Console.Write("1) Enter Student Id: ");
                                    string studentId = Console.ReadLine();
                                    Console.Write("3) Grade Student: ");
                                    string gradeStd = Console.ReadLine();
                                    Console.Write("4) Assignment Name: ");
                                    string assignment = Console.ReadLine();

                                    Grades newGrade = new Grades()
                                    {
                                        StudentId = int.Parse(studentId),
                                        CourseId = _newInstructorDao.GettingCourseId(username),
                                        Grade = int.Parse(gradeStd),
                                        AssignmentName = assignment
                                    };
                                    newGrade = _gradeDao.GetGrade(newGrade);
                                    Console.WriteLine($"\n*** {newGrade.Grade} assigned ***");
                                }
                            }
                            else if (instructorInput == "3")
                            {
                                Console.Write("1) Enter Student Id: ");
                                string studentId = Console.ReadLine();
                                Console.Write("3) Is Present/Absent: ");
                                string status = Console.ReadLine();
                                Attendance getAttendance = new Attendance()
                                {
                                    StudentId = int.Parse(studentId),
                                    CourseId = _newInstructorDao.GettingCourseId(username),
                                    Date = DateTime.Now,
                                    Status = status
                                };
                                getAttendance = _attendanceDao.GetAttendance(getAttendance);
                                Console.WriteLine("Attendence Created");
                            }
                            else if (instructorInput == "4")
                            {
                                break;
                            }
                        }
                    }
                    else
                    {
                        Console.WriteLine("Your role is not assigned!");
                    }
                }
            }
            else
            {
                Console.WriteLine();
                Console.WriteLine("Login Has Failed. Please try again!");
            }

            Console.WriteLine();
        }
        private void AddCourseDetails() 
        {
            Console.Write("Enter course name: ");
            string courseName = Console.ReadLine();
            Console.Write("Course Start Date: ");
            string startDate = Console.ReadLine();
            Console.Write("Course Status: ");
            string courseStatus = Console.ReadLine();
            Console.Write("Total Hours: ");
            string totalHours = Console.ReadLine();
            Console.Write("Instructor Id: ");
            string instructorId = Console.ReadLine();
            Console.Write("Course Descriptions: ");
            string courseDescription = Console.ReadLine();
            Course newCourse = new Course()
            {
                CourseName = courseName,
                StartDate = DateTime.Parse(startDate),
                Status = courseStatus,
                TotalHours = int.Parse(totalHours),
                //   UserId = user.Id,  //PK from user table storing to FK student table
                InstructorId = int.Parse(instructorId),
                Description = courseDescription
            };
            newCourse = _courseDao.GetCourse(newCourse);
            Console.WriteLine($"\n*** {newCourse.CourseName} course created ***");
        }
        private void AddStudentDetails()
        {
            Console.Write("Enter your first name: ");
            string firstName = Console.ReadLine();
            Console.Write("Enter your last name: ");
            string lastName = Console.ReadLine();
            Console.Write("Enter your date of birth: ");
            string dateOfBirth = Console.ReadLine();
            Console.Write("Enter user id: ");
            string userId = Console.ReadLine();
            Student newUser = new Student
            {
                FirstName = firstName,
                LastName = lastName,
                DateOfBirth = DateTime.Parse(dateOfBirth),
                UserId = int.Parse(userId)
             //   UserId = user.Id,  //PK from user table storing to FK student table
            };
            newUser = _newStudentDao.createProfileDetails(newUser);
            Console.WriteLine($"\n*** {newUser.FirstName} {newUser.LastName} created ***");
        }
        private void InstructorDetails() 
        {
            Console.Write("Enter your first name: ");
            string firstName = Console.ReadLine();
            Console.Write("Enter your last name: ");
            string lastName = Console.ReadLine();
            Console.Write("Enter your date of birth: ");
            string dateOfBirth = Console.ReadLine();
            Console.Write("Enter user Id: ");
            string userId = Console.ReadLine();
            Instructor newUser = new Instructor
            {
                FirstName = firstName,
                LastName = lastName,
                DateOfBirth = DateTime.Parse(dateOfBirth),
                UserId =  int.Parse(userId) //PK from user table storing to FK student table
            };
            newUser = _newInstructorDao.createProfileDetails(newUser);
            Console.WriteLine($"\n*** {newUser.FirstName} {newUser.LastName} created ***");
        }
        public void StudentSearch()
        {
            Console.Write("Enter first name to search for: ");
            string firstNameSearch = Console.ReadLine();
            Console.Write("Enter last name to search for: ");
            string lastNameSearch = Console.ReadLine();
            IList<Student> students = _newStudentDao.SearchStudentByName(firstNameSearch, lastNameSearch);
            if(students.Count == 0) { Console.WriteLine(); Console.WriteLine("No student found with the given first and last name."); return; }
            else if (students.Count > 1) { Console.WriteLine(); Console.WriteLine("Multiple students found with a given first and last name."); return; }
            else { DisplayStudents(students); }
            Student studentToUpdate = students[0];
            Console.Write("Enter student ID: ");
            string studentId = Console.ReadLine();
            Console.Write("Enter first name to Update: ");
            string firstNameToUpdate = Console.ReadLine();
            Console.Write("Enter last name to Update: ");
            string lastNameToUpdate = Console.ReadLine();
            Student updateStudent = new Student()
            {
                StudentId = int.Parse(studentId),
                FirstName = firstNameToUpdate,
                LastName = lastNameToUpdate
            };
            bool success = _newStudentDao.UpdateStudent(updateStudent);
            Console.WriteLine();
            if (success) { Console.WriteLine($"Student Updated Successfully. New Updated Name: {updateStudent.FirstName} {updateStudent.LastName}!"); }
            else { Console.WriteLine("Error Updating Student"); }
        }
        public void DeleteStudent()
        {
            Console.Write("Enter first name to search for: ");
            string firstNameSearch = Console.ReadLine();
            Console.Write("Enter last name to search for: ");
            string lastNameSearch = Console.ReadLine();
            IList<Student> students = _newStudentDao.SearchStudentByName(firstNameSearch, lastNameSearch);
            DisplayStudents(students);
            if (students.Count == 0) { Console.WriteLine("No student found with the given first and last name."); return; }
            else if (students.Count > 1) { Console.WriteLine("Multiple students found with a given first and last name."); return; }
            Student studentToUpdate = students[0];
            Console.Write("Enter student ID: ");
            int studentId = int.Parse(Console.ReadLine());
            bool success = _newStudentDao.RemoveStudent(studentId);
            Console.WriteLine();
            if (success) { Console.WriteLine($"Student Deleted Successfully."); }
            else { Console.WriteLine("Error Deleting Student"); }
        }
        private void Admin()
        {
            while (true)
            {
                Console.WriteLine();
                Console.WriteLine("You Logged in as admin!");
                Console.WriteLine();
                Console.WriteLine("1) Add new instructor");
                Console.WriteLine("2) Add new course");
                Console.WriteLine("3) Add new student");
                Console.WriteLine("4) View students");
                Console.WriteLine("5) View instructor");
                Console.WriteLine("6) Course details");
                Console.WriteLine("7) Update student");
                Console.WriteLine("8) Delete Student Record ");
                Console.Write("9) Logout ");
                string adminInput = Console.ReadLine();
                if (adminInput == "1") { InstructorDetails(); }
                else if (adminInput == "2") { AddCourseDetails(); }
                else if (adminInput == "3") { AddStudentDetails(); }
                else if (adminInput == "4") { ListAllstudents(); }
                else if (adminInput == "5") { ListAllInstructor(); }
                else if (adminInput == "6") { ListAllCourses(); }
                else if (adminInput == "7") { StudentSearch(); }
                else if (adminInput == "8") { DeleteStudent(); }
                else if (adminInput == "9") { break; }
                else { Console.WriteLine("Invalid Input!"); }
            }
        }
        private void GetAttendance()
        {
            Console.Write("1) Enter Student Id: ");
            string studentId = Console.ReadLine();
            Console.Write("2) Enter Course Id: ");
            string courseId = Console.ReadLine();
            Console.Write("3) Is Present/Absent: ");
            string status = Console.ReadLine();
            Attendance getAttendance = new Attendance()
            {
                StudentId = int.Parse(studentId),
                CourseId = int.Parse(courseId),
                Date = DateTime.Now,
                Status = status
            };
            getAttendance = _attendanceDao.GetAttendance(getAttendance);
            Console.WriteLine("Attendence Created");
        }

        private void PrintMenu()
        {
            Console.WriteLine();
            Console.WriteLine("Welcome to the Student Management System!");
            Console.WriteLine("1) Login");
            Console.WriteLine("2) Exit");
            Console.WriteLine("********************");
        }
    }
}