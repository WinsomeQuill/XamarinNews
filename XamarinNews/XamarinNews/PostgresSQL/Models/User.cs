﻿using Newtonsoft.Json;
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
        public User(string crop_avatar, string full_avatar)
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