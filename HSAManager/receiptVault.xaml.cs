using System;
using System.Collections.Generic;
using System.Linq;
using System.Collections.ObjectModel;

using Xamarin.Forms;
//using HsaServiceDtos;

namespace HSAManager
{
	public partial class receiptVault : ContentPage
	{
		public ObservableCollection<HsaServiceDtos.ReceiptDto> receipts;

		public receiptVault()
		{
			InitializeComponent();
			if (Application.Current.Properties.ContainsKey("authKey"))
			{
				string authKey = Application.Current.Properties["authKey"].ToString();
				var client = new BizzaroClient();

				receipts = new ObservableCollection<HsaServiceDtos.ReceiptDto>(client.Receipts.GetListOfReceipts().Next());

			}

			listView.ItemsSource = receipts;

		}

		void searchChanged(object sender, Xamarin.Forms.TextChangedEventArgs e)
		{
			
		}

		//public async void Handle_Tapped(object sender, System.EventArgs e)
		//{
		//	await Navigation.PushAsync(new data());
		//}
	}
}
