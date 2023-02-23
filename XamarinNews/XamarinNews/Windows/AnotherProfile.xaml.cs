using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using XamarinNews.PostgresSQL.Models;

namespace XamarinNews.Windows
{
    public partial class AnotherProfile : ContentPage
    {
        private List<Card> _List { get; set; } = new List<Card>();
        private int _authorID { get; set; }
        private bool _isSubsribe { get; set; } = false;

        public AnotherProfile(int author_user_id, int follower_user_id)
        {
            InitializeComponent();
            _authorID = author_user_id;
            Init(follower_user_id);
        }

        private async void Init(int follower_user_id)
        {
            JObject user = await Api.UserInfo(_authorID);
            bool isFollowed = await Api.IsUserFollowed(_authorID, follower_user_id);
            _isSubsribe = isFollowed;
            LabelFirstName.Text = user["message"]["first_name"].ToString();
            LabelLastName.Text = user["message"]["last_name"].ToString();

            if (user["message"]["crop_avatar"] != null)
            {
                byte[] crop_avatar = Convert.FromBase64String(user["message"]["crop_avatar"].ToString());
                ImageAvatarUser.Source = ImageSource.FromStream(() =>
                {
                    return new MemoryStream(crop_avatar);
                });
            }

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

            LabelTimestampRegistration.Text = user["message"]["date_registration"].ToString();
            LabelCountFollowers.Text = $"{await Api.GetUserCountFollowers(_authorID)}";
            LabelDescription.Text = user["message"]["description"].ToString();
            LabelNewsProfileText.Text = $"Последние новости от {LabelFirstName.Text} {LabelLastName.Text}";

            ButtonSubscribe.Clicked += ButtonSubscribe_Clicked;

            for (int i = 0; i < 10; i++)
            {
                ImageSource image = ImageSource.FromResource("testcardimage.png");

                _List.Add(new Card
                {
                    Date = "12.12.2012",
                    Description = "Description",
                    Image = image,
                    Author = $"Автор: {LabelFirstName.Text} {LabelLastName.Text}",
                    Likes = "1000",
                    Dislikes = "1000",
                });
            }

            ListViewProfileNews.ItemsSource = _List;
            ListViewProfileNews.ItemSelected += ListViewProfileNews_ItemSelected;
        }

        private async void ListViewProfileNews_ItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            Card item = (Card)e.SelectedItem;
            await Navigation.PushAsync(new FullArticle());
        }

        private async void ButtonSubscribe_Clicked(object sender, EventArgs e)
        {
            if (!_isSubsribe)
            {
                JObject result = await Api.FollowingUser(_authorID, Cache.ID);
                string status = result["status"].ToString();
                if (status == "success")
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
                JObject result = await Api.RemoveFollowingUser(_authorID, Cache.ID);
                string status = result["status"].ToString();
                if (status == "success")
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