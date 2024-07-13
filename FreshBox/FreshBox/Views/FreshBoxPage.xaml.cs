using System;
using Xamarin.Forms;
using FreshBox.Models;
using Xamarin.CommunityToolkit.Extensions;

namespace FreshBox.Views
{
    public partial class FreshBoxPage : ContentPage
    {
        public FreshBoxPage()
        {
            InitializeComponent();
            BackButton.Clicked += BackButton_Clicked;
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();
            collectionView.ItemsSource = await App.FreshBoxDatabase.GetFreshBoxesAsync();
        }

        private async void AddFreshBoxButtonClicked(object sender, EventArgs e)
        {
            //returns a task
            _ = await Navigation.ShowPopupAsync(new AddFreshBoxPopUp());
            collectionView.ItemsSource = await App.FreshBoxDatabase.GetFreshBoxesAsync();
        }

        private async void OnCollectionViewSelection(object sender, SelectionChangedEventArgs e)
        {
            //does nothing if current selection is not a FreshBox, otherwise navigates to relevant Items page
            if (!(e.CurrentSelection[0] is FreshBoxes freshBox))
                return;
            await Navigation.PushModalAsync(new ItemPage(freshBox));
        }

        private async void OnDeleteSwipeItem(object sender, EventArgs e)
        {
            SwipeItem swipeItem = sender as SwipeItem;
            if (!(swipeItem.BindingContext is FreshBoxes freshBox))
                return;
            _ = await App.FreshBoxDatabase.DeleteFreshBoxAsync(freshBox);
            collectionView.ItemsSource = await App.FreshBoxDatabase.GetFreshBoxesAsync();
            return;
        }

        private async void OnEditSwipeItem(object sender, EventArgs e)
        {
            SwipeItem swipeItem = sender as SwipeItem;
            if (!(swipeItem.BindingContext is FreshBoxes freshBox))
                return;
            _ = await Navigation.ShowPopupAsync(new SelectFreshBoxPopUp(freshBox));
            collectionView.ItemsSource = await App.FreshBoxDatabase.GetFreshBoxesAsync();
            return;
        }

        private async void OnFavSwipeItem(object sender, EventArgs e)
        {
            SwipeItem swipeItem = sender as SwipeItem;
            if (!(swipeItem.BindingContext is FreshBoxes freshBox))
                return;
            freshBox.IsFav = !freshBox.IsFav;
            _ = await App.FreshBoxDatabase.UpdateFreshBoxAsync(freshBox);
            collectionView.ItemsSource = await App.FreshBoxDatabase.GetFreshBoxesAsync();
            return;
        }
        private async void BackButton_Clicked(object sender, EventArgs e)
        {

            await Navigation.PopModalAsync();

        }
    }
}
