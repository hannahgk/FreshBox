using SQLite;
using System;

namespace FreshBox.Models
{
    public class Day
    {
        [PrimaryKey]
        public DateTime Date { get; set; }
        public int EatenCounter { get; set; }
        public int DiscardedCounter { get; set; }
        public int Month { get; set; }
        public int Year { get; set; }
        public bool isCurrentDay { get; set; }


        public Day()
        {
            //Eaten and Discarded counters must begin at 0
            EatenCounter = 0;
            DiscardedCounter = 0;

            //used to track days stored with actual days
            isCurrentDay = true;

            //assigns new date to the day
            Date = DateTime.Today;

            Month = Date.Month;

            Year = Date.Year;
        }

        //assigns a number to DayNum that can be used in graphs/stats page
        public int DayOfMonth()
        {
            return Date.Day;
        }

        //get number of days in month for graph
        public int DaysInMonth()
        {
            return DateTime.DaysInMonth(Date.Year, Date.Month);
        }
    }
}
