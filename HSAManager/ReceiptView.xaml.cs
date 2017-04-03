using HsaServiceDtos;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace HSAManager
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ReceiptView : ContentPage
    {
        private readonly ReceiptDto receipt;

        public ReceiptView(ReceiptDto receiptInput)
        {
            InitializeComponent();
            receipt = receiptInput;
            store.Text = receipt.Store != null ? receipt.Store?.Name + " Receipt" : "No Store For This Receipt";
            date.Text = receipt.DateTime != null
                ? $"{receipt.DateTime:MMMM d, yyyy}"
                : "No Date For This Receipt";
            LineItemListView.ItemsSource = receipt.LineItems;
        }
    }
}