using Microsoft.Identity.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;

namespace HSAManager
{
	public partial class HSAManagerPage : ContentPage
	{
		public IPlatformParameters platformParameters { get; set; }

		void Handle_Clicked_Receipt(object sender, System.EventArgs e)
		{
			Navigation.PushAsync(new receiptVault());
		}

		void Handle_Clicked_Charts(object sender, System.EventArgs e)
		{
			Navigation.PushAsync(new data());
		}

		void Handle_Clicked_Management(object sender, System.EventArgs e)
		{
			Navigation.PushAsync(new balanceManagement());
		}

		void Handle_Clicked_Products(object sender, System.EventArgs e)
		{
			Navigation.PushAsync(new products());
		}

		void Handle_SL(object sender, System.EventArgs e)
		{
			Navigation.PushAsync(new shoppingList());
		}

		public HSAManagerPage()
		{
			InitializeComponent();
		}

		protected override async void OnAppearing()
		{
			App.PCApplication.PlatformParameters = platformParameters;
			// let's see if we have a user in our belly already
			try
			{
				//AuthenticationResult ar = await App.PCApplication.AcquireTokenSilentAsync(App.Scopes, "", App.Authority, App.SignUpSignInpolicy, false);
				AuthenticationResult ar = await App.PCApplication.AcquireTokenAsync(App.Scopes, "", UiOptions.SelectAccount, string.Empty, null, App.Authority, App.SignUpSignInpolicy);
				//Navigation.PushAsync(new HSAManagerPage());
			}
			catch
			{
				// doesn't matter, we go in interactive more
			}
		}


		async void OnForgotPassword()
		{
			try
			{
				AuthenticationResult ar = await App.PCApplication.AcquireTokenAsync(App.Scopes, "", UiOptions.SelectAccount, string.Empty, null, App.Authority, App.ResetPasswordpolicy);
				//Navigation.PushAsync(new TasksPage());
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
				//Navigation.PushAsync(new TasksPage());
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
