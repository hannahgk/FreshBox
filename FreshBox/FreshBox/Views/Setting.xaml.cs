using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace FreshBox.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]

    public partial class Setting : ContentPage
    {
        public Setting()
        {
            InitializeComponent();
        }

        private async void AccountInfoNav(object sender, EventArgs args)
        {
            await Navigation.PushAsync(new AccountInfo());
        }

        private async void NotificationSettingsPageNav(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new NotificationSettingsPage());
        }

        private async void AppDetailsNav(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new AppDetails());
        }

        private async void LoginPageNav(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new LoginPage());
        }

        private async void HelpPageNav(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new HelpPage());
        }
        private async void LogoutPageNav(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new AboutPage()); //Need to fix to log out page
        }

        private void ButtonUser_Clicked(object sender, EventArgs e)
        {
            Xamarin.Essentials.Preferences.Clear();
            Xamarin.Essentials.Preferences.Set("userID", 1);
            Xamarin.Essentials.Preferences.Set("profileName", "Sheng Dong");
            Xamarin.Essentials.Preferences.Set("streetAndCity", "1400 Washington Ave, Albany");
            Xamarin.Essentials.Preferences.Set("stateAndZip", "Albany, 12222");
            Xamarin.Essentials.Preferences.Set("imageUrl", "food_banana.png");
            string data = Xamarin.Essentials.Preferences.Get("profileName", "NA");
            _ = Application.Current.MainPage.DisplayAlert("Success, user created!", data, "OK");
        }
    }
}