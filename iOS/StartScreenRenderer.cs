using Microsoft.Identity.Client;
using System;
using System.Collections.Generic;
using System.Text;
using HSAManager;
using HSAManager.iOS;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;

[assembly: ExportRenderer(typeof(StartScreen), typeof(StartScreenRenderer))]
namespace HSAManager.iOS
{
	class StartScreenRenderer : PageRenderer
	{
		StartScreen page;
		protected override void OnElementChanged(VisualElementChangedEventArgs e)
		{
			base.OnElementChanged(e);
			page = e.NewElement as StartScreen;
		}
		public override void ViewDidLoad()
		{
			base.ViewDidLoad();
			page.platformParameters = new PlatformParameters(this);
		}
	}
}
