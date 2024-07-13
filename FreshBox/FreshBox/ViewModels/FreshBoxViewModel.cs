using FreshBox.Models;

namespace FreshBox.ViewModels
{
    class FreshBoxViewModel
    {
        public static FreshBoxes freshbox;

        //list of possible Fridge images
        public static string[] FridgeImageList = new string[] { "fridge1.png", "fridge2.png", "fridge3.png", "fridge4.png" };
        public static string[] FreezerImageList = new string[] {"freezer1.png", "freezer2.png", "freezer3.png", "freezer4.png" };
        public static string[] MiniFridgeImageList = new string[] { "minifridge1.png", "minifridge2.png", "minifridge3.png", "minifridge4.png" };
        public static string[] PantryImageList = new string[] { "pantry1.png", "pantry2.png", "pantry3.png", "pantry4.png" };
        public static string[] OtherImageList = new string[] { "other1.png", "other2.png", "other3.png", "other4.png" };

        //for Data Binding in SelectFridgePopup.xaml
        public static string Image1;
        public static string Image2;
        public static string Image3;
        public static string Image4;

        public string ImageOne => Image1;
        public string ImageTwo => Image2;
        public string ImageThree => Image3;
        public string ImageFour => Image4;

        public static void GetFridgeImages()
        {
            Image1 = FridgeImageList[0];
            Image2 = FridgeImageList[1];
            Image3 = FridgeImageList[2];
            Image4 = FridgeImageList[3];
        }

        public static void GetFreezerImages()
        {
            Image1 = FreezerImageList[0];
            Image2 = FreezerImageList[1];
            Image3 = FreezerImageList[2];
            Image4 = FreezerImageList[3];
        }

        public static void GetMiniFridgeImages()
        {
            Image1 = MiniFridgeImageList[0];
            Image2 = MiniFridgeImageList[1];
            Image3 = MiniFridgeImageList[2];
            Image4 = MiniFridgeImageList[3];
        }

        public static void GetPantryImages()
        {
            Image1 = PantryImageList[0];
            Image2 = PantryImageList[1];
            Image3 = PantryImageList[2];
            Image4 = PantryImageList[3];
        }

        public static void GetOtherImages()
        {
            Image1 = OtherImageList[0];
            Image2 = OtherImageList[1];
            Image3 = OtherImageList[2];
            Image4 = OtherImageList[3];
        }

    }
}
