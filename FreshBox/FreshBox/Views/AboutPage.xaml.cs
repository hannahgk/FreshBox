using FreshBox.Models;
using Microcharts;
using SkiaSharp;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Xamarin.CommunityToolkit.Extensions;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using ZXing.Net.Mobile.Forms;

namespace FreshBox.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class AboutPage : ContentPage
    {
        ZXingScannerPage scanPage;

        static List<Day> days;
        static float maxEaten;
        internal static string searchBarText;
        static int topSearchResults = 3;

        public AboutPage()
        {
            InitializeComponent();
            ButtonScan.Clicked += ButtonScan_Clicked;
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();
            ExpiringSoon.ItemsSource = await App.ItemDatabase.GetTopItemsSortedByDate("6");
            FavFreshBoxes.ItemsSource = await App.FreshBoxDatabase.GetFavFreshBoxes();

            searchFrame.HeightRequest = 47;
            searchResults.IsVisible = false;
            moreSearchResults.IsVisible = false;
            endOfSearchResults.IsVisible = false;

            days = await App.CalendarDatabase.GetCalendarAsync(App.currentDay.Month, App.currentDay.Year);

            //labels initialized
            monthYearLabel.Text = "Your " + CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(App.currentDay.Month) + " Statistics";
            dayEatenLabel.Text = "Days in " + CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(App.currentDay.Month);

            //charts initialized
            EatenLineChart.Chart = new LineChart
            {
                Entries = PopulateEatenChart(),
                BackgroundColor = SKColors.Transparent,
                LabelColor = SKColor.Parse("#D2B48C"),
                LabelTextSize = 40,
                ValueLabelOrientation = Orientation.Horizontal,
                PointMode = PointMode.None,
                LineSize = 10,
                MinValue = 0,
                MaxValue = maxEaten + 2
            };
        }

        public ChartEntry[] PopulateEatenChart()
        {
            ChartEntry[] EatenEntries = new ChartEntry[App.currentDay.DaysInMonth()];

            //initializes chart to 0 for every day of month
            for (int i = 0; i < EatenEntries.Length; i++)
            {
                EatenEntries[i] = new ChartEntry(0)
                {
                    Label = i + 1 + "",
                    ValueLabel = " ",
                    ValueLabelColor = SKColor.Parse("#B4937B"),
                    Color = SKColor.Parse("#D2B48C")
                };

                //makes numbers 1 every 3 days
                if (i % 3 != 0 && i != EatenEntries.Length - 1)
                    EatenEntries[i].Label = "";
            }

            //populates chart
            foreach (Day day in days)
            {
                int j = day.DayOfMonth() - 1;
                EatenEntries[j] = new ChartEntry(day.EatenCounter)
                {
                    Label = j + 1 + "",
                    ValueLabel = " ",
                    ValueLabelColor = SKColor.Parse("#B4937B"),
                    Color = SKColor.Parse("#D2B48C")
                };

                //makes day label
                if (j % 3 != 0 && j != EatenEntries.Length - 1)
                    EatenEntries[j].Label = "";

                if (day.EatenCounter != 0)
                    EatenEntries[j].ValueLabel = day.EatenCounter + "";

                maxEaten = MaxEaten(day.EatenCounter);

            }
            return EatenEntries;
        }

        private float MaxEaten(int i)
        {
            if (i >= maxEaten)
                maxEaten = i;
            return maxEaten;
        }


        private async void OnTextChanged(object sender, EventArgs e)
        {
            SearchBar searchBar = (SearchBar)sender;

            searchBarText = searchBar.Text;
            Console.WriteLine("text changed");
            topSearchResults = 3;

            if (string.IsNullOrWhiteSpace(searchBarText))
            {
                Console.WriteLine("empty");
                searchFrame.HeightRequest = 47;
                searchResults.IsVisible = false;
                moreSearchResults.IsVisible = false;
                endOfSearchResults.IsVisible = true;
            }
            else
            {
                searchResults.IsVisible = true;
                
                Console.WriteLine("not empty");
                List<Item> searchResultsList = await App.ItemDatabase.GetTopItemSearchResults(searchBarText, topSearchResults);
                searchResults.ItemsSource = searchResultsList;

                if (searchResultsList.Count < 3)
                {
                    moreSearchResults.IsVisible = false;
                    endOfSearchResults.IsVisible = true;
                    searchFrame.HeightRequest = searchBar.Height + endOfSearchResults.Height + (searchResultsList.Count * 46.25);
                    Console.WriteLine(searchResultsList.Count + " x " + searchResults.Height);
                }
                else
                {
                    moreSearchResults.IsVisible = true;
                    endOfSearchResults.IsVisible = false;
                    searchFrame.HeightRequest = searchBar.Height + moreSearchResults.Height + (searchResultsList.Count * 46.25);
                }
            }

           }


        private async void ButtonScan_Clicked(object sender, EventArgs e)
        {
            bool answer = await DisplayAlert("Adding Item", "Would you like to add item manually?", "Yes", "No");
            //Debug.WriteLine("Answer: " + answer);

            if (answer)
            {
                await Navigation.ShowPopupAsync(new ScannedPopup("no Barcode"));
            }

            else
            {
                scanPage = new ZXingScannerPage();
                scanPage.OnScanResult += (result) =>
                {
                    scanPage.IsScanning = false;
                    string resultBarcode = result.ToString();
                    Device.BeginInvokeOnMainThread(async () =>
                    {
                        _ = await Navigation.PopModalAsync();
                    //var result1 = await Navigation.ShowPopupAsync(new ScannedPopup(result.Text));

                    var client = new HttpClient();
                        var request = new HttpRequestMessage
                        {
                            Method = HttpMethod.Get,
                            RequestUri = new Uri("https://nutritionix-api.p.rapidapi.com/v1_1/item?upc=" + resultBarcode),
                            Headers =
                            {
                            { "x-rapidapi-host", "nutritionix-api.p.rapidapi.com" },
                            { "x-rapidapi-key", "1edf0846e4msh13bce5d624b2739p1c808bjsn0df58282212d" },
                            }
                        };
                        using (HttpResponseMessage response = await client.SendAsync(request))
                        {
                            try
                            {
                                _ = response.EnsureSuccessStatusCode();
                                string responseBody = await response.Content.ReadAsStringAsync();
                                responseBody = responseBody.Substring(1, responseBody.Length - 2); //remove curly braces from response
                            responseBody = responseBody.Replace("\"", ""); //remove quotation marks from response
                            string[] responseBodyAsArray = responseBody.Split(new[] { ',' });
                                responseBodyAsArray = responseBodyAsArray.Take(3).ToArray();
                                Dictionary<string, string> responseBodyAsDict = responseBodyAsArray.Select(item => item.Split(':')).ToDictionary(s => s[0], s => s[1]);
                                _ = await Navigation.ShowPopupAsync(new ScannedPopup(resultBarcode, responseBodyAsDict));
                            }
                            catch (HttpRequestException)
                            {
                                _ = await Navigation.ShowPopupAsync(new ScannedPopup(resultBarcode));
                            }
                        }
                    });
                };

                await Navigation.PushModalAsync(scanPage);
            }
            
        }

        //send user to Expiration page on click from bottom navbar
        private void FlyoutPageNav(object sender, EventArgs e)
        {
            Shell.Current.FlyoutIsPresented = true;
        }

        //send user to FreshBoxes page on click from bottom navbar
        private async void FreshBoxPageNav(object sender, EventArgs e)
        {
            await Navigation.PushModalAsync(new FreshBoxPage());
        }

        
        //send user to Expiration page on click from bottom navbar
        private async void ExpirationPageNav(object sender, EventArgs e)
        {
            await Navigation.PushModalAsync(new ExpirationListTabbedPage());
        }
        
        //send user to Expiration page on click from bottom navbar
        private async void StatsPageNav(object sender, EventArgs e)
        {
            await Navigation.PushModalAsync(new StatsPage());
        }

        private async void OnFreshBoxCollectionViewSelection(object sender, SelectionChangedEventArgs e)
        {
            //does nothing if current selection is not a FreshBox, otherwise navigates to relevant Items page
            if (!(e.CurrentSelection[0] is FreshBoxes freshBox))
                return;
            await Navigation.PushModalAsync(new ItemPage(freshBox));
        }
        private async void OnItemCollectionViewSelection(object sender, SelectionChangedEventArgs e)
        {
            if (!(e.CurrentSelection[0] is Item item))
                return;
            await Navigation.PushModalAsync(new ItemPage(App.FreshBoxDatabase.GetFreshBoxByID(item.FridgeId), item));
        }
        private async void SearchResults_ItemSelectedAsync(object sender, SelectedItemChangedEventArgs e)
        {
            if (!(e.SelectedItem is Item item))
                return;
            await Navigation.ShowPopupAsync(new ItemDetailPopup(item));
        }
        
         async void moreSearchResults_ClickedAsync(object sender, EventArgs e)
        {
            topSearchResults += 3;

            Console.WriteLine("show more");
            List<Item> searchResultsList = await App.ItemDatabase.GetTopItemSearchResults(searchBarText, topSearchResults);
            List<Item> nextResultsList = await App.ItemDatabase.GetTopItemSearchResults(searchBarText, topSearchResults+1);

            searchResults.ItemsSource = searchResultsList;

            if (searchResultsList.Count == nextResultsList.Count)
            {
                endOfSearchResults.IsVisible = true;
                moreSearchResults.IsVisible = false;
                searchFrame.HeightRequest = searchBar.Height + endOfSearchResults.Height + (searchResultsList.Count * 46.25);
            }
            else
            {
                moreSearchResults.IsVisible = true;
                searchFrame.HeightRequest = searchBar.Height + moreSearchResults.Height + (searchResultsList.Count * 46.25);
            }
        }
    }
}