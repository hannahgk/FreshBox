using System;
using System.Collections.Generic;

using Xamarin.Forms;

namespace FreshBox.Views
{
    public partial class LoggedOut : ContentPage
    {
        public LoggedOut()
        {
            InitializeComponent();
        }
        protected async override void OnAppearing()
        {
            await DisplayAlert("Log out", "You have successfully logged out", "OK");
            await Navigation.PushAsync(new AboutPage());
        }
    }
}
