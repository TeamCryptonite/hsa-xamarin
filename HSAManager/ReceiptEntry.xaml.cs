using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

using Xamarin.Forms;

namespace HSAManager
{
	public partial class ReceiptEntry : ContentPage 
	{
		ObservableCollection<HsaServiceDtos.LineItemDto> lineItemNames = new ObservableCollection<HsaServiceDtos.LineItemDto>();

		public HsaServiceDtos.ReceiptDto thisReceipt = new HsaServiceDtos.ReceiptDto();
		public HsaServiceDtos.LineItemDto lineItemDto = new HsaServiceDtos.LineItemDto();

		public ReceiptEntry()
		{
			InitializeComponent();
			addItemInfo.ItemsSource = lineItemNames;
		}

		void getBlob(object sender, System.EventArgs e)
		{
			// get image file type and retrieve blob
		}

		void addItem(object sender, System.EventArgs e)
		{
			lineItemDto.Product = new HsaServiceDtos.ProductDto();
			lineItemDto.Product.Name = item.Text;
			lineItemDto.Price = Convert.ToDecimal(price.Text);
			item.Text = "";
			price.Text = "";
			store.Text = "";
			date.Text = "";
			thisReceipt.LineItems.Add(lineItemDto);
			lineItemNames.Add(lineItemDto);
			lineItemDto = null;
			lineItemDto = new HsaServiceDtos.LineItemDto();

			
			//catch{

			//	DisplayAlert("Alert", "You must enter a number for the price", "OK");
			//}	

		}

		void submitReceipt(object sender, System.EventArgs e)
		{
			thisReceipt.DateTime = Convert.ToDateTime(date.Text);
			thisReceipt.Store = new HsaServiceDtos.StoreDto();
			thisReceipt.Store.Name = store.Text;
			//convert to json and send to db

		}
	}
}
