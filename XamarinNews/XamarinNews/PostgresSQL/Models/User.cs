using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Xamarin.Forms;

namespace XamarinNews.PostgresSQL.Models
{
    public class User
    {
        [JsonProperty("id")]
        public int Id { get; set; }

        [JsonProperty("first_name")]
        public string FirstName { get; set; }

        [JsonProperty("last_name")]
        public string LastName { get; set; }

        [JsonProperty("about")]
        public string About { get; set; }

        [JsonIgnore]
        public ImageSource CropAvatar { get; set; }

        [JsonIgnore]
        public ImageSource FullAvatar { get; set; }

        [JsonProperty("date_registration")]
        public string DateRegistration { get; set; }

        [JsonConstructor]
        public User(byte[] crop_avatar, byte[] full_avatar)
        {
            CropAvatar = ImageSource.FromStream(() => new MemoryStream(crop_avatar));

            FullAvatar = ImageSource.FromStream(() => new MemoryStream(full_avatar));
        }
    }
}
