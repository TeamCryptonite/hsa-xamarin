using System;
using System.Collections.Generic;

using Xamarin.Forms;

namespace HSAManager
{
	public partial class ReceiptEntry : ContentPage
	{

		public class lineItem
		{
			public lineItem(string name, double price)
			{
				this.Name = name;
				this.Price = price;
			}

			public string Name { private set; get;}
			public double Price { private set; get; }
		}


		List<lineItem> receiptItems = new List<lineItem>();


		public ReceiptEntry()
		{
			InitializeComponent();
		}

		void getBlob(object sender, System.EventArgs e)
		{
			// get image file type and retrieve blob
		}

		void addItems(object sender, System.EventArgs e)
		{
			lineItem lI = new lineItem(item.Text, Double.Parse(price.Text));

			System.Diagnostics.Debug.WriteLine(lI.Name + " " + lI.Price);
			receiptItems.Add(lI);
			for (int x = 0; x <= receiptItems.Count-1; x++) 
			{
				if (x < 1)
				{
					itemsBox.Text= receiptItems[x].Name + "                                         " + receiptItems[x].Price;
				}
				else
				{
					itemsBox.Text += "\n" + receiptItems[x].Name + "                                         " + receiptItems[x].Price;
				}

			}
			//itemsBox.Text(lI.Name, lI.Price);
			
		}

		void submitReceipt(object sender, System.EventArgs e)
		{
			
		}
	}
}
