using System;
using Microsoft.Identity.Client;
using Xamarin.Forms;

namespace HSAManager
{
    public partial class StartScreen : ContentPage
    {
        public StartScreen()
        {
            InitializeComponent();
        }

        public IPlatformParameters platformParameters { get; set; }


        protected override async void OnAppearing()
        {
            App.PCApplication.PlatformParameters = platformParameters;
            // let's see if we have a user in our belly already
            try
            {
                var ar = await App.PCApplication.AcquireTokenSilentAsync(App.Scopes, "", App.Authority,
                    App.SignUpSignInpolicy, false);
                Application.Current.Properties["authKey"] = ar.Token;
                await Navigation.PushAsync(new Dashboard());
				Navigation.RemovePage(this);
            }
            catch
            {
                // doesn't matter, we go in interactive more
            }
        }


        private async void OnForgotPassword()
        {
            try
            {
                var ar = await App.PCApplication.AcquireTokenAsync(App.Scopes, "", UiOptions.SelectAccount, string.Empty,
                    null, App.Authority, App.ResetPasswordpolicy);
                Application.Current.Properties["authKey"] = ar.Token;
                await Navigation.PushAsync(new Dashboard());
				Navigation.RemovePage(this);
            }
            catch (MsalException ee)
            {
                if (ee.ErrorCode != "authentication_canceled")
                    await DisplayAlert("An error has occurred", "Exception message: " + ee.Message, "Dismiss");
            }
        }


        private async void OnSignUpSignIn(object sender, EventArgs e)
        {
            try
            {
                var ar = await App.PCApplication.AcquireTokenAsync(App.Scopes, "", UiOptions.SelectAccount, string.Empty,
                    null, App.Authority, App.SignUpSignInpolicy);
                Application.Current.Properties["authKey"] = ar.Token;
                await Navigation.PushAsync(new Dashboard());
				Navigation.RemovePage(this);
            }
            catch (MsalException ee)
            {
                if (ee.Message != null && ee.Message.Contains("AADB2C90118"))
                    OnForgotPassword();

                //if (ee.ErrorCode != "authentication_canceled")
                //{
                //	await DisplayAlert("An error has occurred", "Exception message: " + ee.Message, "Dismiss");
                //}
            }
        }
    }
}