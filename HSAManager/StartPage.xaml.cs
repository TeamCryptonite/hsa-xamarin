using Microsoft.Identity.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;

namespace HSAManager
{
	public partial class StartPage : ContentPage
	{
		public IPlatformParameters platformParameters { get; set; }

		public StartPage()
		{
			InitializeComponent();
		}

		protected override async void OnAppearing()
		{
			App.PCApplication.PlatformParameters = platformParameters;
			try
			{
				//AuthenticationResult ar = await App.PCApplication.AcquireTokenSilentAsync(App.Scopes, "", App.Authority, App.SignUpSignInpolicy, false);
				//Navigation.PushAsync(new HSAManager.HSAManagerPage());
			}
			catch
			{
				// doesn't matter, we go in interactive more
			}
			//// let's see if we have a user in our belly already
			//try
			//{
			//	//AuthenticationResult ar = await App.PCApplication.AcquireTokenSilentAsync(App.Scopes, "", App.Authority, App.SignUpSignInpolicy, false);
			//	AuthenticationResult ar = await App.PCApplication.AcquireTokenAsync(App.Scopes, "", UiOptions.SelectAccount, string.Empty, null, App.Authority, App.SignUpSignInpolicy);
			//	//Navigation.PushAsync(new HSAManagerPage());
			//}
			//catch
			//{
			//	// doesn't matter, we go in interactive more
			//}
		}


		async void OnForgotPassword()
		{
			try
			{
				AuthenticationResult ar = await App.PCApplication.AcquireTokenAsync(App.Scopes, "", UiOptions.SelectAccount, string.Empty, null, App.Authority, App.ResetPasswordpolicy);
				Navigation.PushAsync(new HSAManager.HSAManagerPage());
			}
			catch (MsalException ee)
			{
				if (ee.ErrorCode != "authentication_canceled")
				{
					DisplayAlert("An error has occurred", "Exception message: " + ee.Message, "Dismiss");
				}
			}
		}


		async void OnSignUpSignIn(object sender, EventArgs e)
		{
			try
			{
				AuthenticationResult ar = await App.PCApplication.AcquireTokenAsync(App.Scopes, "", UiOptions.SelectAccount, string.Empty, null, App.Authority, App.SignUpSignInpolicy);

				Navigation.PushAsync(new HSAManagerPage());


			}
			catch (MsalException ee)
			{
				if (ee.Message != null && ee.Message.Contains("AADB2C90118"))
				{
					OnForgotPassword();
				}

				if (ee.ErrorCode != "authentication_canceled")
				{
					DisplayAlert("An error has occurred", "Exception message: " + ee.Message, "Dismiss");
				}
			}
		}
	}
}
