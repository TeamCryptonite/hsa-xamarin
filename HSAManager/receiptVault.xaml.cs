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
		public ObservableCollection<HsaServiceDtos.ReceiptDto> receipts; //= new ObservableCollection<HsaServiceDtos.ReceiptDto>();
		public HsaServiceDtos.ReceiptDto receipt = new HsaServiceDtos.ReceiptDto();


		public receiptVault()
		{
			receipt.Store = new HsaServiceDtos.StoreDto();

			InitializeComponent();
			if (Application.Current.Properties.ContainsKey("authKey"))
			{
				string authKey = Application.Current.Properties["authKey"].ToString();
				var client = new BizzaroClient(authKey);

				receipts = new ObservableCollection<HsaServiceDtos.ReceiptDto>(client.Receipts.GetListOfReceipts());

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
