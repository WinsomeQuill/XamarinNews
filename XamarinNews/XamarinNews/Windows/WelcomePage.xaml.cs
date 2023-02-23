using System;
using Xamarin.Forms;

namespace XamarinNews.Windows
{
    public partial class WelcomePage : ContentPage
    {
        public WelcomePage()
        {
            InitializeComponent();
        }

        private async void ButtonWelcomeSignUp_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new SignUp());
        }

        private async void ButtonWelcomeSignIn_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new SignIn());
        }
    }
}