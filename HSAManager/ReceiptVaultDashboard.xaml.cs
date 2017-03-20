using System;
using System.Collections.Generic;

using Xamarin.Forms;

namespace HSAManager
{
	public partial class ReceiptVaultDashboard : ContentPage
	{
		public ReceiptVaultDashboard()
		{
			InitializeComponent();
		}

		void Handle_Receipt_Vault(object sender, System.EventArgs e)
		{
			Navigation.PushAsync(new receiptVault());
		}

		void Handle_OCR_Entry(object sender, System.EventArgs e)
		{
			Navigation.PushAsync(new OCREntry());
		}

		void Handle_Manual_Entry(object sender, System.EventArgs e)
		{
			Navigation.PushAsync(new ReceiptEntry());
		}
	}
}
