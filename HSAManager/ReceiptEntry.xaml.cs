using System;
using System.Collections.ObjectModel;
using HsaServiceDtos;

using Xamarin.Forms;
using HsaServiceDtos;

namespace HSAManager
{
	public partial class ReceiptEntry : ContentPage 
	{
		ObservableCollection<LineItemDto> lineItemNames = new ObservableCollection<LineItemDto>();

		ReceiptDto thisReceipt = new ReceiptDto();
		public LineItemDto lineItemDto = new LineItemDto();

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
			lineItemDto.Product = new ProductDto();
			if(item.Text != "")
			{
				lineItemDto.Product.Name = item.Text;
			}
			else
			{
				DisplayAlert("Alert", "You must enter a product name", "OK");
				return;
			}
			if (price.Text != "")
			{
				try
				{
					lineItemDto.Price = Convert.ToDecimal(price.Text);
				}
				catch
				{
					DisplayAlert("Alert", "You must enter a number for the price", "OK");
				}
			}
			else
			{
				DisplayAlert("Alert", "You must enter a number for the price", "OK");
				return;
			}
			item.Text = "";
			price.Text = "";
			store.Text = "";
			date.Text = "";
			thisReceipt.LineItems.Add(lineItemDto);
			lineItemNames.Add(lineItemDto);
			lineItemDto = null;
			lineItemDto = new LineItemDto();

			
			//catch{

			//	DisplayAlert("Alert", "You must enter a number for the price", "OK");
			//}	

		}

		void submitReceipt(object sender, System.EventArgs e)
		{
			thisReceipt.DateTime = Convert.ToDateTime(date.Text);
			thisReceipt.Store = new StoreDto();
			thisReceipt.Store.Name = store.Text;
			//convert to json and send to db

		}
	}
}
