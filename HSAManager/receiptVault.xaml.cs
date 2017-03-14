using System;
using System.Collections.Generic;

using Xamarin.Forms;

namespace HSAManager
{
	public partial class receiptVault : ContentPage
	{
		List<string> names = new List<string>
		{
			"yes", "no", "maybe"
		};
		public receiptVault()
		{
			InitializeComponent();

			listView.ItemsSource = names;

		}
	}
}
