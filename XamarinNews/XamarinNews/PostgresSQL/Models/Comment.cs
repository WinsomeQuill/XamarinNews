using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace XamarinNews.PostgresSQL.Models
{
    public class Comment
    {
        [JsonProperty("id")]
        public int Id { get; set; }

        [JsonProperty("author")]
        public User Author { get; set; }

        [JsonProperty("message")]
        public string Message { get; set; }

        [JsonProperty("publish_date")]
        public DateTime PublishDate { get; set; }
    }
}
