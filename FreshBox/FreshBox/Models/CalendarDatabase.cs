using SQLite;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FreshBox.Models
{
    public class CalendarDatabase
    {
        private readonly SQLiteAsyncConnection calendarDatabase;

        public CalendarDatabase(string dbPath)
        {
            calendarDatabase = new SQLiteAsyncConnection(dbPath);
            calendarDatabase.CreateTableAsync<Day>().Wait();
        }

        public Day GetDayAsync(DateTime date)
        {
            Day day = calendarDatabase.Table<Day>().FirstOrDefaultAsync(x => x.Date == date).Result;
            return day;
        }
         
        public Task<List<Day>> GetDaysAsync()
        {
            return calendarDatabase.Table<Day>().ToListAsync();
        }

        public Day GetCurrentDayAsync()
        {
            return calendarDatabase.Table<Day>().FirstOrDefaultAsync(x => x.isCurrentDay).Result ?? NullCurrentDayAsync();
        }

        private Day NullCurrentDayAsync()
        {
            Day newDay = new Day();
            calendarDatabase.InsertAsync(newDay).Wait();
            return newDay;
        }
        public Task<int> SaveDayAsync(Day day)
        {
            return calendarDatabase.InsertAsync(day);
        }

        public Task<int> UpdateDayAsync(Day day)
        {
            return calendarDatabase.UpdateAsync(day);
        }
        public Task<int> DeleteDayAsync(Day day)
        {
            return calendarDatabase.DeleteAsync(day);
        }

        public Task<List<Day>> GetCalendarAsync(int Month, int Year)
        {
            Task<List<Day>> days = calendarDatabase.Table<Day>().Where(x => x.Month == Month && x.Year == Year).ToListAsync();
            return days;
        }

    }
}