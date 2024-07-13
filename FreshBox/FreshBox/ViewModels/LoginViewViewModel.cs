using System;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace FreshBox.ViewModels
{
    public class LoginViewViewModel : bBase
    {
        private string _Username;
        public string Username
        {
            set
            {
                this._Username = value;
                onPropertyChanged();
            }
            get
            {
                return this._Username;
            }
        }

        private string _Password;
        public string Password
        {
            set
            {
                this._Password = value;
                onPropertyChanged();
            }
            get
            {
                return this._Password;
            }
        }

        public Command LoginCommand { get; set; }
        public Command RegisterCommand { get; set; }

        public LoginViewViewModel()
        {
            LoginCommand = new Command(async () => await LoginCommandAsync());
            RegisterCommand = new Command(async () => await RegisterCommandAsync());
        }

        private async Task RegisterCommandAsync()
        {
            try
            {
                await Application.Current.MainPage.DisplayAlert("Success", "User Registered", "OK");
            }
            catch(Exception ex)
            {
                await Application.Current.MainPage.DisplayAlert("Error", ex.Message, "OK");
            }
        }

        private async Task LoginCommandAsync()
        {
            try
            {
                Xamarin.Essentials.Preferences.Set("Username", Username);
                Application.Current.MainPage = new AppShell();
            }
            catch(Exception ex)
            {
                await Application.Current.MainPage.DisplayAlert("Error", ex.Message, "OK");
            }
        }
    }
}