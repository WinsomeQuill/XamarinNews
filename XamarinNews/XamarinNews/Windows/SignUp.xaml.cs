using Newtonsoft.Json;
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
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class SignUp : ContentPage
    {
        public SignUp()
        {
            InitializeComponent();
        }

        private async void ButtonSignUpConfirm_Clicked(object sender, EventArgs e)
        {
            string firstName = EntrySignUpFirstName.Text;
            string lastName = EntrySignUpLastName.Text;
            string login = EntrySignUpLogin.Text;
            string password = EntrySignUpPassword.Text;

            User user = new User(firstName, lastName, null, login, password);
            JObject result = await Api.RegistrationUser(user);
            if (result["status"].ToString() == "error")
            {
                LabelError.Text = result["message"].ToString();
                return;
            }

            if (result["status"].ToString() == "success")
            {
                result = await Api.LoginUser(login, password);
                user = JsonConvert.DeserializeObject<User>(result["message"].ToString());

                Cache.Login = login;
                Cache.ID = user.Id;
                Cache.About = user.About;
                Cache.FirstName = user.FirstName;
                Cache.LastName = user.LastName;
                Cache.CropAvatar = user.CropAvatar;
                Cache.FullAvatar = user.FullAvatar;
                await Navigation.PushAsync(new MainPage());
            }
        }
    }
}