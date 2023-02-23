using Android.App.Slices;
using Android.Widget;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Stormlion.ImageCropper;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;
using XamarinNews.PostgresSQL.Models;

namespace XamarinNews.Windows
{
    public partial class MainPage : TabbedPage
    {
        private List<Article> _ListProfiles { get; set; } = new List<Article>();
        public MainPage()
        {
            InitializeComponent();
            Init();
        }

        private async void Init()
        {
            // Это костыль, чтобы пользователь не смог вернуться назад по кнопке
            // Просто у xamarin тупая система навигации
            // Кто знает, как это делается правильно - welcome to pull request
            NavigationPage.SetHasNavigationBar(this, false);

            MainPageSearchImageAvatarUser.Source = Cache.CropAvatar;
            ImageAvatarUser.Source = Cache.FullAvatar;

            for (int i = 0; i < 10; i++)
            {
                ImageSource image = ImageSource.FromResource("testcardimage.png");

                _ListProfiles.Add(new Article
                {
                    PublishDate = DateTime.Now,
                    Description = "Description",
                    Image = image,
                    Author = null,
                    Id = i+1000,
                    Title = "Test",
                    Likes = 1000,
                    Dislikes = 1000
                });
            }

            JObject result = await Api.GetArticles();
            List<Article> articles = JsonConvert.DeserializeObject<List<Article>>(result["message"].ToString());

            ListViewNews.ItemsSource = ListViewProfileNews.ItemsSource = articles;
            ListViewProfileNews.ItemsSource = _ListProfiles;

            ListViewNews.ItemSelected += ListViewNews_ItemSelected;
            ListViewProfiles.ItemSelected += ListViewProfiles_ItemSelected;
        }

        private async void ListViewProfiles_ItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            Article item = (Article)e.SelectedItem;
            await Navigation.PushAsync(new AnotherProfile(item.Author.Id, Cache.ID));
        }

        private void SearchNews_TextChanged(object sender, TextChangedEventArgs e)
        {

        }

        private async void ListViewNews_ItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            Article item = (Article)e.SelectedItem;
            await Navigation.PushAsync(new FullArticle(item));
        }

        private async void ButtonChangePhoto_Clicked(object sender, EventArgs e)
        {
            FilePickerFileType customFileType = FilePickerFileType.Images;

            PickOptions options = new PickOptions
            {
                PickerTitle = "Please select a comic file",
                FileTypes = customFileType,
            };

            FileResult result = await FilePicker.PickAsync(options);
            if (result != null)
            {

                string text = $"File Name: {result.FileName}";
                if (result.FileName.EndsWith("jpg", StringComparison.OrdinalIgnoreCase) ||
                    result.FileName.EndsWith("png", StringComparison.OrdinalIgnoreCase))
                {
                    Stream stream = await result.OpenReadAsync();
                    byte[] full_avatar = Utils.ConvertStreamToByteArray(stream);

                    new ImageCropper()
                    {
                        CropShape = ImageCropper.CropShapeType.Oval,
                        SelectSourceTitle = "Select source",
                        TakePhotoTitle = "Camera",
                        PhotoLibraryTitle = "Gallery",
                        CancelButtonTitle = "Cancel",
                        Success = (imageFile) =>
                        {
                            Device.BeginInvokeOnMainThread(async () =>
                            {
                                byte[] crop_avatar = File.ReadAllBytes(imageFile);
                                MainPageSearchImageAvatarUser.Source = Cache.CropAvatar = ImageSource.FromFile(imageFile);
                                ImageAvatarUser.Source = ImageSource.FromFile(result.FullPath);
                                await Api.SetAvatar(Cache.Login, crop_avatar, full_avatar);
                            });
                        }
                    }.Show(this, result.FullPath);
                }
            }
        }
    }
}