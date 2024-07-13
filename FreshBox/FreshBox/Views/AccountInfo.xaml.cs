using System;
using System.Linq;
using Xamarin.Forms;

namespace FreshBox.Views
{
    public partial class AccountInfo : ContentPage
    {
        public AccountInfo()
        {
            InitializeComponent();
        }

        private async void EditButton_Clicked(object sender, EventArgs e)
        {
            var previousPage = Navigation.NavigationStack.LastOrDefault();
            await Navigation.PushAsync(new AccountInfoEditPage());
            Navigation.RemovePage(previousPage);
        }
    }
}