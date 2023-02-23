using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace XamarinNews.PostgresSQL.Models
{
    public class User
    {
        [JsonProperty("id")]
        public int Id { get; set; } = 0;

        [JsonProperty("first_name")]
        public string FirstName { get; set; }

        [JsonProperty("last_name")]
        public string LastName { get; set; }

        [JsonProperty("description")]
        public string Description { get; set; }

        [JsonProperty("login")]
        public string Login { get; set; }

        [JsonProperty("password")]
        public string Password { get; set; }

        [JsonProperty("date_registration")]
        public string DateRegistration { get; set; } = null;

        [JsonProperty("crop_avatar")]
        public ImageSource CropAvatar { get; set; } = null;

        [JsonProperty("full_avatar")]
        public ImageSource FullAvatar { get; set; } = null;

        public User(string firstName, string lastName, string description, string login, string password)
        {
            FirstName = firstName;
            LastName = lastName;
            Description = description;
            Login = login;
            Password = password;
        }

        public User(string firstName, string lastName, string description, string login)
        {
            FirstName = firstName;
            LastName = lastName;
            Description = description;
            Login = login;
        }
    }
}
