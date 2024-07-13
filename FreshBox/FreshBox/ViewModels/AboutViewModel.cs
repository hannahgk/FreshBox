using System.Collections.ObjectModel;
using Xamarin.Forms;
using System;
using System.Collections.Generic;
using FreshBox.Models;
using System.Windows.Input;
using System.Threading.Tasks;
using Xamarin.CommunityToolkit.UI.Views;
using System.ComponentModel;
using FreshBox.Views;

namespace FreshBox.ViewModels
{

    public class AboutViewModel : BaseViewModel
    {
        private LayoutState _mainState;
        public Image _image = new Image { Source = "profile.png" };
        String _today = DateTime.Today.ToString("D");
        //rowheight of collectionView in search bar on AboutPage
        private int _searchBarRowHeight;

        public event PropertyChangedEventHandler PropertyChanged;

        public ICommand AddCommand { protected set; get; }

        public Image Image { get => _image; }

        public String Today { get => _today; }

        public bool IsExpanded { get; set; }

        public string SearchText { get; set; }

        public ICommand RefreshCommand => new Command(async () => await OnRefreshAsync());
        public ICommand SkeletonCommand { get; set; }

        public LayoutState MainState
        {
            get => _mainState;
            set => SetProperty(ref _mainState, value);
        }

        public int searchBarRowHeight
        {
            get { return _searchBarRowHeight; }
            set
            {
                //needs to be # of items in the collection * frame Height
                _searchBarRowHeight = value;
                RaisePropertyChanged("searchBarRowHeight");
            }
        }

        public AboutViewModel()
        {
            searchBarRowHeight = SearchBarViewBinding(App.ItemDatabase.GetItemSearchResultsSync(AboutPage.searchBarText));
        }

        private int SearchBarViewBinding(List<Item> Items)
        {
            var SearchBarItems = new ObservableCollection<Item>();

            foreach (var Item in Items)
            {
                SearchBarItems.Add(Item); //This is important
            }

            var rowHeight = SearchBarItems.Count * 32;

            AddCommand = new Command<Item>(async (key) =>
            {
                //low key should change this... but so far past caring
                SearchBarItems.Add(new Item() { });
                rowHeight = SearchBarItems.Count * 32;
            });

            return rowHeight;
        }

        private async Task OnRefreshAsync()
        {
            MainState = LayoutState.Loading;
            IsBusy = true;
            await Task.Delay(3000);
            IsBusy = false;
            MainState = LayoutState.None;
        }

        public void RaisePropertyChanged(string propertyName)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null)
            {
                handler(this, new PropertyChangedEventArgs(propertyName));
            }
        }

    }
}
