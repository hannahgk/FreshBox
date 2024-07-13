using System.Collections.Generic;

namespace FreshBox.Models
{
    public class HelpItem
    {
        public string Id { get; set; }
        public string Text { get; set; }
        public string Description { get; set; }

        public List<string> ToLowerInvariant()
        {
            return new List<string> { Text.ToLower(), Id.ToLower(), Description.ToLower() };
        }
        //public IEnumerable<string> GetList()
        //{
        //    return new List<string> { Text.ToLower(), Id.ToLower(), Description.ToLower() };
        //}
    }
    public class HelpItems : List<HelpItem>
    {
        public IEnumerable<string> GetList()
        {
            return new List<string> { this[0].Text.ToLower(), this[0].Id.ToLower(), this[0].Description.ToLower() };
        }
    }
    }

