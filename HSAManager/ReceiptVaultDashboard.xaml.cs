using System;
using Xamarin.Forms;

namespace HSAManager
{
    public partial class ReceiptVaultDashboard : ContentPage
    {
        public ReceiptVaultDashboard()
        {
            InitializeComponent();
        }

        private void Handle_Receipt_Vault(object sender, EventArgs e)
        {
            Navigation.PushAsync(new receiptVault());
        }

        private void Handle_OCR_Entry(object sender, EventArgs e)
        {
            Navigation.PushAsync(new OCREntry());
        }

        private void Handle_Manual_Entry(object sender, EventArgs e)
        {
            Navigation.PushAsync(new ReceiptEntry());
        }
    }
}