using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using SQLite;

namespace FreshBox.Models
{
    public class ItemDatabase
    {
        private readonly SQLiteAsyncConnection itemDatabase;
        private readonly SQLiteConnection syncItemDatabase;

        public ItemDatabase(string dbPath)
        {
            itemDatabase = new SQLiteAsyncConnection(dbPath);
            itemDatabase.CreateTableAsync<Item>().Wait();
            syncItemDatabase = new SQLiteConnection(dbPath);
            syncItemDatabase.CreateTable<Item>();
        }

        //sync
        public List<Item> GetItems()
        {
            return syncItemDatabase.Table<Item>().ToList();
        }

        public Task<List<Item>> GetItemsAsync()
        {
            return itemDatabase.Table<Item>().ToListAsync();
           
        }

        public Task<List<Item>> GetItemsByBox(string freshBoxId)
        {
            AsyncTableQuery<Item> itemList = itemDatabase.Table<Item>().Where(x => x.FridgeId.Equals(freshBoxId));
            return itemList.ToListAsync();
        }

        public Task<List<Item>> GetItemsByBarcode(string barcode)
        {
            AsyncTableQuery<Item> itemList = itemDatabase.Table<Item>().Where(x => x.Barcode.Equals(barcode));
            return itemList.ToListAsync();
        }

        public Task<List<Item>> GetItemsByDate(DateTime date)
        {
            AsyncTableQuery<Item> itemList = itemDatabase.Table<Item>().Where(x => x.ExpiryDate == date);
            return itemList.ToListAsync();
        }

        //sync
        public List<Item> GetItemsByDateSync(DateTime date)
        {
            TableQuery<Item> itemList = syncItemDatabase.Table<Item>().Where(x => x.ExpiryDate == date);
            return itemList.ToList();
        }

        public Task<List<Item>> GetItemsSortedByDate()
        {
            AsyncTableQuery<Item> itemList = itemDatabase.Table<Item>().OrderBy(x => x.ExpiryDate);
            return itemList.ToListAsync();
        }

        //sync
        public List<Item> GetItemsSortedByDateSync()
        {
            TableQuery<Item> itemList = syncItemDatabase.Table<Item>().OrderBy(x => x.ExpiryDate);
            return itemList.ToList();
        }

        //NEED TO ADD ERROR HANDLING
        public Task<List<Item>> GetTopItemsSortedByDate(string num)
        {
            AsyncTableQuery<Item> itemList = itemDatabase.Table<Item>().OrderBy(x => x.ExpiryDate).Take(Int32.Parse(num));
            return itemList.ToListAsync();
        }

        public Task<List<Item>> GetItemSearchResults(string queryString)
        {
            var normalizedQuery = queryString?.ToLower() ?? "";
            return itemDatabase.Table<Item>().Where(f => f.Name.ToLower().Contains(normalizedQuery)).ToListAsync();
        }

        public List<Item> GetItemSearchResultsSync(string queryString)
        {
            var normalizedQuery = queryString?.ToLower() ?? "";
            return syncItemDatabase.Table<Item>().Where(f => f.Name.ToLower().Contains(normalizedQuery)).ToList();
        }

        public Task<List<Item>> GetTopItemSearchResults(string queryString, int num)
        {
            var normalizedQuery = queryString?.ToLower() ?? "";
            return itemDatabase.Table<Item>().Where(f => f.Name.ToLower().Contains(normalizedQuery)).Take(num).ToListAsync();
        }

        public List<Item> GetTopItemSearchResultsSync(string queryString, int num)
        {
            var normalizedQuery = queryString?.ToLower() ?? "";
            return syncItemDatabase.Table<Item>().Where(f => f.Name.ToLower().Contains(normalizedQuery)).Take(num).ToList();
        }

        public Task<Item> GetItemAsync(string itemId)
        {
            return itemDatabase.Table<Item>().Where(x => x.Id.Equals(itemId)).FirstAsync();
        }

        public Task<int> SaveItemAsync(Item item)
        {
            return itemDatabase.InsertAsync(item);
        }

        public Task<int> UpdateItemAsync(Item item)
        {
            return itemDatabase.UpdateAsync(item);
        }

        public Task<int> DeleteItemAsync(Item item)
        {
            return itemDatabase.DeleteAsync(item);
        }

        public async Task<int> DeleteItemsByBox(string freshBoxId)
        {
            var query = itemDatabase.Table<Item>().Where(x => x.FridgeId.Equals(freshBoxId));
            List<Item> list = await query.ToListAsync();
            int rowsDeleted = 0;
            foreach(Item item in list)
            {
                await DeleteItemAsync(item);
                rowsDeleted++;
            }
            return rowsDeleted;
        }

    }
}