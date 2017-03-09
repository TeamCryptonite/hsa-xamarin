using Microsoft.Identity.Client;
using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;
using HSAManager;
using HSAManager.iOS;
using Xamarin.Forms.Platform.iOS;

[assembly: ExportRenderer(typeof(StartPage), typeof(HSAManagerPageRenderer))]
namespace HSAManager.iOS
{
	class HSAManagerPageRenderer : PageRenderer
	{
		StartPage page;
		protected override void OnElementChanged(VisualElementChangedEventArgs e)
		{
			base.OnElementChanged(e);
			page = e.NewElement as StartPage;
		}
		public override void ViewDidLoad()
		{
			base.ViewDidLoad();
			page.platformParameters = new PlatformParameters(this);
		}
	}
}
