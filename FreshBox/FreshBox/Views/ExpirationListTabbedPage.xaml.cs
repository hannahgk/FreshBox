using System;
using System.Collections.Generic;
using System.Linq;
using FreshBox.Models;
using Xamarin.Forms;
using Xamarin.CommunityToolkit.Extensions;
using Xamarin.Forms.Xaml;
using ZXing.Net.Mobile.Forms;
using FreshBox.ViewModels;
using System.Threading.Tasks;
using Xamarin.CommunityToolkit.UI.Views;

namespace FreshBox.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ExpirationListTabbedPage : TabbedPage
    {
        public ExpirationListTabbedPage()
        {
            InitializeComponent();
            //must include this, or expiring page will break
            this.BindingContext = new ItemViewModel();
        }

        //send user to Home page on click from bottom navbar
        private async void HomePage(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new AboutPage());
        }
        protected override async void OnAppearing()
        {
            base.OnAppearing();
            UpdateCollectionViewsAsync();
        }

        async void UpdateCollectionViewsAsync()
        {
            List<Item> items = await App.ItemDatabase.GetItemsSortedByDate();
            TodayUnexpiredCollection.ItemsSource = await App.ItemDatabase.GetItemsByDate(DateTime.Today);
            TomorrowUnexpiredCollection.ItemsSource = await App.ItemDatabase.GetItemsByDate(DateTime.Today.AddDays(1));
            LaterUnexpiredCollection.ItemsSource = from item in items
                                                   where item.ExpiryDate.CompareTo(DateTime.Today.AddDays(1)) > 0
                                                   select item;
            //edit this to -1,-2,-2<0
            YesterdayExpiredCollection.ItemsSource = await App.ItemDatabase.GetItemsByDate(DateTime.Today.AddDays(-1));
            EarlierExpiredCollection.ItemsSource = from item in items
                                                  where item.ExpiryDate.CompareTo(DateTime.Today.AddDays(-1)) < 0
                                                  select item;

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
                CollectionView collectionView = (CollectionView)button.Parent.Parent.Parent.Parent.Parent.Parent.Parent;
                if (collectionView.ItemsSource.OfType<Item>().Count() == 1)
                {
                    collectionView.HeightRequest = 30;
                }
                else
                    collectionView.HeightRequest = collectionView.HeightRequest - 149;
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
            UpdateCollectionViewsAsync();
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
                CollectionView collectionView = (CollectionView)button.Parent.Parent.Parent.Parent.Parent.Parent.Parent;
                if (collectionView.ItemsSource.OfType<Item>().Count() == 1)
                {
                    collectionView.HeightRequest = 30;
                } else
                    collectionView.HeightRequest = collectionView.HeightRequest - 149;
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
            UpdateCollectionViewsAsync();
        }

        static void Expander_Tapped(System.Object sender, System.EventArgs e)
        {
            Expander s = sender as Expander;
            
            //get collection expander was tapped in
            
            CollectionView collectionView = (CollectionView) s.Parent.Parent.Parent;

            //grow height of that collection depending on state of expander
            //and number of items in collection
            
            if (s.IsExpanded)
                collectionView.HeightRequest = collectionView.HeightRequest + 47;
            if (s.IsExpanded==false)
                collectionView.HeightRequest = collectionView.HeightRequest - 47;
            
        }
    }
}
