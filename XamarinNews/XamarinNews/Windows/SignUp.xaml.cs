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

            RegisterUser user = new RegisterUser(firstName, lastName, null, login, password);
            bool result = await Api.RegistrationUser(user);
            if (!result)
            {
                LabelError.Text = "Failed registration!";
                return;
            }

            User newuser = await Api.LoginUser(login, password);

            Cache.Login = login;
            Cache.ID = newuser.Id;
            Cache.About = newuser.About;
            Cache.FirstName = newuser.FirstName;
            Cache.LastName = newuser.LastName;
            Cache.CropAvatar = newuser.CropAvatar;
            Cache.FullAvatar = newuser.FullAvatar;
            await Navigation.PushAsync(new MainPage());
        }
    }
}