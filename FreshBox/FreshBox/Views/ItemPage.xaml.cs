using FreshBox.Models;
using FreshBox.Services;
using System;
using Xamarin.CommunityToolkit.Extensions;
using Xamarin.Forms;
using System.Collections.Generic;
using FreshBox.ViewModels;

namespace FreshBox.Views
{
    public partial class ItemPage : ContentPage
    {
        private FreshBoxes FreshBox { get; }
        private INotificationManager notificationManager;
        private readonly List<string> DropdownItems = new List<string> { "Name", "Expiration Date", "Quantity" };
        private Item startItem { get; set; }

        public ItemPage(FreshBoxes freshBox)
        {
            InitializeComponent();
            dropDown.ItemsSource = DropdownItems;
            dropDown.SelectedIndex = 0;
            dropDown.ItemSelected += OnDropdownSelected;

            BackButton.Clicked += BackButton_Clicked;
            FreshBox = freshBox;

            notificationManager = DependencyService.Get<INotificationManager>();
            /*
             * When the add item page button is clicked, we want a notification to be triggered.
             * When we select the notification, we want it to take us to ItemDetailsPage.
             * There is no such page yet so it will bring us to home page instead.
             */

            //when notification received, select and bring us to item details page
            notificationManager.NotificationReceived += (sender, eventArgs) =>//notification received - want notification sent
            {
                GoToHomePage(sender, eventArgs);
            };
        }

        public ItemPage(FreshBoxes freshBox, Item item)
        {
            InitializeComponent();
            dropDown.ItemsSource = DropdownItems;
            dropDown.SelectedIndex = 0;
            dropDown.ItemSelected += OnDropdownSelected;

            BackButton.Clicked += BackButton_Clicked;
            FreshBox = freshBox;

            startItem = item;

            notificationManager = DependencyService.Get<INotificationManager>();
            /*
             * When the add item page button is clicked, we want a notification to be triggered.
             * When we select the notification, we want it to take us to ItemDetailsPage.
             * There is no such page yet so it will bring us to home page instead.
             */

            //when notification received, select and bring us to item details page
            notificationManager.NotificationReceived += (sender, eventArgs) =>//notification received - want notification sent
            {
                GoToHomePage(sender, eventArgs);
            };
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();
            collectionView.ItemsSource = await App.ItemDatabase.GetItemsByBox(FreshBox.Id);
            //if (startItem is Item item)
            //{
            //    Item x = await collectionView.GetItemAsync(item.Id);
            //    collectionView.ScrollTo(x);
            //}


        }

        private async void OnDropdownSelected(object sender, ItemSelectedEventArgs e)
        {
            List<Item> itemsource = await App.ItemDatabase.GetItemsByBox(FreshBox.Id);
            switch (e.SelectedIndex)
            {
                case 0:
                    //sort by Name
                    itemsource.Sort((Item a, Item b) => a.Name.CompareTo(b.Name));
                    collectionView.ItemsSource = itemsource;
                    break;
                case 1:
                    //sort by Expiration Date
                    itemsource.Sort((Item a, Item b) => a.ExpiryDate.CompareTo(b.ExpiryDate));
                    collectionView.ItemsSource = itemsource;
                    break;
                case 2:
                    //sort by Quantity
                    itemsource.Sort((Item a, Item b) => a.Quantity.CompareTo(b.Quantity));
                    collectionView.ItemsSource = itemsource;
                    break;
                default:
                    //didn't work
                    Console.WriteLine("This didn't work");
                    break;
            }
        }


        //goes to Home Page
        private async void GoToHomePage(Object sender, EventArgs e)
        {
            await Navigation.PushModalAsync(new AboutPage());
        }

        private async void AddItemButtonClicked(object sender, EventArgs e)
        {
            //returns a task
            _ = await Navigation.ShowPopupAsync(new AddItemPopup(FreshBox));
            collectionView.ItemsSource = await App.ItemDatabase.GetItemsByBox(FreshBox.Id);
        }

        private async void OnDeleteSwipeItem(object sender, EventArgs e)
        {
            SwipeItem swipeItem = sender as SwipeItem;
            if (!(swipeItem.BindingContext is Item item))
                return;
            _ = await App.ItemDatabase.DeleteItemAsync(item);
            collectionView.ItemsSource = await App.ItemDatabase.GetItemsByBox(FreshBox.Id);
            return;
        }

        private async void EatenButton_Clicked(object sender, System.EventArgs e)
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
            collectionView.ItemsSource = await App.ItemDatabase.GetItemsByBox(FreshBox.Id);
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
            collectionView.ItemsSource = await App.ItemDatabase.GetItemsByBox(FreshBox.Id);
        }

        private async void CollectionView_SelectionChangedAsync(object sender, SelectionChangedEventArgs e)
        {
            //does nothing if current selection is not a FreshBox, otherwise navigates to relevant Items page
            if (!(e.CurrentSelection[0] is Item item))
                return;
            await Navigation.ShowPopupAsync(new ItemDetailPopup(item));
        }

        private async void OnEditSwipeItem(object sender, EventArgs e)
        {
            SwipeItem swipeItem = sender as SwipeItem;
            if (!(swipeItem.BindingContext is Item item))
                return;
            _ = await Navigation.ShowPopupAsync(new EditItemPopup(item));
            collectionView.ItemsSource = await App.ItemDatabase.GetItemsByBox(FreshBox.Id);
            return;
        }

        private async void BackButton_Clicked(object sender, EventArgs e)
        {
            _ = await Navigation.PopModalAsync();
        }
    }
}
