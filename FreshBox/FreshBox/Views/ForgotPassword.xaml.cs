using System;
using System.Collections.Generic;

using Xamarin.Forms;

namespace FreshBox.Views
{
    public partial class ForgotPassword : ContentPage
    {
        public ForgotPassword()
        {
            InitializeComponent();
        }

        private async void Button_Clicked_forgot(object sender, EventArgs e)
        {
            //await Navigation.PushAsync(new SignUp());
            await DisplayAlert("Email", "Please Email Freshboxcustomerservice@gmail.com to have your password reset.", "Ok");
            //DisplayAlert("Please contact bush.j.2018@gmail.com to rest your password");
        }
    }
}
