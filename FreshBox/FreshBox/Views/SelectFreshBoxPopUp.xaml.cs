using FreshBox.Models;
using FreshBox.ViewModels;
using System;
using System.Threading.Tasks;
using Xamarin.CommunityToolkit.Extensions;
using Xamarin.CommunityToolkit.UI.Views;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace FreshBox.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    //If it gives erros, Try this link: https://stackoverflow.com/questions/42386406/xamarin-forms-xaml-xamlparseexception 
    public partial class SelectFreshBoxPopUp : Popup
    {
        bool newFreshBox = false;

        public SelectFreshBoxPopUp()
        {
            InitializeComponent();
            newFreshBox = true;
            typeButton.IsVisible = false;
            BindingContext = new FreshBoxViewModel();
        }
        public SelectFreshBoxPopUp(FreshBoxes freshbox)
        {
            newFreshBox = false;
            
            BindingContext = new FreshBoxViewModel();
            FreshBoxViewModel.freshbox = freshbox;

            if (FreshBoxViewModel.freshbox.Image.Contains("mini"))
            {
                FreshBoxViewModel.GetMiniFridgeImages();
            }
            else if (FreshBoxViewModel.freshbox.Image.Contains("fridge"))
            {
                FreshBoxViewModel.GetFridgeImages();
            }
            else if (FreshBoxViewModel.freshbox.Image.Contains("freezer"))
            {
                FreshBoxViewModel.GetFreezerImages();
            }
            else if (FreshBoxViewModel.freshbox.Image.Contains("pantry"))
            {
                FreshBoxViewModel.GetPantryImages();
            }
            else if (FreshBoxViewModel.freshbox.Image.Contains("other"))
            {
                FreshBoxViewModel.GetOtherImages();
            }

            InitializeComponent();
            nameEntry.Text = freshbox.Name;
            
            typeButton.IsVisible = true;

        }

       
        public async void AddFreshBoxButtonClicked(object sender, EventArgs e)
        {
            ImageButton imgbutton = (ImageButton)sender;
            //super hacky way to make images update correctly on fridge/button
            string imageName = $"{imgbutton.Source}";
            imageName = imageName.Substring(6);
            if (!string.IsNullOrWhiteSpace(nameEntry.Text))
            {
                if(newFreshBox)
                {
                    _ = await App.FreshBoxDatabase.SaveFreshBoxAsync(new FreshBoxes
                    {
                        Id = Guid.NewGuid().ToString(),
                        Name = nameEntry.Text,
                        Image = imageName, //The image clicked on is passed in
                        IsFav = false
                    });
                }
                else
                {
                    _ = await App.FreshBoxDatabase.UpdateFreshBoxAsync(new FreshBoxes
                    {
                        Id = FreshBoxViewModel.freshbox.Id,
                        Name = nameEntry.Text,
                        Image = imageName, //The image clicked on is passed in
                        IsFav = FreshBoxViewModel.freshbox.IsFav
                    });

                }

                nameEntry.Text = string.Empty;

                var result = await App.FreshBoxDatabase.GetFreshBoxesAsync();
                Dismiss(result);
            }
        }

        private async void typeButton_ClickedAsync(object sender, EventArgs e)
        {
            FreshBoxViewModel.freshbox.Name = nameEntry.Text;
            _ = await Navigation.ShowPopupAsync(new AddFreshBoxPopUp(FreshBoxViewModel.freshbox));
            Dismiss(this);
            return;
        }


        

    }
}