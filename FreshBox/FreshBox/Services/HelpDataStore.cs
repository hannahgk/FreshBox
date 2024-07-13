using FreshBox.Models;
using System.Collections.Generic;
using System.Linq;



namespace FreshBox.Services
{
    class HelpDataStore
    {
        public static HelpItems HelpTopics { get; } = new HelpItems()
           {
               new HelpItem{Id = "Id1", Text = "What is FreshBox?", Description = "FreshBox is an innovative way to organize and " +
                   "track your item's expiration dates. You can create fridges and freezers to hold your items, and " +
                   "when an item is going to expire, FreshBox will conveniently notify you! FreshBox will save you money, " +
                   "and help the planet!" },
               new HelpItem{Id = "Id2", Text = "Who is The FreshBox Team?", Description = "We have a passion for making the world " +
                   "a better place by developing innovative technologies. Check out our socials here! <socials>" },
               new HelpItem{Id = "Id3", Text = "Who is FreshBox for?", Description = "FreshBox is for the person that needs to eat that salad" +
                   " in the back of the fridge, finish their leftovers, replace their batteries, throw away their makeup products, " +
                   "buy new medications, help reduce food waste ... It's for anyone and everyone that has items that can expire " +
                   "and wants to make a difference to the planet." },
               new HelpItem{Id = "Id4", Text = "How do I create a FreshBox?", Description = "On the Home Page, select the second button" +
                   " to the right. This is where your FreshBoxes will be. Create a FreshBox by pressing the 'Create FreshBox' button. Select" +
                   " what type of FreshBox you would like, name it, and select an icon that you like!" },
               new HelpItem{Id = "Id5", Text = "How do I add an item?", Description = "You can add an item by pressing the green button " +
                   "on the Home Page, or you can select a FreshBox on the FreshBox Page and add an item by selecting 'add item' on the Item Page." },
               new HelpItem{Id = "Id6", Text = "How do I edit or delete a FreshBox?", Description = "Swipe right on a FreshBox to edit or delete it!" },
               new HelpItem{Id = "Id7", Text = "How do I edit or delete an item?", Description = "Swipe right on an item to edit or delete it! But if " +
                   "you have eaten or discarded the item, make sure to record it so you can see your progress in Analytics." },
               new HelpItem{Id = "Id8", Text = "How will I know if my item is about to expire?", Description = "When you add an item, you " +
                   "will be notified the day before or the day of your item's expiration." },
               new HelpItem{Id = "Id9", Text = "How can I see expired items?", Description = "On the home page, press the third button to " +
                   "the right on the bottom bar. It'll open your expiration list, where you can select the 'expired' tab. There, you can " +
                   "look at the items that have expired that haven't been eaten or thrown away." },
               new HelpItem{Id = "Id10", Text = "How can I see unexpired items?", Description = "On the home page, press the third button to " +
                   "the right on the bottom bar. It'll open your expiration list, where you can select the 'unexpired' tab. There, you can " +
                   "look at the items that haven't expired, been eaten or thrown away." },
               new HelpItem{Id = "Id11", Text = "How do I register an item as eaten or discarded?", Description = "When looking at an item, " +
                   "press the carrot to expand the eaten/discarded menu. Then select if your item has been eaten or discarded to register it. " +
                   "It will be removed from that fridge." },
               new HelpItem{Id = "Id12", Text = "What is the Stats Page?", Description = "The Stats Page is where you can check your progress " +
                   "in reducing your food waste and saving the planet! It records how much food you eat and throw away each month - " +
                   "the less food you throw away, the better." },
           };
        public static IEnumerable<HelpItem> GetSearchResults(string queryString)
        {
            var normalizedQuery = queryString?.ToLower() ?? "";
            IEnumerable<HelpItem> Resultset = HelpTopics.Where(f => f.Text.ToLower().Contains(normalizedQuery)).Select(f=>f);
          
            return Resultset;
        }

    }
}
