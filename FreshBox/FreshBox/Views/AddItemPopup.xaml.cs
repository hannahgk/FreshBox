using FreshBox.Models;
using FreshBox.Services;
using System;
using Xamarin.CommunityToolkit.UI.Views;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace FreshBox.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class AddItemPopup : Popup
    {
        private readonly FreshBoxes freshBox;
        private readonly INotificationManager notificationManager;

        public AddItemPopup(FreshBoxes freshBox)
        {
            InitializeComponent();
            this.freshBox = freshBox;

            notificationManager = DependencyService.Get<INotificationManager>();
        }

        /*
         * This method will add an item. The user will enter a name and date for the item. 
         * Adding the item will schedule a notification to be triggered the day before the
         * expiration date.
         */
        public async void AddItemButtonClicked(object sender, EventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(nameEntry.Text))
            {
                var result = await App.ItemDatabase.SaveItemAsync(new Item
                {
                    Id = Guid.NewGuid().ToString(),
                    Name = nameEntry.Text,
                    Barcode = "",
                    Quantity = quantityStepper.Value,
                    ExpiryDate = expiryDatePicker.Date,
                    FridgeId = freshBox.Id
                });

                string title = $"Your {nameEntry.Text} will expire on {expiryDatePicker.Date.ToString("d")}!";
                string message = $"Please eat or throw away your {nameEntry.Text}.";

                notificationManager.SendNotification(title, message, expiryDatePicker.Date
                                   .AddDays(-Xamarin.Essentials.Preferences.Get("NumDaysNotifiedBefore", 1.0))
                                   .AddMinutes(Xamarin.Essentials.Preferences.Get("TimeNotified", 720)));

                nameEntry.Text = string.Empty;
                Dismiss(result);
            }
        }
    }

    public class NotificationEventArgs : EventArgs
    {
        public string Title { get; set; }
        public string Message { get; set; }
    }

}