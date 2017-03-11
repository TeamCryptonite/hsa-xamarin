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

[assembly: ExportRenderer(typeof(StartPage), typeof(HSAManagerPageRenderer))]
namespace HSAManager.Droid
{
	class HSAManagerPageRenderer : PageRenderer
	{
		StartPage page;

		protected override void OnElementChanged(ElementChangedEventArgs<Page> e)
		{
			base.OnElementChanged(e);
			page = e.NewElement as StartPage;
			var activity = this.Context as Activity;
			page.platformParameters = new PlatformParameters(activity);
		}

	}
}
