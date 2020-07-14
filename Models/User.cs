using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;

namespace TrainingTests.Models
{
    public class SuperUser : User
    {
        public override Roles Role => Roles.SuperUser;
    }
    //https://metanit.com/sharp/mvc5/25.1.php
    //https://www.robinwieruch.de/react-list-component
    //https://metanit.com/sharp/aspnet5/25.1.php
    //https://reactjs.net/tutorials/aspnetcore.html

    public class TeacherUser : User
    {
        public override Roles Role => Roles.Teacher;
        [JsonIgnore]
        public string Discipline { get; set; }
        [JsonIgnore]
        public string Department { get; set; }
        [JsonIgnore]
        public List<Test> Tests { get; set; }
    }
    public class StudentUser : User
    {
        public override Roles Role => Roles.Student;
        [JsonIgnore]
        public string Group { get; set; }
        [JsonIgnore]
        public string Department { get; set; }
        [JsonIgnore]
        public List<TestStudent> TestStudents { get; set; }
    }

    public abstract class User
    {
        [Key, JsonIgnore]
        public string Id { get; set; }
        [Required, JsonIgnore]
        public string Username { get; set; }
        [Required, DataType(DataType.EmailAddress), JsonIgnore]
        public string Email { get; set; }
        [Required, DataType(DataType.Password), JsonIgnore]
        public string Password { get; set; }
        [JsonIgnore]
        public abstract Roles Role { get; }

        public string Firstname { get; set; }
        public string Secondname { get; set; }
        [JsonIgnore]
        public string Token { get; set; }

        //[DisplayFormat(DataFormatString = "{0:dd.MM.yyyy HH:mm:ss}", ApplyFormatInEditMode = true)]
        [JsonIgnore]
        public DateTime DateRegistration { get; set; }
    }
    public enum Roles
    {
        SuperUser, Teacher, Student, User
    }
}
