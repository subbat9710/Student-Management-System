USE master;
GO

DROP DATABASE IF EXISTS Studentms;

CREATE DATABASE Studentms;
GO

USE Studentms;
GO

BEGIN TRANSACTION;

CREATE TABLE users_sms (
  id                INT IDENTITY PRIMARY KEY,
  username          VARCHAR(255) NOT NULL UNIQUE,
  password          VARCHAR(48) NOT NULL,
  salt              VARCHAR(256) NOT NULL,
  role              VARCHAR(50) NOT NULL
);

CREATE TABLE student_sms (
  student_id        INT IDENTITY PRIMARY KEY,
  first_name        VARCHAR(50) NOT NULL,
  last_name         VARCHAR(100) NOT NULL,
  date_of_birth     DATE NOT NULL,
  user_id           INT NOT NULL,
  completed_hours   INT NOT NULL DEFAULT 0,
  course_id         INT NOT NULL,

  CONSTRAINT FK_student_user FOREIGN KEY (user_id) REFERENCES users_sms(id),
 
);

CREATE TABLE instructor_sms (
  instructor_id     INT IDENTITY PRIMARY KEY,
  first_name        VARCHAR(50) NOT NULL,
  last_name         VARCHAR(100) NOT NULL,
  date_of_birth     DATE NOT NULL,
  user_id           INT NOT NULL

  CONSTRAINT FK_instructor_user FOREIGN KEY (user_id) REFERENCES users_sms(id),
 
);

CREATE TABLE course_sms (
  course_id         INT IDENTITY PRIMARY KEY,
  name              VARCHAR(80) NOT NULL,
  start_date        DATE,
  status            VARCHAR(50),
  total_hours       INT NOT NULL,
  learn_hour        INT,
  instructor_id     INT NOT NULL,
  description       VARCHAR(255),

  CONSTRAINT FK_course_instructor FOREIGN KEY (instructor_id) REFERENCES instructor_sms(instructor_id)
);

ALTER TABLE student_sms ADD  CONSTRAINT FK_student_course FOREIGN KEY (course_id) REFERENCES course_sms(course_id)
COMMIT;
