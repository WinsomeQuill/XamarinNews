using Newtonsoft.Json;
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
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class SignIn : ContentPage
    {
        public SignIn()
        {
            InitializeComponent();
        }

        private async void ButtonSignInConfirm_Clicked(object sender, EventArgs e)
        {
            string login = EntrySignInLogin.Text;
            string pass = EntrySignInPassword.Text;

            if (!string.IsNullOrEmpty(LabelError.Text))
                LabelError.Text = "";

            try
            {
                JObject result = await Api.LoginUser(login, pass);
                if (result["status"].ToString() == "error")
                {
                    LabelError.Text = result["message"].ToString();
                    return;
                }

                User user = JsonConvert.DeserializeObject<User>(result["message"].ToString());

                Cache.Login = login;
                Cache.ID = user.Id;
                Cache.About = user.About;
                Cache.FirstName = user.FirstName;
                Cache.LastName = user.LastName;
                Cache.CropAvatar = user.CropAvatar;
                Cache.FullAvatar = user.FullAvatar;
                await Navigation.PushAsync(new MainPage());
            }
            catch (Exception ex)
            {
                LabelError.Text = $"[{ex.HResult}] {ex.Message}";
            }
        }
    }
}