using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using XamarinNews.PostgresSQL.Models;

namespace XamarinNews.Windows
{
    public partial class FullArticle : ContentPage
    {
        private List<FullArticleComment> _Comments { get; set; } = new List<FullArticleComment>();
        public FullArticle(int author_id = 0)
        {
            InitializeComponent();

            ImageSource image = ImageSource.FromResource("testcardimage.png");

            for (int i = 0; i < 10; i++)
            {
                _Comments.Add(new FullArticleComment() {
                    Date = "01.01.1970",
                    FirstName = "Alex",
                    LastName = "Maroni",
                    Message = "Test message",
                    Image = image,
                });
            }

            ListViewFullArticleComments.ItemsSource = _Comments;
        }
    }
}