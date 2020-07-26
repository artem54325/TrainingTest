using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TrainingTests.ViewModels
{
    public class UserView
    {
        public string Username { get; set; }
        public string AccessToken { get; set; }
    }

    public class SettingsView
    {

    }
    public class RegistrationView
    {
        public string Username { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string Role { get; set; }
        public string Firstname { get; set; }
        public string Secondname { get; set; }
        public DateTime DateBirth { get; set; }
        public string Group { get; set; }
        public string Department { get; set; }
        public string Discipline { get; set; }
    }
}
