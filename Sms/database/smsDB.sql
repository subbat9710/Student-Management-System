-- Create the database
USE master;
GO

DROP DATABASE IF EXISTS Studentms;

CREATE DATABASE Studentms;
GO

USE Studentms;
GO

BEGIN TRANSACTION;


-- Create the USERS table
CREATE TABLE USERS_SMS (
  user_id INT PRIMARY KEY IDENTITY(1000, 1),
  username VARCHAR(50) UNIQUE NOT NULL,
  password VARCHAR(50) NOT NULL,
  salt VARCHAR(256) NOT NULL,
  role VARCHAR(20) NOT NULL,
);

-- Create the STUDENTS table
CREATE TABLE STUDENTS_SMS (
  student_id INT PRIMARY KEY IDENTITY(2000, 1),
  first_name VARCHAR(50) NOT NULL,
  last_name VARCHAR(50) NOT NULL,
  date_of_birth DATE NOT NULL,
  user_id INT NOT NULL,
  CONSTRAINT FK_student_user FOREIGN KEY (user_id) REFERENCES USERS_SMS(user_id)
);

-- Create the COURSES table
CREATE TABLE COURSES_SMS (
  course_id INT PRIMARY KEY IDENTITY(4000, 1),
  course_name VARCHAR(50) NOT NULL,
  start_date DATE NOT NULL,
  status VARCHAR(30),
  instructor_id INT NOT NULL,
  description VARCHAR(200),
  total_hours int NOT NULL
);

-- Create the INSTRUCTORS table
CREATE TABLE INSTRUCTORS_SMS (
  instructor_id INT PRIMARY KEY IDENTITY(3000, 1),
  first_name VARCHAR(50) NOT NULL,
  last_name VARCHAR(50) NOT NULL,
  date_of_birth DATE NOT NULL,
  user_id INT NOT NULL,
  CONSTRAINT FK_instructor_user FOREIGN KEY (user_id) REFERENCES USERS_SMS(user_id)
);

-- Create the GRADES table
CREATE TABLE GRADES_SMS (
  grade_id INT PRIMARY KEY IDENTITY(5000, 1),
  student_id INT NOT NULL,
  course_id INT NOT NULL,
  grade INT NOT NULL,
  assignment_name VARCHAR(50),
  CONSTRAINT FK_student_grade FOREIGN KEY (student_id) REFERENCES STUDENTS_SMS(student_id),
  CONSTRAINT FK_grade_course FOREIGN KEY (course_id) REFERENCES COURSES_SMS(course_id)
);

-- Create the ATTENDANCE table
CREATE TABLE ATTENDANCE_SMS (
  attendance_id INT PRIMARY KEY IDENTITY(6000, 1),
  student_id INT NOT NULL,
  course_id INT NOT NULL,
  date DATE NOT NULL,
  status VARCHAR(10) NOT NULL,
  CONSTRAINT FK_attendance_student FOREIGN KEY (student_id) REFERENCES STUDENTS_SMS(student_id),
  CONSTRAINT FK_attendance_course FOREIGN KEY (course_id) REFERENCES COURSES_SMS(course_id)
);
ALTER TABLE COURSES_SMS ADD CONSTRAINT FK_course_instructor FOREIGN KEY (instructor_id) REFERENCES INSTRUCTORS_SMS(instructor_id)
COMMIT;