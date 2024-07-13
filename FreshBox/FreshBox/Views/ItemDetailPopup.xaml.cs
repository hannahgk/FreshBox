using FreshBox.Models;
using Xamarin.CommunityToolkit.UI.Views;
using Xamarin.Forms.Xaml;

namespace FreshBox.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]

    public partial class ItemDetailPopup : Popup
    {
        private Item Item { get; }

        public ItemDetailPopup(Item item)
        {
            InitializeComponent();
            Item = item;
            /*
            NameLabel.Text = "Item Name: " + Item.Name;
            QuantityLabel.Text = "Quantity: " + Item.Quantity;
            ExpirationDateLabel.Text = "Expiration Date: " + Item.ExpiryDate;
            FreshBoxLabel.Text = "Item '" + Item.Name +"' belongs to FreshBox: " + (App.FreshBoxDatabase.GetFreshBoxByID(Item.FridgeId)).Name; 
            */
            NameLabel.Text = Item.Name;
            QuantityLabel.Text = "" + Item.Quantity;
            ExpirationDateLabel.Text = "" + Item.ExpiryDate;
            FreshBoxLabel.Text = "Freshbox " + (App.FreshBoxDatabase.GetFreshBoxByID(Item.FridgeId)).Name;

        }

        //editing thiss
        /*private async void EatenButton_Clicked(object sender, EventArgs e)
        {
            int NumEaten = App.currentDay.EatenCounter;
            NumEaten += 1;
            App.currentDay.EatenCounter = NumEaten;
            _ = await App.CalendarDatabase.UpdateDayAsync(App.currentDay);

            Button button = sender as Button;
            if (!(button.BindingContext is Item item)) { return; }
            if (item.Quantity == 1)
            {
                _ = await App.ItemDatabase.DeleteItemAsync(item);
            }
            else
            {
                _ = await App.ItemDatabase.UpdateItemAsync(new Item
                {
                    Id = item.Id,
                    Name = item.Name,
                    Image = item.Image,
                    Quantity = item.Quantity - 1,
                    ExpiryDate = item.ExpiryDate,
                    FridgeId = item.FridgeId,
                }); ;
            }
            var result = await App.ItemDatabase.GetItemsByBox(Item.Id);
            Dismiss(result);
        }

        private async void DiscardButton_Clicked(object sender, System.EventArgs e)
        {
            int NumDiscarded = App.currentDay.DiscardedCounter;
            NumDiscarded += 1;
            App.currentDay.DiscardedCounter = NumDiscarded;
            _ = await App.CalendarDatabase.UpdateDayAsync(App.currentDay);

            Button button = sender as Button;
            if (!(button.BindingContext is Item item)) { return; }
            if (item.Quantity == 1)
            {
                _ = await App.ItemDatabase.DeleteItemAsync(item);
            }
            else
            {
                _ = await App.ItemDatabase.UpdateItemAsync(new Item
                {
                    Id = item.Id,
                    Name = item.Name,
                    Image = item.Image,
                    Quantity = item.Quantity - 1,
                    ExpiryDate = item.ExpiryDate,
                    FridgeId = item.FridgeId,
                }); ;
            }

            var result = await App.ItemDatabase.GetItemsByBox(Item.Id);
            Dismiss(result);
        }*/

    }
}