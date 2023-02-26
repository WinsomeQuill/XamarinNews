using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Xamarin.Forms;

namespace XamarinNews.PostgresSQL.Models
{
    public class FullUser
    {
        [JsonProperty("id")]
        public int Id { get; set; }

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

        [JsonProperty("date_registration")]
        public string DateRegistration { get; set; }

        [JsonIgnore]
        public ImageSource CropAvatar { get; set; }

        [JsonIgnore]
        public ImageSource FullAvatar { get; set; }


        public FullUser(string firstName, string lastName, string about, string login)
        {
            FirstName = firstName;
            LastName = lastName;
            About = about;
            Login = login;
        }

        [JsonConstructor]
        public FullUser(string crop_avatar, string full_avatar)
        {
            CropAvatar = ImageSource.FromStream(() =>
            {
                return new MemoryStream(Convert.FromBase64String(crop_avatar));
            });

            FullAvatar = ImageSource.FromStream(() =>
            {
                return new MemoryStream(Convert.FromBase64String(full_avatar));
            });
        }
    }
}
