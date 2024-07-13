using System;
using System.Collections.Generic;
using FreshBox.Models;
using Xamarin.CommunityToolkit.Extensions;
using Xamarin.Forms;

namespace FreshBox.Views
{
    public partial class SearchPage : ContentPage
    {
        public SearchPage(string searchQuery)
        {
            InitializeComponent();
            searchBar.Text = searchQuery;

            BindingContext = this;
            //searchResultText.Text = "hi";
            //searchResultText.Detail = "detail";

        }

        private async void OnTextChangedAsync(object sender, EventArgs e)
        {
            SearchBar searchBar = (SearchBar)sender;
            searchResults.ItemsSource = await App.ItemDatabase.GetItemSearchResults(searchBar.Text);
        }

        private async void searchResults_ItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            if (!(e.SelectedItem is Item item))
                return;
            await Navigation.ShowPopupAsync(new ItemDetailPopup(item));
        }

        async void searchBar_TextChanged(System.Object sender, Xamarin.Forms.TextChangedEventArgs e)
        {
            SearchBar searchBar = (SearchBar)sender;
            searchResults.ItemsSource = await App.ItemDatabase.GetItemSearchResults(searchBar.Text);
        }
    }
}

