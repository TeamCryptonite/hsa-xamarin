﻿using System;
using System.Collections.Generic;
using Microsoft.Identity.Client;

using Xamarin.Forms;

namespace HSAManager
{
	public partial class StartScreen : ContentPage
	{

		public IPlatformParameters platformParameters { get; set; }

		public StartScreen()
		{
			InitializeComponent();
		}

		protected override async void OnAppearing()
		{
			App.PCApplication.PlatformParameters = platformParameters;
			// let's see if we have a user in our belly already
			try
			{
				AuthenticationResult ar = await App.PCApplication.AcquireTokenSilentAsync(App.Scopes, "", App.Authority, App.SignUpSignInpolicy, false);
				await Navigation.PushAsync(new Dashboard());
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
				Navigation.PushAsync(new Dashboard());
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
				Navigation.PushAsync(new Dashboard());
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
