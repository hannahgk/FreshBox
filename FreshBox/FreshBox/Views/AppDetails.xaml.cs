using System;
using Xamarin.Forms;


namespace FreshBox.Views
{
    public partial class AppDetails : ContentPage
    {
        public AppDetails()
        {
            InitializeComponent();
        }

        //send user to Home page on click from bottom navbar
        private async void HomePage(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new AboutPage());
        }
    }
}
