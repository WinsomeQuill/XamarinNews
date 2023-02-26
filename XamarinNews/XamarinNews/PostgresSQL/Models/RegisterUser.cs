using System;
using System.Collections.Generic;
using System.Text;

namespace XamarinNews.PostgresSQL.Models
{
    public class RegisterUser
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string About { get; set; }
        public string Login { get; set; }
        public string Password { get; set; }

        public RegisterUser(string firstName, string lastName, string about, string login, string password)
        {
            FirstName = firstName;
            LastName = lastName;
            About = about;
            Login = login;
            Password = password;
        }
    }
}
