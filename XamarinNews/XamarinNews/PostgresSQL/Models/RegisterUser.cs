using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace XamarinNews.PostgresSQL.Models
{
    public class RegisterUser
    {
        [JsonProperty("first_name")]
        public string FirstName { get; set; }

        [JsonProperty("last_name")]
        public string LastName { get; set; }

        [JsonProperty("about")]
        public string About { get; set; }

        [JsonProperty("login")]
        public string Login { get; set; }

        [JsonProperty("password")]
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
