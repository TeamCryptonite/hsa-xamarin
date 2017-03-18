using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using HsaServiceDtos;
using Xamarin.Forms;

//using HsaServiceDtos;

namespace HSAManager
{
    public partial class receiptVault : ContentPage
    {
        private BizzaroClient client;
        private CancellationTokenSource cts;
        public ObservableCollection<ReceiptDto> receipts;
        private Paginator<ReceiptDto> ReceiptsPaginator;

        public receiptVault()
        {
            InitializeComponent();
            receipts = new ObservableCollection<ReceiptDto>();
            listView.ItemsSource = receipts;

            listView.ItemAppearing += async (sender, e) =>
            {
                if (receipts.Count == 0)
                    return;
                cts = new CancellationTokenSource();
                if (((ReceiptDto) e.Item).ReceiptId == receipts[receipts.Count - 1].ReceiptId)
                    try
                    {
                        await AddNewReceipts(cts.Token);
                    }
                    catch
                    {
                    }
            };
        }

        protected override async void OnAppearing()
        {
            if (Application.Current.Properties.ContainsKey("authKey"))
            {
                //string authKey = Application.Current.Properties["authKey"].ToString();
                client = new BizzaroClient();

                ReceiptsPaginator = client.Receipts.GetListOfReceipts();
                cts = new CancellationTokenSource();
                try
                {
                    await AddNewReceipts(cts.Token);
                }
                catch
                {
                }
            }
        }

        protected async void searchChanged(object sender, TextChangedEventArgs e)
        {
            cts.Cancel();
            receipts.Clear();
            ReceiptsPaginator = client.Receipts.GetListOfReceipts(e.NewTextValue);
            cts = new CancellationTokenSource();
            try
            {
                await AddNewReceipts(cts.Token);
            }
            catch
            {
            }
        }

        protected async Task AddNewReceipts(CancellationToken cToken)
        {
            Debug.WriteLine("Addding New Receipts");
            var receiptsToAdd = await ReceiptsPaginator.Next();
            cToken.ThrowIfCancellationRequested();
            foreach (var receipt in receiptsToAdd)
            {
                cToken.ThrowIfCancellationRequested();
                receipts.Add(receipt);
            }
        }

        //public async void Handle_Tapped(object sender, System.EventArgs e)
        //{
        //	await Navigation.PushAsync(new data());

        //}
    }
}