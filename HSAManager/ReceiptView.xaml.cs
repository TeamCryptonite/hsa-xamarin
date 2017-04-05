using HsaServiceDtos;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using System;
using System.Collections.ObjectModel;

namespace HSAManager
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ReceiptView : ContentPage
    {
        private readonly ReceiptDto receipt;

		private ObservableCollection<LineItemDto> lineItems = new ObservableCollection<LineItemDto>();

        public ReceiptView(ReceiptDto receiptInput)
        {
            InitializeComponent();
            receipt = receiptInput;
            store.Text = receipt.Store != null ? receipt.Store?.Name + " Receipt" : "No Store For This Receipt";
            date.Text = receipt.DateTime != null
                ? $"{receipt.DateTime:MMMM d, yyyy}"
                : "No Date For This Receipt";
			//lineItems = receipt.LineItems as ObservableCollection<LineItemDto>;
			foreach (LineItemDto lineItem in receipt.LineItems)
			{
				lineItems.Add(lineItem);
			}
			LineItemListView.ItemsSource = lineItems;
        }

		public async void OnDelete(object sender, EventArgs e)
		{
			var client = new BizzaroClient();
			var mi = (MenuItem)sender;
			var lineItem = mi.CommandParameter as LineItemDto;
			receipt.LineItems.Remove(lineItem);
			lineItems.Remove(lineItem);
			await client.Receipts.UpdateReceipt(receipt.ReceiptId,receipt);
		}
    }
}