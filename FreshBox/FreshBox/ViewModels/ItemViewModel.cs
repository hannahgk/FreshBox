using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Windows.Input;
using FreshBox.Models;
using Xamarin.Forms;

namespace FreshBox.ViewModels
{
    public class ItemViewModel : INotifyPropertyChanged
    {
        //public ObservableCollection<Item> ItemsExpSoon { get; private set; }
        public ICommand AddCommand { protected set; get; }

        //row heights of collection views on ExpirationListTabbedPage
        private int _expiringTodayRowHeight;
        private int _expiringTomorrowRowHeight;
        private int _expiringLaterRowHeight;
        private int _expiredYesterdayRowHeight;
        private int _expiredEarlierRowHeight;

        public event PropertyChangedEventHandler PropertyChanged;

        public int expiringTodayRowHeight
        {
            get { return _expiringTodayRowHeight; }
            set
            {
                //needs to be # of items in the collection * frame Height
                _expiringTodayRowHeight = value;
                RaisePropertyChanged("expiringTodayRowHeight");
            }
        }

        public int expiringTomorrowRowHeight
        {
            get { return _expiringTomorrowRowHeight; }
            set
            {
                //needs to be # of items in the collection * frame Height
                _expiringTomorrowRowHeight = value;
                RaisePropertyChanged("expiringTomorrowRowHeight");
            }
        }

        public int expiringLaterRowHeight
        {
            get { return _expiringLaterRowHeight; }
            set
            {
                //needs to be # of items in the collection * frame Height
                _expiringLaterRowHeight = value;
                RaisePropertyChanged("expiringLaterRowHeight");
            }
        }
        public int expiredYesterdayRowHeight
        {
            get { return _expiredYesterdayRowHeight; }
            set
            {
                //needs to be # of items in the collection * frame Height
                _expiredYesterdayRowHeight = value;
                RaisePropertyChanged("expiredYesterdayRowHeight");
            }
        }
        public int expiredEarlierRowHeight
        {
            get { return _expiredEarlierRowHeight; }
            set
            {
                //needs to be # of items in the collection * frame Height
                _expiredEarlierRowHeight = value;
                RaisePropertyChanged("expiredEarlierRowHeight");

            }
        }

        //populates collectionViews
        public ItemViewModel()
        {
            List<Item> items = App.ItemDatabase.GetItemsSortedByDateSync();

            //expiring
            if (App.ItemDatabase.GetItemsByDateSync(DateTime.Today).Count < 1)
                expiringTodayRowHeight = 30;
            else
                expiringTodayRowHeight = ExpirationListViewBinding(App.ItemDatabase.GetItemsByDateSync(DateTime.Today));

            if (App.ItemDatabase.GetItemsByDateSync(DateTime.Today.AddDays(1)).Count < 1)
                expiringTomorrowRowHeight = 30;
            else
                expiringTomorrowRowHeight = ExpirationListViewBinding(App.ItemDatabase.GetItemsByDateSync(DateTime.Today.AddDays(1)));

            if ((from item in items
                 where item.ExpiryDate.CompareTo(DateTime.Today.AddDays(1)) > 0
                 select item).ToList().Count < 1)
                expiringLaterRowHeight = 30;
            else
                expiringLaterRowHeight = ExpirationListViewBinding((from item in items
                                                                    where item.ExpiryDate.CompareTo(DateTime.Today.AddDays(1)) > 0
                                                                    select item).ToList());


            //expired
            if (App.ItemDatabase.GetItemsByDateSync(DateTime.Today.AddDays(-1)).Count < 1)
                expiredYesterdayRowHeight = 30;
            else
                expiredYesterdayRowHeight = ExpirationListViewBinding(App.ItemDatabase.GetItemsByDateSync(DateTime.Today.AddDays(-1)));

            if ((from item in items
                 where item.ExpiryDate.CompareTo(DateTime.Today.AddDays(-1)) < 0
                 select item).ToList().Count < 1)
                expiredEarlierRowHeight = 30;
            else
                expiredEarlierRowHeight = ExpirationListViewBinding((from item in items
                                                                     where item.ExpiryDate.CompareTo(DateTime.Today.AddDays(-1)) < 0
                                                                     select item).ToList());

        }

        private int ExpirationListViewBinding(List<Item> Items)
        {
            var ExpiringItems = new ObservableCollection<Item>();

            foreach (var Item in Items)
            {
                ExpiringItems.Add(Item); //This is important
            }

            var rowHeight = ExpiringItems.Count * 102;

            AddCommand = new Command<Item>(async (key) =>
            {
                //low key should change this... but so far past caring
                ExpiringItems.Add(new Item() { });
                rowHeight = ExpiringItems.Count * 102;
            });

            return rowHeight;
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
