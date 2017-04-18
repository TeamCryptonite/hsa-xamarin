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
			receiptImage.Source = ImageSource.FromUri(new Uri(receipt.ImageUrl));
			//receiptImage.Source = "https://memorycrystal.blob.core.windows.net/userreceipts/781f540b13bd49779d35c8d9ae37ec2frec.jpg";
            store.Text = receipt.Store != null ? receipt.Store?.Name : "No Store";
            date.Text = receipt.DateTime != null
                ? $"{receipt.DateTime:MMMM d, yyyy}"
                : "No Date For This Receipt";
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
			await client.Receipts.DeleteReceiptListItem(receipt.ReceiptId,lineItem.LineItemId);
		}

        private async void Name_Entry_Unfocused(object sender, FocusEventArgs e)
        {
            var client = new BizzaroClient();
            var entry = (Xamarin.Forms.Entry)sender;
            var name = entry.Text;
            var bindingContext = (Xamarin.Forms.BindableObject)entry.Parent.Parent;
            var lineItem = bindingContext.BindingContext as LineItemDto;
            lineItem.Product.Name = name;
			lineItem.Product.ProductId = 0;
            await client.Receipts.UpdateReceiptListItem(receipt.ReceiptId, lineItem);
        }

		private async void Price_Entry_Unfocused(object sender, FocusEventArgs e)
		{
			var client = new BizzaroClient();
			var entry = (Xamarin.Forms.Entry)sender;
			var price = entry.Text;
			var bindingContext = (Xamarin.Forms.BindableObject)entry.Parent.Parent;
			var lineItem = bindingContext.BindingContext as LineItemDto;
			try
			{
				if (price != "")
				{
					lineItem.Price = Convert.ToDecimal(price);
					await client.Receipts.UpdateReceiptListItem(receipt.ReceiptId, lineItem);
				}
				else
				{
					lineItem.Price = 0;
				}
			}
			catch (Exception ex)
			{
				entry.Text = "";
				await DisplayAlert("Oops", "Please enter a valid price. Ex: 1.00, 5.35, 2, etc.", "OK");
			}
		}

		private async void Store_Entry_Unfocused(object sender, FocusEventArgs e)
		{
			var client = new BizzaroClient();
			var entry = (Xamarin.Forms.Entry)sender;
			receipt.Store.Name = entry.Text;
			receipt.Store.StoreId = 0;

			await client.Receipts.UpdateReceipt(receipt.ReceiptId, receipt);
		}
    }
}