using System;
using Xamarin.Forms;

namespace FreshBox.Views
{
    public partial class addProfilebutton : ContentPage
    {
        public addProfilebutton()
        {
            InitializeComponent();
        }

        private async void AccountInfoNav(object sender, EventArgs args)
        {
            await Navigation.PushAsync(new AccountInfo());
        }
    }
}
