using Android.App.Slices;
using Android.Widget;
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
using XamarinNews.MongoDB.Models;

namespace XamarinNews.Windows
{
    public partial class MainPage : TabbedPage
    {
        private List<Card> _List { get; set; } = new List<Card>();
        private List<Card> _ListProfiles { get; set; } = new List<Card>();
        public MainPage()
        {
            InitializeComponent();
            Init();
        }

        private async void Init()
        {
            NavigationPage.SetHasNavigationBar(this, false);
            JObject result = await Api.GetAvatar(Cache.Login);
            byte[] crop_avatar = Convert.FromBase64String(result["message"]["crop_avatar"].ToString());
            byte[] full_avatar = Convert.FromBase64String(result["message"]["full_avatar"].ToString());

            if (crop_avatar.Length > 0)
            {
                MainPageSearchImageAvatarUser.Source = ImageSource.FromStream(() =>
                {
                    return new MemoryStream(crop_avatar);
                });
            }

            if (full_avatar.Length > 0)
            {
                ImageAvatarUser.Source = Cache.FullAvatar = ImageSource.FromStream(() =>
                {
                    return new MemoryStream(full_avatar);
                });
            }

            for (int i = 0; i < 10; i++)
            {
                ImageSource image = ImageSource.FromResource("testcardimage.png");

                _ListProfiles.Add(new Card
                {
                    Date = "12.12.2012",
                    Description = "Description",
                    Image = image,
                    Author = "Автор: РИО Новости",
                    AuthorID = 2,
                    Likes = "1000",
                    Dislikes = "1000",
                });
            }

            for (int i = 0; i < 10; i++)
            {
                ImageSource image = ImageSource.FromResource("testcardimage.png");

                _List.Add(new Card {
                    Date = "12.12.2012",
                    Description = "Description",
                    Image = image,
                    Author = "Автор: РИО Новости",
                    AuthorID = 2,
                    Likes = "1000",
                    Dislikes = "1000",
                });
            }

            ListViewNews.ItemsSource = ListViewProfileNews.ItemsSource = _List;
            ListViewProfiles.ItemsSource = _ListProfiles;

            ListViewNews.ItemSelected += ListViewNews_ItemSelected;
            ListViewProfiles.ItemSelected += ListViewProfiles_ItemSelected;
        }

        private async void ListViewProfiles_ItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            Card item = (Card)e.SelectedItem;
            await Navigation.PushAsync(new AnotherProfile(item.AuthorID, Cache.ID));
        }

        private void SearchNews_TextChanged(object sender, TextChangedEventArgs e)
        {

        }

        private async void ListViewNews_ItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            Card item = (Card)e.SelectedItem;
            await Navigation.PushAsync(new FullArticle());
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