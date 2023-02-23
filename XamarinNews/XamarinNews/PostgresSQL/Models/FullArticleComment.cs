using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace XamarinNews.PostgresSQL.Models
{
    public class FullArticleComment
    {
        public string Message { get; set; }
        public string Date { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public ImageSource Image { get; set; }
    }
}
