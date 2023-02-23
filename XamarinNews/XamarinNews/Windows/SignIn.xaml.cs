using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

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

                Cache.Login = login;
                Cache.ID = (int)result["message"]["id"];
                Cache.Description = result["message"]["description"].ToString();
                Cache.FirstName = result["message"]["first_name"].ToString();
                Cache.LastName = result["message"]["last_name"].ToString();
                await Navigation.PushAsync(new MainPage());
            }
            catch (Exception ex)
            {
                LabelError.Text = $"[{ex.HResult}] {ex.Message}";
            }
        }
    }
}