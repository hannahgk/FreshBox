using System;
using Xamarin.Forms;

namespace FreshBox
{
    public partial class AppShell : Shell
    {
        public AppShell()
        {
            InitializeComponent();
        }
        private async void OnMenuItemClicked(object sender, EventArgs e)
        {
            await Current.GoToAsync("//LoginPage");
        }

        //send user to Home page on click from bottom navbar
        private async void HomePage(object sender, EventArgs e)
        {
            await Current.GoToAsync("//AboutPage");
        }
        
    }
}
