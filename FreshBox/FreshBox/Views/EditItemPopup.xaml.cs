using FreshBox.Models;
using System;
using Xamarin.CommunityToolkit.UI.Views;
using Xamarin.Forms.Xaml;


namespace FreshBox.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    //If it gives erros, Try this link: https://stackoverflow.com/questions/42386406/xamarin-forms-xaml-xamlparseexception 
    public partial class EditItemPopup : Popup
    {
        private readonly Item item;
        public EditItemPopup(Item item)
        {
            InitializeComponent();
            this.item = item;
            nameEntry.Text = item.Name;
            expiryDatePicker.Date = item.ExpiryDate;
            quantityStepper.Value = item.Quantity;
        }

        public async void EditItemButtonClicked(object sender, EventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(nameEntry.Text))
            {
                var result = await App.ItemDatabase.UpdateItemAsync(new Item
                {
                    Id = item.Id,
                    Name = nameEntry.Text,
                    Quantity = quantityStepper.Value,
                    ExpiryDate = expiryDatePicker.Date,
                    FridgeId = item.FridgeId
                });

                nameEntry.Text = string.Empty;
                Dismiss(result);
            }
        }
    }
}