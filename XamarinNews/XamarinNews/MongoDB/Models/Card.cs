using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace XamarinNews.MongoDB.Models
{
    public class Card
    {
        public string Description { get; set; }
        public string Date { get; set; }
        public string Author { get; set; }
        public int AuthorID { get; set; }
        public ImageSource Image { get; set; }
        public string Likes { get; set; }
        public string Dislikes { get; set; }
    }
}
