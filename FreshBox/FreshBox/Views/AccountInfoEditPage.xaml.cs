using System;
using System.ComponentModel;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;


namespace FreshBox.Views
{
    [DesignTimeVisible(false)]
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class AccountInfoEditPage : ContentPage
    {
        public AccountInfoEditPage()
        {
            InitializeComponent();
        }

        private async void DoneButton_Clicked(object sender, EventArgs e)
        {
            Xamarin.Essentials.Preferences.Set("profileName", profileName.Text);
            Xamarin.Essentials.Preferences.Set("streetAndCity", Address1.Text);
            Xamarin.Essentials.Preferences.Set("stateAndZip", Address2.Text);
            await Navigation.PopAsync();
        }

        void Button_Clicked(object sender, EventArgs e)
        {
            var mainDisplayInfo = DeviceDisplay.MainDisplayInfo;


            if (MyDraggableView.Height == 0)
            {
                Action<double> callback = input => MyDraggableView.HeightRequest = input;
                double startHeight = 0;
                double endHeight = mainDisplayInfo.Height / 3;
                uint rate = 32;
                uint length = 150;
                Easing easing = Easing.CubicOut;
                MyDraggableView.Animate("anim", callback, startHeight, endHeight, rate, length, easing);
            }
            else
            {
                Action<double> callback = input => MyDraggableView.HeightRequest = input;
                double startHeight = mainDisplayInfo.Height / 3;
                double endiendHeight = 0;
                uint rate = 32;
                uint length = 150;
                Easing easing = Easing.SinOut;
                MyDraggableView.Animate("anim", callback, startHeight, endiendHeight, rate, length, easing);

            }
        }
        void TapGestureRecognizer_Tapped(object sender, EventArgs e)
        {
            var mainDisplayInfo = DeviceDisplay.MainDisplayInfo;
            Action<double> callback = input => MyDraggableView.HeightRequest = input;
            double startHeight = mainDisplayInfo.Height / 3;
            double endiendHeight = 0;
            uint rate = 32;
            uint length = 150;
            Easing easing = Easing.SinOut;
            MyDraggableView.Animate("anim", callback, startHeight, endiendHeight, rate, length, easing);
        }
        //END
    }

}