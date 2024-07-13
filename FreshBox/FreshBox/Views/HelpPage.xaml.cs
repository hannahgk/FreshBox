using FreshBox.Models;
using FreshBox.Services;
using System;
using Xamarin.Forms;

namespace FreshBox.Views
{
    public partial class HelpPage : ContentPage
    {

        public HelpPage()
        {
            InitializeComponent();
        }

        private void OnTextChanged(object sender, EventArgs e)
        {
            SearchBar searchBar = (SearchBar)sender;
            searchResults.ItemsSource = HelpDataStore.GetSearchResults(searchBar.Text);
        }

        private void SearchResults_ItemSelected(object sender, ItemTappedEventArgs e)
        {
            var txt = (HelpItem)e.Item;
            DisplayAlert(txt.Text, txt.Description, "ok");
        }
    }
}