using Android.App;
using Android.Content;
using Android.OS;
using System;
using System.Threading.Tasks;

namespace FreshBox.Droid
{
    [Activity(Label = "FreshBox", MainLauncher = true, Theme = "@style/MyTheme.Splash", NoHistory = true)]
    public class SplashActivity : Activity
    {
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            var intent = new Intent(this, typeof(MainActivity));
            intent.AddFlags(ActivityFlags.ClearTop);
            intent.AddFlags(ActivityFlags.SingleTop);
            StartActivity(intent);
            Finish();

            // Create your application here
        }

        async Task SimulateStartup()
        {
            await Task.Delay(TimeSpan.FromSeconds(2));
            StartActivity(new Intent(Application.Context, typeof(MainActivity)));
        }
    }
}