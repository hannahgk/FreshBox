using System.Collections.ObjectModel;
using Xamarin.Forms;
using FreshBox.Models;

namespace FreshBox.ViewModels
{
    public class ExpiringSoonViewModel : ContentPage
    {

        public ObservableCollection<Item> ItemsExpSoon { get; private set; }

        public ExpiringSoonViewModel()
        {


        }
    }
}
