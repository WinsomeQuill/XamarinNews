using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Xamarin.Forms;

namespace XamarinNews.PostgresSQL.Models
{
    public class Article
    {
        [JsonProperty("id")]
        public int Id { get; set; }

        [JsonProperty("author")]
        public User Author { get; set; }

        [JsonIgnore]
        public ImageSource Image { get; set; }

        [JsonProperty("title")]
        public string Title { get; set; }

        [JsonProperty("description")]
        public string Description { get; set; }

        [JsonProperty("publish_date")]
        public DateTime PublishDate { get; set; }

        [JsonProperty("likes")]
        public int Likes { get; set; }

        [JsonProperty("dislikes")]
        public int Dislikes { get; set; }

        public Article() { }

        [JsonConstructor]
        public Article(byte[] image)
        {
            Image = ImageSource.FromStream(() =>
            {
                return new MemoryStream(image);
            });
        }
    }
}
