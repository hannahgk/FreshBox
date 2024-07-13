using FreshBox.Models;
using FreshBox.ViewModels;
using System;
using Xamarin.CommunityToolkit.Extensions;
using Xamarin.CommunityToolkit.UI.Views;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace FreshBox.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class AddFreshBoxPopUp : Popup
    {
        readonly bool newFreshBox;
        FreshBoxes fB;

        public AddFreshBoxPopUp()
        {
            InitializeComponent();
            newFreshBox = true;
        }

        public AddFreshBoxPopUp(FreshBoxes freshbox)
        {
            InitializeComponent();
            fB = freshbox;
            newFreshBox = false;
        }

        /*
* When SelectFreshBoxButtonClicked, makes new SelectFreshBoxPopUp.
*/
        private async void SelectFreshBoxButtonClicked(object sender, EventArgs e)
        {
            //id button
            Button btn = (Button)sender;

            if (newFreshBox)
            {
                //if button text is fridge, initialize fridge images,
                //If button text is freezer, initialize freezer images... etc
                if (btn.Text.Equals("Fridge"))
                {
                    FreshBoxViewModel.GetFridgeImages();
                }
                else if (btn.Text.Equals("Freezer"))
                {
                    FreshBoxViewModel.GetFreezerImages();
                }
                else if (btn.Text.Equals("Mini-fridge"))
                {
                    FreshBoxViewModel.GetMiniFridgeImages();
                }
                else if (btn.Text.Equals("Pantry"))
                {
                    FreshBoxViewModel.GetPantryImages();
                }
                else if (btn.Text.Equals("Other"))
                {
                    FreshBoxViewModel.GetOtherImages();
                }
                var result = await Navigation.ShowPopupAsync(new SelectFreshBoxPopUp());
                Dismiss(result);
            }
            //sets image of freshbox to first of new type
            //so when selectfreshbocpopup is called, the
            //correct images are displayed
            else
            {
                if (btn.Text.Equals("Fridge"))
                {
                    fB.Image = "fridge1.png";
                }
                else if (btn.Text.Equals("Freezer"))
                {
                    fB.Image = "freezer1.png";
                }
                else if (btn.Text.Equals("Mini-fridge"))
                {
                    fB.Image = "minifridge1.png";
                }
                else if (btn.Text.Equals("Pantry"))
                {
                    fB.Image = "pantry1.png";
                }
                else if (btn.Text.Equals("Other"))
                {
                    fB.Image = "other1.png";
                }

                var result = await Navigation.ShowPopupAsync(new SelectFreshBoxPopUp(fB));
                Dismiss(result);
            }
            
        }
    }
}