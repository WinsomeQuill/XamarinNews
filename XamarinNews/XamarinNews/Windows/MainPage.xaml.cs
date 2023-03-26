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
using System.Threading;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;
using XamarinNews.PostgresSQL.Models;

namespace XamarinNews.Windows
{
    public partial class MainPage : TabbedPage
    {
        public MainPage()
        {
            InitializeComponent();
            Init();
        }

        private void Init()
        {
            // Это костыль, чтобы пользователь не смог вернуться назад по кнопке
            // Просто у xamarin тупая система навигации
            // Кто знает, как это делается правильно - welcome to pull request
            NavigationPage.SetHasNavigationBar(this, false);

            MainPageSearchImageAvatarUser.Source = Cache.CropAvatar;
            ImageAvatarUser.Source = Cache.FullAvatar;
            LabelProfileAbout.Text = Cache.About;

            new Thread(new ThreadStart(() =>
            {
                Device.InvokeOnMainThreadAsync(async () =>
                {
                    while(true)
                    {
                        List<Article> articles = await Api.GetArticles();
                        ListViewNews.ItemsSource = articles;
                        await Task.Delay(60000);
                    }
                });
            })).Start();

            new Thread(new ThreadStart(() =>
            {
                Device.InvokeOnMainThreadAsync(async () =>
                {
                    while (true)
                    {
                        List<Article> articles = await Api.GetArticlesFromUser(Cache.ID);
                        ListViewProfileNews.ItemsSource = articles;
                        await Task.Delay(60000);
                    }
                });
            })).Start();

            ListViewNews.ItemSelected += ListViewNews_ItemSelected;
            ListViewProfileNews.ItemSelected += ListViewNews_ItemSelected;
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
            PermissionStatus storageRead = await Permissions.CheckStatusAsync<Permissions.StorageRead>();
            PermissionStatus storageWrite = await Permissions.CheckStatusAsync<Permissions.StorageWrite>();
            PermissionStatus photos = await Permissions.CheckStatusAsync<Permissions.Photos>();

            if (storageRead == PermissionStatus.Denied || storageWrite == PermissionStatus.Denied
                || photos == PermissionStatus.Denied)
            {
                await Permissions.RequestAsync<Permissions.StorageRead>();
                await Permissions.RequestAsync<Permissions.StorageWrite>();
                await Permissions.RequestAsync<Permissions.Photos>();
            }

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
                                MainPageSearchImageAvatarUser.Source = ImageAvatarUser.Source = Cache.CropAvatar = ImageSource.FromStream(() =>
                                {
                                    return new MemoryStream(crop_avatar);
                                });
                                await Api.SetAvatar(Cache.Login, crop_avatar, full_avatar);
                            });
                        }
                    }.Show(this, result.FullPath);
                }
            }
        }

        private async void ButtonCreateArticle_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new InsertArticle());
        }
    }
}