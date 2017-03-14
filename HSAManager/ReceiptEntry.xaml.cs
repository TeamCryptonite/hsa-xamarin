using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

using Xamarin.Forms;

namespace HSAManager
{
	public partial class ReceiptEntry : ContentPage
	{
		ObservableCollection<string> receiptItems = new ObservableCollection<string>();

		public class lineItem
		{
			public lineItem(string store, string date, string name, double price)
			{
				this.Store = store;
				this.Date = date;
				this.Name = name;
				this.Price = price;
			}

			public string Store { private set; get; }
			public string Date { private set; get;}
			public string Name { private set; get;}
			public double Price { private set; get; }
		}

		public ReceiptEntry()
		{
			InitializeComponent();
			addInfo.ItemsSource = receiptItems;
		}

		void getBlob(object sender, System.EventArgs e)
		{
			// get image file type and retrieve blob
		}

		void addItems(object sender, System.EventArgs e)
		{
			try
			{
				lineItem lI = new lineItem(store.Text, date.Text, item.Text, Double.Parse(price.Text));
				item.Text = "";
				price.Text = "";
				store.Text = "";
				date.Text = "";
				receiptItems.Add(lI.Store + ", " + lI.Date + ", " + lI.Name + ", $" + lI.Price);
			}
			catch
			{
				DisplayAlert("Alert", "You must enter a number for the price", "OK");
			}	

			//for (int x = 0; x <= receiptItems.Count-1; x++) 
			//{
			//	if (x < 1)
			//	{
			//		itemsBox.Text= receiptItems[x].Name + "                                         " + receiptItems[x].Price;
			//	}
			//	else
			//	{
			//		itemsBox.Text += "\n" + receiptItems[x].Name + "                                         " + receiptItems[x].Price;
			//	}

			//}
			//itemsBox.Text(lI.Name, lI.Price);

			
		}

		void submitReceipt(object sender, System.EventArgs e)
		{
			
		}
	}
}
