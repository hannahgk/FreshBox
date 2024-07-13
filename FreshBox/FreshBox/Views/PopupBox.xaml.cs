using FreshBox.Models;
using Xamarin.CommunityToolkit.UI.Views;
using Xamarin.Forms;

namespace FreshBox.Views
{
    public partial class PopupBox : Popup
    {
        private FreshBoxes selectedBox;
        public PopupBox()
        {
            InitializeComponent();
            OnAppearing();
        }

        private async void OnAppearing()
        {
            collectionView.ItemsSource = await App.FreshBoxDatabase.GetFreshBoxesAsync();
        }

        private void OnCollectionViewSelection(object sender, SelectionChangedEventArgs e)
        {
            //does nothing if current selection is not a FreshBox, otherwise navigates to relevant Items page
            if (!(e.CurrentSelection[0] is FreshBoxes freshBox))
                return;
            selectedBox = freshBox;
            Dismiss(selectedBox);
        }
    }
}
