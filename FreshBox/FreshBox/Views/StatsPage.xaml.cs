using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using System;
using FreshBox.Models;
using System.Collections.Generic;
using Microcharts;
using SkiaSharp;
using System.Globalization;
using Xamarin.Essentials;

namespace FreshBox.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class StatsPage : ContentPage
    {
        static Day currentDay;
        static List<Day> days;
        static float maxEaten;
        static float maxDiscarded;
        static int daysInMonth;
        static int month;
        static int year;

        public StatsPage()
        {
            InitializeComponent();
            BackButton.Clicked += BackButton_Clicked;

            currentDay = App.currentDay;
            month = currentDay.Month;
            year = currentDay.Year;
            UpdateCharts();
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();
        }

        public void UpdateCharts()
        {
            //days in month
            SetMonthAttributes();
            days = App.CalendarDatabase.GetCalendarAsync(month, year).Result;

            //labels initialized
            monthYearLabel.Text = CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(month) + " " + year;
            dayEatenLabel.Text = "Days in " + CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(month);
            dayDiscardedLabel.Text = "Days in " + CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(month);
            itemsEatenInMonthLabel.Text = "Items Eaten in " + CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(month);
            itemsDiscardedInMonthLabel.Text = "Items Discarded in " + CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(month);
            itemsEatenDiscardedInMonthLabel.Text = "Items Eaten and Discarded in " + CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(month);

            //charts initialized
            EatenLineChart.Chart = new LineChart
            {
                Entries = PopulateEatenChart(),
                BackgroundColor = SKColors.Transparent,
                LabelColor = SKColor.Parse("#f2ebdd"),
                LabelTextSize = 40,
                ValueLabelOrientation = Orientation.Horizontal,
                // Can't display properly 
                //LabelOrientation = Orientation.Horizontal,
                PointMode = PointMode.None,
                LineSize = 10,
                MinValue = 0,
                MaxValue = maxEaten + 2
            };

            DiscardedLineChart.Chart = new LineChart
            {
                Entries = PopulateDiscardedChart(),
                BackgroundColor = SKColors.Transparent,
                LabelColor = SKColor.Parse("#84644D"),
                LabelTextSize = 40,
                ValueLabelOrientation = Orientation.Horizontal,
                // won't display properly 
                //LabelOrientation = Orientation.Horizontal,
                PointMode = PointMode.None,
                LineSize = 10,
                MinValue = 0,
                Margin = 40,
                MaxValue = maxDiscarded + 2
            };

            EatenDiscardedDonutChart.Chart = new DonutChart
            {
                Entries = PopulateEatenDiscardedChart(),
                BackgroundColor = SKColors.Transparent,
                LabelColor = SKColor.Parse("#84644D"),
                LabelMode = LabelMode.RightOnly,
                LabelTextSize = 50
            };
        }

        //this will update the month and days in month upon pressing the back button
        public void SetMonthAttributes()
        {
            if (month == 0)
                month = 12;
            if (month == 13)
                month = 1;

            daysInMonth = DateTime.DaysInMonth(year, month);
        }

        //will record max eaten/discarded for accurate scaling in graph
        private float MaxEaten(int i)
        {
            if (i >= maxEaten)
                maxEaten = i;
            return maxEaten;
        }

        private float MaxDiscarded(int i)
        {
            if (i >= maxEaten)
                maxEaten = i;
            return maxEaten;
        }

        //have chart w month and year be sent into db call
        //go thru existing list in loop and set entries to each day
        public List<ChartEntry> PopulateEatenChart()
        {
            //can't initialize new ChartEntry array w new length
            List<ChartEntry> EatenEntries = new List<ChartEntry>(daysInMonth);

            //initializes chart to 0 for every day of month
            for (int i = 0; i < EatenEntries.Capacity; i++)
            {
                EatenEntries.Add(new ChartEntry(0)
                {
                    Label = i + 1 + "",
                    ValueLabel = " ",
                    ValueLabelColor = SKColor.Parse("#bdda60"),
                    Color = SKColor.Parse("#bdda60")
                });
                //makes numbers 1 every 3 days
                if(i % 3 != 0 && i != EatenEntries.Capacity-1)
                    EatenEntries[i].Label = "";
            }

            //populates chart
            foreach (Day day in days) {
                int j = day.DayOfMonth() - 1;
                EatenEntries[j] = new ChartEntry(day.EatenCounter)
                {
                    Label = j+1 + "",
                    ValueLabel = " ",
                    ValueLabelColor = SKColor.Parse("#bdda60"),
                    Color = SKColor.Parse("#bdda60")
                };

                //makes day labels have 3 day interval, also including first day
                //and last day of month
                if (j % 3 != 0 && j != EatenEntries.Capacity-1)
                {
                     EatenEntries[j].Label = "";
                }

                //only show value label if the user ate something that day
                if (day.EatenCounter != 0)
                    EatenEntries[j].ValueLabel = day.EatenCounter + "";

                //updates maxEaten var
                maxEaten = MaxEaten(day.EatenCounter);
                
            }
            return EatenEntries;
        }
        
        public List<ChartEntry> PopulateDiscardedChart()
        {
            List<ChartEntry> DiscardedEntries = new List<ChartEntry>(daysInMonth);

            //initializes chart to 0 for every day of month
            for (int i = 0; i < DiscardedEntries.Capacity; i++)
            {
                DiscardedEntries.Add(new ChartEntry(0)
                {
                    Label = i+1 + "",
                    ValueLabel = " ",
                    ValueLabelColor = SKColor.Parse("#B4937B"),
                    Color = SKColor.Parse("#D2B48C")
                });

                if (i % 3 != 0 && i != DiscardedEntries.Capacity - 1)
                    DiscardedEntries[i].Label = "";
            }

            //populates chart
            foreach (Day day in days)
            {
                int j = day.DayOfMonth()-1;
                DiscardedEntries[j] = new ChartEntry(day.DiscardedCounter)
                {
                    Label = j+1 + "",
                    ValueLabel = " ",
                    ValueLabelColor = SKColor.Parse("#B4937B"),
                    Color = SKColor.Parse("#D2B48C")
                };

                //makes day label
                if (j % 3 != 0 && j != DiscardedEntries.Capacity - 1)
                    DiscardedEntries[j].Label = "";

                if (day.DiscardedCounter != 0)
                    DiscardedEntries[j].ValueLabel = day.DiscardedCounter + "";

                maxDiscarded = MaxDiscarded(day.DiscardedCounter);
            }
            return DiscardedEntries;
        }

        public ChartEntry[] PopulateEatenDiscardedChart()
        {
            ChartEntry[] EatenDiscardedEntries = new ChartEntry[2];
            int EatenInMonth = 0;
            int DiscardedInMonth = 0;

            //sums up all eaten and discarded
            foreach (Day day in days)
            {
                EatenInMonth += day.EatenCounter;
                DiscardedInMonth += day.DiscardedCounter;
            }

            //creates number eaten and discarded as entries
            EatenDiscardedEntries[0] = new ChartEntry(EatenInMonth)
            {
                Label = "Eaten",
                ValueLabel = EatenInMonth + "",
                ValueLabelColor = SKColor.Parse("#bdda60"),
                Color = SKColor.Parse("#bdda60")
            };
            EatenDiscardedEntries[1] = new ChartEntry(DiscardedInMonth)
            {
                Label = "Discarded",
                ValueLabel = DiscardedInMonth + "",
                ValueLabelColor = SKColor.Parse("#785C47"),
                Color = SKColor.Parse("#785C47")
            };

            return EatenDiscardedEntries;
        }

        private void PreviousMonthButton_Clicked(object sender, EventArgs e)
        {   //if it's the first month of the year and you go to
            //the last month of last year, 
            if (month == 1)
                year--;

            month--;

            maxEaten = 0;
            maxDiscarded = 0;

            //update the month, days in month and year based on the decrementer
            SetMonthAttributes();

            //update list of days from calendar
            days = App.CalendarDatabase.GetCalendarAsync(month, year).Result;

            MainThread.BeginInvokeOnMainThread(
                () => UpdateCharts());
        }

        private void NextMonthButton_Clicked(object sender, EventArgs e)
        {
            
            if(month != currentDay.Month || year != currentDay.Year)
            {
                //if it's the last month of last year and
                //you go to the first month of the next year,
                //it should update the year to the next year
                if (month == 12)
                    year++;

                month++;

                maxEaten = 0;
                maxDiscarded = 0;
                //update the month and days in month and year based on the decrementer
                SetMonthAttributes();
                //update list of days from calendar
                days = App.CalendarDatabase.GetCalendarAsync(month, year).Result;

                MainThread.BeginInvokeOnMainThread(
                    () => UpdateCharts());
            }
        }

        private async void BackButton_Clicked(object sender, EventArgs e)
        {
            await Navigation.PopModalAsync();
        }
    }
}
