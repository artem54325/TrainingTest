using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Newtonsoft.Json;

namespace TrainingTests.Models
{
    public class SuperUser : User
    {
        public Roles Role => Roles.SuperUser;
    }
    //https://metanit.com/sharp/mvc5/25.1.php
    //https://www.robinwieruch.de/react-list-component
    //https://metanit.com/sharp/aspnet5/25.1.php
    //https://reactjs.net/tutorials/aspnetcore.html

    public class TeacherUser : User
    {
        public Roles Role => Roles.Teacher;

        public string Discipline { get; set; }

        public string Department { get; set; }
        [JsonIgnore]
        public List<Test> Tests { get; set; }
    }
    public class StudentUser : User
    {
        public Roles Role => Roles.Student;

        public string Group { get; set; }

        public string Department { get; set; }
        [JsonIgnore]
        public List<TestStudent> TestStudents { get; set; }
    }

    public class User
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity), JsonIgnore]
        public string Id { get; set; }
        [Required]
        public string Username { get; set; }
        [Required, DataType(DataType.EmailAddress)]
        public string Email { get; set; }
        [Required, DataType(DataType.Password), JsonIgnore]
        public string Password { get; set; }


        public Roles Role { get; }

        public string Firstname { get; set; }
        public string Secondname { get; set; }

        public DateTime DateBirth { get; set; }

        public DateTime DateRegistration { get; set; }
    }


    public enum Roles
    {
        SuperUser, Teacher, Student, User
    }
}
