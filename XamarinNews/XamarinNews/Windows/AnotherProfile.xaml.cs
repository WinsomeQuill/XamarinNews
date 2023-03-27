using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xamarin.Forms;
using XamarinNews.PostgresSQL.Models;

namespace XamarinNews.Windows
{
    public partial class AnotherProfile : ContentPage
    {
        private int _authorID { get; set; }
        private bool _isSubsribe { get; set; } = false;

        public AnotherProfile(int author_user_id, int follower_user_id)
        {
            InitializeComponent();
            _authorID = author_user_id;
            Init(follower_user_id);
            InitArticles();
        }

        private async void Init(int follower_user_id)
        {
            User user = await Api.UserInfo(_authorID);
            bool isFollowed = await Api.IsUserFollowed(_authorID, follower_user_id);
            _isSubsribe = isFollowed;
            LabelFirstName.Text = user.FirstName.ToString();
            LabelLastName.Text = user.LastName.ToString();
            ImageAvatarUser.Source = user.CropAvatar;

            if (isFollowed)
            {
                SubscribeInfo.Text = "Вы подписаны";
                ButtonSubscribe.Text = "Отписаться";
            }
            else
            {
                SubscribeInfo.Text = "";
                ButtonSubscribe.Text = "Подписаться";
            }

            LabelTimestampRegistration.Text = user.DateRegistration.ToString();
            LabelCountFollowers.Text = $"{await Api.GetUserCountFollowers(_authorID)}";
            LabelDescription.Text = user.About;
            LabelNewsProfileText.Text = $"Последние новости от {LabelFirstName.Text} {LabelLastName.Text}";

            ButtonSubscribe.Clicked += ButtonSubscribe_Clicked;
        }

        private async void InitArticles()
        {
            List<Article> articles = await Api.GetArticlesFromUser(_authorID);
            ListViewProfileNews.ItemsSource = articles;
            ListViewProfileNews.ItemSelected += ListViewProfileNews_ItemSelected;
        }

        private async void ListViewProfileNews_ItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            Article item = (Article)e.SelectedItem;
            await Navigation.PushAsync(new FullArticle(item));
        }

        private async void ButtonSubscribe_Clicked(object sender, EventArgs e)
        {
            if (!_isSubsribe)
            {
                bool result = await Api.FollowingUser(_authorID, Cache.ID);
                if (result)
                {
                    SubscribeInfo.Text = "Вы подписаны";
                    ButtonSubscribe.IsEnabled = false;
                    _isSubsribe = true;
                    await Task.Delay(2000);
                    ButtonSubscribe.IsEnabled = true;
                    ButtonSubscribe.Text = "Отписаться";
                    LabelCountFollowers.Text = $"{await Api.GetUserCountFollowers(_authorID)}";
                }
                return;
            }
            else
            {
                bool result = await Api.RemoveFollowingUser(_authorID, Cache.ID);
                if (result)
                {
                    SubscribeInfo.Text = "Вы отписались";
                    ButtonSubscribe.IsEnabled = false;
                    await Task.Delay(2000);
                    ButtonSubscribe.IsEnabled = true;
                    SubscribeInfo.Text = "";
                    ButtonSubscribe.Text = "Подписаться";
                    _isSubsribe = false;
                    LabelCountFollowers.Text = $"{await Api.GetUserCountFollowers(_authorID)}";
                }
            }
        }
    }
}