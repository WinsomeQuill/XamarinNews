using Newtonsoft.Json.Linq;
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

        public FullArticle(Article article)
        {
            InitializeComponent();

            LabelTitle.Text = article.Title;
            ImageHeader.Source = article.Image;
            LabelDescription.Text = article.Description;
            ImageReaderAvatar.Source = Cache.CropAvatar;
            LabelAuthorName.Text = $"{article.Author.FirstName} {article.Author.LastName}";
            ImageAuthorAvatar.Source = article.Author.CropAvatar;

            InitComments();
        }

        private void InitComments()
        {
            ImageSource image = ImageSource.FromResource("testcardimage.png");

            for (int i = 0; i < 10; i++)
            {
                _Comments.Add(new FullArticleComment()
                {
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