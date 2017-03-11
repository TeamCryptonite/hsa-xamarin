using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Xamarin.Forms;
using HSAManager;
using HSAManager.Droid;
using Xamarin.Forms.Platform.Android;
using Microsoft.Identity.Client;

[assembly: ExportRenderer(typeof(StartScreen), typeof(StartScreenRenderer))]
namespace HSAManager.Droid
{
	class StartScreenRenderer : PageRenderer
	{
		StartScreen page;

		protected override void OnElementChanged(ElementChangedEventArgs<Page> e)
		{
			base.OnElementChanged(e);
			page = e.NewElement as StartScreen;
			var activity = this.Context as Activity;
			page.platformParameters = new PlatformParameters(activity);
		}

	}
}