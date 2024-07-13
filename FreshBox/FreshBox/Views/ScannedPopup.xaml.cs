using FreshBox.Models;
using FreshBox.Services;
using System;
using System.Collections.Generic;
using Xamarin.CommunityToolkit.Extensions;
using Xamarin.CommunityToolkit.UI.Views;
using Xamarin.Forms;

namespace FreshBox.Views
{
    public partial class ScannedPopup : Popup
    {
        private readonly string barcode;
        private FreshBoxes freshBox;
        private readonly INotificationManager notificationManager;

        public ScannedPopup(string barcode)
        {
            InitializeComponent();
            this.barcode = barcode;
            MyLabel.Text = "Barcode Number: " + barcode;
            expiryDatePicker.Date = DateTime.Today.AddDays(7);
            notificationManager = DependencyService.Get<INotificationManager>();
        }
        
        public ScannedPopup(string barcode, Dictionary<string, string> responseDict)
        {
            InitializeComponent();
            this.barcode = barcode;
            MyLabel.Text = "Barcode Number: " + barcode;
            try
            {
                nameEntry.Text = responseDict["item_name"];
            }
            catch (KeyNotFoundException) { }
            expiryDatePicker.Date = DateTime.Today.AddDays(7);

            notificationManager = DependencyService.Get<INotificationManager>();
        }

        private async void WhereButton_Clicked(object sender, EventArgs e)
        {
            freshBox = (FreshBoxes)await Navigation.ShowPopupAsync(new PopupBox());
            whereLabel.TextColor = Color.Black;
            whereLabel.Text = "Box " + freshBox.Name + " selected";
        }

        private async void AddItemButtonClicked(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(nameEntry.Text))
            {
                nameEntry.PlaceholderColor = Color.Red;
            }
            else if (freshBox is null)
            {
                whereLabel.TextColor = Color.Red;
            }
            else
            {
                var result = await App.ItemDatabase.SaveItemAsync(new Item
                {
                    Id = Guid.NewGuid().ToString(),
                    Name = nameEntry.Text,
                    Barcode = barcode,
                    Quantity = 1,
                    ExpiryDate = expiryDatePicker.Date,
                    FridgeId = freshBox.Id
                });

                string title = $"Your {nameEntry.Text} will expire on {expiryDatePicker.Date.ToString("d")}!";
                string message = $"Please eat or throw away your {nameEntry.Text}.";

                notificationManager.SendNotification(title, message, expiryDatePicker.Date
                   .AddDays(-Xamarin.Essentials.Preferences.Get("NumDaysNotifiedBefore", 1.0))
                   .AddMinutes(Xamarin.Essentials.Preferences.Get("TimeNotified", 720)));

                Dismiss(result);
            }
        }

        private void ExpireButton_Clicked(object sender, EventArgs e)
        {
            if (expiryDatePicker.IsFocused)
            {
                expiryDatePicker.Unfocus();
            }
            _ = expiryDatePicker.Focus();
        }

        //send user to Home page on click from bottom navbar
        private async void HomePage(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new AboutPage());
        }
    }
}