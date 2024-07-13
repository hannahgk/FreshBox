using Xamarin.Forms;
using System;

namespace FreshBox.Views
{
    public partial class Preferences : ContentPage
    {
        public Preferences()
        {
            InitializeComponent();
        }
        async void NotificationSettings_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new NotificationSettingsPage());
        }
    }
}
