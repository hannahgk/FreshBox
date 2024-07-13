using System;
using Xamarin.Essentials;

namespace FreshBox.ViewModels
{
    class AccountInfoViewModel : bBase
    {
        private int _UserID;
        public int UserID
        {
            set
            {
                _UserID = value;
                onPropertyChanged();
            }

            get
            {
                return _UserID;
            }
        }

        private string _profileName;
        public string profileName
        {
            set
            {
                _profileName = value;
                onPropertyChanged();
            }

            get
            {
                return _profileName;
            }
        }

        private string _streetAndCity;
        public string streetAndCity
        {
            set
            {
                _streetAndCity = value;
                onPropertyChanged();
            }

            get
            {
                return _streetAndCity;
            }
        }

        private string _stateAndZip;
        public string stateAndZip
        {
            set
            {
                _stateAndZip = value;
                onPropertyChanged();
            }

            get
            {
                return _stateAndZip;
            }
        }

        private string _imageUrl;
        public string imageUrl
        {
            set
            {
                _imageUrl = value;
                onPropertyChanged();
            }

            get
            {
                return _imageUrl;
            }
        }

        public AccountInfoViewModel()
        {
            var id = Preferences.Get("userID", 0);
            if (id == 0)
            {
                UserID = 0;
            }
            else
            {
                UserID = id;
            }

            var uname = Preferences.Get("profileName", String.Empty);
            if (String.IsNullOrEmpty(uname))
            {
                profileName = "Guest";
            }
            else
            {
                profileName = uname;
            }

            var Street = Preferences.Get("streetAndCity", String.Empty);
            if (String.IsNullOrEmpty(Street))
            {
                streetAndCity = "Guest";
            }
            else
            {
                streetAndCity = Street;
            }

            var state = Preferences.Get("stateAndZip", String.Empty);
            if (String.IsNullOrEmpty(state))
            {
                stateAndZip = "Guest";
            }
            else
            {
                stateAndZip = state;
            }

            var image = Preferences.Get("imageUrl", String.Empty);
            if (String.IsNullOrEmpty(image))
            {
                imageUrl = "Guest";
            }
            else
            {
                imageUrl = image;
            }
        }
    }

}