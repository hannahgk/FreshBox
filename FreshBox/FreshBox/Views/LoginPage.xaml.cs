using Amazon.CognitoIdentityProvider;
using Amazon.Extensions.CognitoAuthentication;
using Newtonsoft.Json;
using Plugin.Fingerprint;
using Plugin.Fingerprint.Abstractions;
using System;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace FreshBox.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class LoginPage : ContentPage
    {
        public LoginPage()
        {
            InitializeComponent();
            //BindingContext = new LoginViewModel();
            //var handler = new JwtSecurityTokenHandler();
            //var tokenS = handler.ReadToken(App.user.IdToken) as JwtSecurityToken;
            //lstToken.ItemsSource = tokenS.Claims.ToList();
            //string userSub = tokenS.Claims.First(claim => claim.Type == "sub").Value;

            //lblTitle.Text = "WELCOME " + tokenS.Claims.First(claim => claim.Type == "cognito:username").Value.ToUpper();
            lblTitle.Text = "You have been Logged In, please go back to the page to continue.";
            _ = Color.Black;
        }

        protected async override void OnAppearing()
        {
            base.OnAppearing();
            if (App.user == null)
            {

                //AuthenticationRequestConfiguration biometricConfig = new AuthenticationRequestConfiguration("Use your fingerprint to log in", "");
                //var resp = await CrossFingerprint.Current.AuthenticateAsync(biometricConfig);

                //!!! RESOURCE to fixing the error that Jack and Sheng had, and Sena Fixed the error by using the link:
                //https://social.msdn.microsoft.com/Forums/en-US/028ad81d-70f0-4c59-939c-3561c99dc7a0/how-to-check-if-the-device-has-biometrics?forum=xamarinforms

                var result = await CrossFingerprint.Current.IsAvailableAsync(true); //new
                Plugin.Fingerprint.Abstractions.FingerprintAuthenticationResult auth; //new

                if (result)
                {
                    try
                    {
                        // var res = await App.Current.MainPage.DisplayAlert("Success", "Your data are saved", "Ok", "Cancel");

                        string r = await RefreshToken(App.user.IdToken, App.user.AccessToken, App.user.RefreshToken, App.user.TokenIssued, App.user.Expires);
                        if (r == "Refreshed")
                        {
                            await SecureStorage.SetAsync("User", JsonConvert.SerializeObject(App.user));
                            await Navigation.PushAsync(new LoginPage());
                            Navigation.RemovePage(this);
                        }

                    }
                    catch
                    {
                        //This is to display popup letting user know before going into login. THIS IS NOT NEEDED BUT MAY IN THE FUTURE SO COMMENT THEM OUT.
                        //await App.Current.MainPage.DisplayAlert("permission to use FaceID", "We need permission to use FaceID", "Ok");

                        //Having nothing in catch will return;
                    }
                }
            }
        }

        //LoginButton
        private async void Button_Clicked(object sender, EventArgs e)
        {
            string v = await SignIn(UserNameTextBox.Text, PasswordTextBox.Text);
            if (!v.StartsWith("Error"))
            {
                await SecureStorage.SetAsync("User", JsonConvert.SerializeObject(App.user));
                await DisplayAlert("Success", "You are logged in.", "Ok");
                await Navigation.PushAsync(new Logged());
                Navigation.RemovePage(this);
            }
            else if (v == "pass-change-required")
                await DisplayAlert("Password Change Required", "Check your email.", "Ok");
            else if (v.StartsWith("Error"))
                await DisplayAlert("Error", "Login Error. " + v, "Ok");
        }

        public async Task<string> SignIn(string userName, string password)
        {
            try
            {
                CognitoUserPool userPool = new CognitoUserPool(AWS.UserpoolID, AWS.ClientID, App.provider);
                CognitoUser user = new CognitoUser(userName, AWS.ClientID, userPool, App.provider);

                AuthFlowResponse context = await user.StartWithSrpAuthAsync(new InitiateSrpAuthRequest()
                {
                    Password = password
                }).ConfigureAwait(false);

                if (context.ChallengeName == ChallengeNameType.NEW_PASSWORD_REQUIRED)
                    return "pass-change-required";
                else
                {
                    App.user = new AWSUser
                    {
                        IdToken = context.AuthenticationResult?.IdToken,
                        RefreshToken = context.AuthenticationResult?.RefreshToken,
                        AccessToken = context.AuthenticationResult?.AccessToken,
                        TokenIssued = user.SessionTokens.IssuedTime,
                        Expires = user.SessionTokens.ExpirationTime
                    };
                    return "Logged in";
                }
            }
            catch (Exception e)
            {
                return "Error " + e.Message;
            }
        }

        //SignUp button
        private async void Button_Clicked_1(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new SignUp());
        }

        //send user to Fly page on click from bottom navbar
        private void FlyPage(object sender, EventArgs e)
        {
            Shell.Current.FlyoutIsPresented = true;
        }

        //send user to Home page on click from bottom navbar
        private async void HomePage(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new AboutPage());
        }

        //This is just for an example inorder to sucess clicking a button for an image icon
        //Need able to go to each Social Media Platform to log in
        private void ImageButton_Clicked(object sender, EventArgs e)
        {
            DisplayAlert("Success", "User Registered", "OK");
        }

        public async Task<string> RefreshToken(string idToken, string accessToken, string refreshToken, DateTime issued, DateTime expires)
        {
            try
            {
                CognitoUserPool userPool = new CognitoUserPool(AWS.UserpoolID, AWS.ClientID, App.provider);
                CognitoUser user = new CognitoUser("", AWS.ClientID, userPool, App.provider)
                {
                    // We have to use expire time of REFRESH TOKEN not ACCESS TOKEN, now forcing refresh adding 1h as we don't have REFRESH TOKEN expire date
                    SessionTokens = new CognitoUserSession(idToken, accessToken, refreshToken, issued, DateTime.Now.AddHours(1))
                };

                AuthFlowResponse context = await user.StartWithRefreshTokenAuthAsync(new InitiateRefreshTokenAuthRequest
                {
                    AuthFlowType = AuthFlowType.REFRESH_TOKEN_AUTH
                })
                .ConfigureAwait(false);

                App.user = new AWSUser
                {
                    IdToken = context.AuthenticationResult?.IdToken,
                    RefreshToken = App.user.RefreshToken,
                    AccessToken = context.AuthenticationResult?.AccessToken,
                    TokenIssued = user.SessionTokens.IssuedTime,
                    Expires = user.SessionTokens.ExpirationTime
                };
                return "Refreshed";
            }
            catch (Exception e)
            {
                return "Error " + e.Message;
            }
        }

        private async void Button_Clicked_2(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new ForgotPassword());
            //if (App.user != null)
            //{
            //    AuthenticationRequestConfiguration biometricConfig = new AuthenticationRequestConfiguration("Use your fingerprint to log in", "");

            //    var resp = await CrossFingerprint.Current.AuthenticateAsync(biometricConfig);

            //    if (resp.Authenticated)
            //    {
            //        string r = await RefreshToken(App.user.IdToken, App.user.AccessToken, App.user.RefreshToken, App.user.TokenIssued, App.user.Expires);
            //        if (r == "Refreshed")
            //        {
            //            await SecureStorage.SetAsync("User", JsonConvert.SerializeObject(App.user));
            //            await Navigation.PushAsync(new Logged());
            //            Navigation.RemovePage(this);
            //        }
            //    }
            //}
        }
    }
}