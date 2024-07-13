using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace FreshBox.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class NotificationSettingsPage : ContentPage
    {

        public NotificationSettingsPage()
        {
            InitializeComponent();
            dayStepper.Value = Xamarin.Essentials.Preferences.Get("NumDaysNotifiedBefore", 1.0);
            notifyTimePicker.Time = new TimeSpan(0, Xamarin.Essentials.Preferences.Get("TimeNotified", 720), 0);
        }

        private void DayStepper_ValueChanged(object sender, ValueChangedEventArgs e)
        {
            Xamarin.Essentials.Preferences.Set("NumDaysNotifiedBefore", dayStepper.Value);
        }

        private void NotifyTimePicker_Unfocused(object sender, FocusEventArgs e)
        {
            int chosenTime = (int)Math.Round(notifyTimePicker.Time.TotalMinutes);
            Xamarin.Essentials.Preferences.Set("TimeNotified", chosenTime);
        }
    }
}
