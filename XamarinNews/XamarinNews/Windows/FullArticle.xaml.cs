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
        public int Id { get; set; }
        private List<Comment> _Comments { get; set; } = new List<Comment>();

        public FullArticle(Article article)
        {
            InitializeComponent();

            LabelTitle.Text = article.Title;
            ImageHeader.Source = article.Image;
            LabelDescription.Text = article.Description;
            ImageReaderAvatar.Source = Cache.CropAvatar;
            LabelAuthorName.Text = $"{article.Author.FirstName} {article.Author.LastName}";
            ImageAuthorAvatar.Source = article.Author.CropAvatar;
            Id = article.Id;

            InitComments();
        }

        private async void InitComments()
        {
            List<Comment> comments = await Api.GetArticleComments(Id);
            ListViewFullArticleComments.ItemsSource = _Comments = comments;
        }

        private async void ImageButtonSendComment_Clicked(object sender, EventArgs e)
        {
            string message = EntryMessage.Text;

            if (string.IsNullOrWhiteSpace(message))
            {
                await DisplayAlert("Ошибка", "Нельзя отправить пустой комментарий!", "Ок");
                return;
            }

            if (message.Length < 5)
            {
                await DisplayAlert("Ошибка", "Слишком короткий комментарий!", "Ок");
                return;
            }

            bool result = await Api.InsertArticleComment(Cache.ID, Id, message);
            if (!result)
            {
                await DisplayAlert("Ошибка", "Произошла неизвестная ошибка! Попробуйте чуть позже!", "Ок");
                return;
            }

            await DisplayAlert("Успешно", "Вы добавили комментарий к записи!", "Ок");
            EntryMessage.Text = null;
        }
    }
}