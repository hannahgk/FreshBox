using Amazon.CognitoIdentityProvider;
using Amazon.Runtime;
using FreshBox.Models;
using Newtonsoft.Json;
using System;
using System.IO;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace FreshBox
{
    public partial class App : Application
    {

        private static FreshBoxDatabase freshBoxDatabase;
        public static FreshBoxDatabase FreshBoxDatabase
        {
            get
            {
                if (freshBoxDatabase == null)
                {
                    freshBoxDatabase = new FreshBoxDatabase(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "FreshBoxes.db"));
                }
                return freshBoxDatabase;
            }
        }

        private static ItemDatabase itemDatabase;
        public static ItemDatabase ItemDatabase
        {
            get
            {
                if (itemDatabase == null)
                {
                    itemDatabase = new ItemDatabase(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "FreshBoxItems.db"));
                }
                return itemDatabase;
            }
        }

        private static CalendarDatabase calendarDatabase;
        public static CalendarDatabase CalendarDatabase
        {
            get
            {
                if (calendarDatabase == null)
                {
                    calendarDatabase = new CalendarDatabase(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "Calendar.db"));
                }
                return calendarDatabase;
            }
        } 

        public static AWSUser user;
        public static AmazonCognitoIdentityProviderClient provider;

        public App()
        {
            InitializeComponent();
            provider = new AmazonCognitoIdentityProviderClient(new AnonymousAWSCredentials(), AWS.RegionEndpoint);
            MainPage = new AppShell();
            //MainPage = new NavigationPage(new LoginViewPage());
        }

        public static Day currentDay;

        protected async override void OnStart()
        {
            string secureUser = await SecureStorage.GetAsync("User");
            if (secureUser != null)
            {
                user = JsonConvert.DeserializeObject<AWSUser>(secureUser);
            }
            else
            {
                user = null;
            }

            currentDay = CalendarDatabase.GetCurrentDayAsync();
            if (currentDay.Date != DateTime.Today)
            {
                currentDay.isCurrentDay = false;
                _ = await calendarDatabase.UpdateDayAsync(currentDay);
                currentDay = new Day();
                _ = await calendarDatabase.SaveDayAsync(currentDay);
            }
        }

        protected override void OnSleep()
        {
        }

        protected override void OnResume()
        {
        }
    }
}