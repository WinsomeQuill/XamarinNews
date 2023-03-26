using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using XamarinNews.PostgresSQL.Models;

namespace XamarinNews.Windows
{
    public partial class FullArticle : ContentPage
    {
        public int Id { get; set; }
        public string Reaction { get; set; }

        public FullArticle(Article article)
        {
            InitializeComponent();

            LabelTitle.Text = article.Title;
            ImageHeader.Source = article.Image;
            LabelDescription.Text = article.FullDescription;
            ImageReaderAvatar.Source = Cache.CropAvatar;
            LabelAuthorName.Text = $"{article.Author.FirstName} {article.Author.LastName}";
            ImageAuthorAvatar.Source = article.Author.CropAvatar;
            Id = article.Id;
            LabelLikesCount.Text = $"{article.Likes}";
            LabelDislikesCount.Text = $"{article.Dislikes}";

            Device.InvokeOnMainThreadAsync(async () =>
            {
                Reaction = await Api.GetReactionArticleByUser(Cache.ID, Id);

                if (Reaction == "Нравится")
                {
                    ImageButtonLike.Source = ImageSource.FromFile("pressed_like.png");
                    return;
                }

                if (Reaction == "Не нравится")
                {
                    ImageButtonDislike.Source = ImageSource.FromFile("pressed_dislike.png");
                    return;
                }

                LabelLikeOrDislakeComment.IsVisible = false;
            });

            InitComments();
        }

        private void InitComments()
        {
            new Thread(new ThreadStart(() =>
            {
                Device.InvokeOnMainThreadAsync(async () =>
                {
                    while (true)
                    {
                        List<Comment> comments = await Api.GetArticleComments(Id);
                        ListViewFullArticleComments.ItemsSource = comments;
                        await Task.Delay(60000);
                    }
                });
            })).Start();
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

        private async void ImageButtonLike_Clicked(object sender, EventArgs e)
        {
            if (Reaction != null)
            {
                if (!await Api.RemoveReactionArticle(Cache.ID, Id, Reaction))
                {
                    await DisplayAlert("Ошибка", "Произошла неизвестная ошибка! Попробуйте чуть позже!", "Ок");
                    return;
                }
            }

            ImageButtonLike.Source = ImageSource.FromFile("like.png");
            ImageButtonDislike.Source = ImageSource.FromFile("dislike.png");

            bool result = await Api.InsertReactionArticle(Cache.ID, Id, "Нравится");
            if (!result)
            {
                await DisplayAlert("Ошибка", "Произошла неизвестная ошибка! Попробуйте чуть позже!", "Ок");
                return;
            }

            LabelLikeOrDislakeComment.Text = "Вы считает эту статью полезной!";
            LabelLikeOrDislakeComment.IsVisible = true;
            ImageButtonLike.Source = ImageSource.FromFile("pressed_like.png");
        }

        private async void ImageButtonDislike_Clicked(object sender, EventArgs e)
        {
            if (Reaction != null)
            {
                if (!await Api.RemoveReactionArticle(Cache.ID, Id, Reaction))
                {
                    await DisplayAlert("Ошибка", "Произошла неизвестная ошибка! Попробуйте чуть позже!", "Ок");
                    return;
                }
            }

            ImageButtonLike.Source = ImageSource.FromFile("like.png");
            ImageButtonDislike.Source = ImageSource.FromFile("dislike.png");

            bool result = await Api.InsertReactionArticle(Cache.ID, Id, "Не нравится");
            if (!result)
            {
                await DisplayAlert("Ошибка", "Произошла неизвестная ошибка! Попробуйте чуть позже!", "Ок");
                return;
            }

            LabelLikeOrDislakeComment.Text = "Вы считает эту статью бесполезной!";
            LabelLikeOrDislakeComment.IsVisible = true;
            ImageButtonDislike.Source = ImageSource.FromFile("pressed_dislike.png");
        }
    }
}