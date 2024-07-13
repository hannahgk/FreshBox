using System;
using SQLite;

namespace FreshBox.Models
{
    public class Item
    {
        //Id must be manually set with Id = Guid.NewGuid().ToString() when constructing new items
        [PrimaryKey]
        public string Id { get; set; }
        public string Name { get; set; }
        public string Barcode { get; set; }
        public string Image { get; set; }
        public double Quantity { get; set; }
        public DateTime ExpiryDate { get; set; }
        public string FridgeId { get; set; }
    }
}
