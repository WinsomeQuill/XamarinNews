using Stormlion.ImageCropper;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace XamarinNews.Windows
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class InsertArticle : ContentPage
    {
        private byte[] Image { get; set; } = null;

        public InsertArticle()
        {
            InitializeComponent();
        }

        private async void ButtonUploadCover_Clicked(object sender, EventArgs e)
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
                    new ImageCropper()
                    {
                        CropShape = ImageCropper.CropShapeType.Rectangle,
                        SelectSourceTitle = "Select source",
                        TakePhotoTitle = "Camera",
                        PhotoLibraryTitle = "Gallery",
                        CancelButtonTitle = "Cancel",
                        Success = (imageFile) =>
                        {
                            Device.BeginInvokeOnMainThread(() =>
                            {
                                Image = File.ReadAllBytes(imageFile);
                                ImageArticleCover.Source = ImageSource.FromStream(() =>
                                {
                                    return new MemoryStream(Image);
                                });
                            });
                        }
                    }.Show(this, result.FullPath);
                }
            }
        }

        private async void ButtonPublish_Clicked(object sender, EventArgs e)
        {
            string title = EntryTitle.Text;
            string description = EditorDescription.Text;

            if (string.IsNullOrWhiteSpace(title))
            {
                await DisplayAlert("Ошибка", "Заголовок пусто!", "Ок");
                return;
            }

            if (string.IsNullOrWhiteSpace(description))
            {
                await DisplayAlert("Ошибка", "Описание пустое!", "Ок");
                return;
            }

            if (title.Length < 3)
            {
                await DisplayAlert("Ошибка", "Слишком короткий заголовок!", "Ок");
                return;
            }

            if (description.Length < 10)
            {
                await DisplayAlert("Ошибка", "Слишком короткое описание!", "Ок");
                return;
            }

            if (Image == null)
            {
                await DisplayAlert("Ошибка", "Обложка не выбрана!", "Ок");
                return;
            }

            bool result = await Api.InsertArticle(Cache.ID, Image, title, description);
            if (!result)
            {
                await DisplayAlert("Ошибка", "Произошла неизвестная ошибка! Попробуйте чуть позже!", "Ок");
                return;
            }

            await DisplayAlert("Успешно", "Вы добавили статью!", "Ок");
            await Navigation.PopAsync();
        }
    }
}