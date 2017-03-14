using System;

using Xamarin.Forms;

namespace HSAManager
{
	public class CustomCell : ContentPage
	{
		public CustomCell()
		{
			Title = "Simple";
			Padding = new Thickness(0, 20, 0, 0);

			var listView = new ListView();
			listView.ItemsSource = data.StringList;
			Content = listView;
		}
	}
}

