using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Threading.Tasks;
using HsaServiceDtos;
using HSAManager.Helpers.BizzaroHelpers;
using Xamarin.Forms;

//using HsaServiceDtos;

namespace HSAManager
{
    public partial class receiptVault : ContentPage
    {
        private const string DefaultSearchTerm = "RECEIPT";
        private readonly Dictionary<string, ObservableCollection<ReceiptDto>> cachedSearchResults;
        private Dictionary<string, Paginator<ReceiptDto>> cachedPaginators;
        private BizzaroClient client;
        //public ObservableCollection<ReceiptDto> receipts;
        //private Paginator<ReceiptDto> ReceiptsPaginator;

        public receiptVault()
        {
            InitializeComponent();

            // Initialize class properties
            //receipts = new ObservableCollection<ReceiptDto>();
            cachedSearchResults = new Dictionary<string, ObservableCollection<ReceiptDto>>();
            cachedSearchResults[DefaultSearchTerm] = new ObservableCollection<ReceiptDto>();

            cachedPaginators = new Dictionary<string, Paginator<ReceiptDto>>();
            
            // Set listView settings
            listView.ItemsSource = cachedSearchResults[DefaultSearchTerm];

            listView.ItemAppearing += (sender, e) =>
            {
                string searchTerm;
                if (string.IsNullOrWhiteSpace(receiptVaultSearch.Text))
                {
                    searchTerm = DefaultSearchTerm;
                }
                else
                {
                    searchTerm = receiptVaultSearch.Text;
                }
                if (cachedSearchResults[searchTerm].Count == 0)
                    return;
                if (((ReceiptDto) e.Item).ReceiptId ==
                    cachedSearchResults[searchTerm][cachedSearchResults[searchTerm].Count - 1].ReceiptId)
                    AddNewReceipts(searchTerm);
            };

            if (Application.Current.Properties.ContainsKey("authKey"))
            {
                //string authKey = Application.Current.Properties["authKey"].ToString();
                client = new BizzaroClient();
                Debug.WriteLine("Test");
                cachedPaginators[DefaultSearchTerm] = client.Receipts.GetListOfReceipts();
                Debug.WriteLine("Test");
                //ReceiptsPaginator = client.Receipts.GetListOfReceipts();
                AddNewReceipts(DefaultSearchTerm);
            }
        }

        protected async void searchChanged(object sender, TextChangedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(e.NewTextValue))
            {
                listView.ItemsSource = cachedSearchResults[DefaultSearchTerm];
                return;
            }
            if (!cachedSearchResults.ContainsKey(e.NewTextValue))
                cachedSearchResults[e.NewTextValue] = new ObservableCollection<ReceiptDto>();
            if (!cachedPaginators.ContainsKey(e.NewTextValue))
                cachedPaginators[e.NewTextValue] = client.Receipts.GetListOfReceipts(e.NewTextValue);

            listView.ItemsSource = cachedSearchResults[e.NewTextValue];

            if (cachedSearchResults[e.NewTextValue].Count < 1)
                AddNewReceipts(e.NewTextValue);
        }

        protected async void AddNewReceipts(string queryString)
        {
            Debug.WriteLine("Addding New Receipts");
            try
            {
                var receiptsToAdd = await cachedPaginators[queryString].Next();
                foreach (var receipt in receiptsToAdd)
                    cachedSearchResults[queryString].Add(receipt);
            }
            catch (Exception ex)
            {
                await DisplayAlert("Alert", ex.Message, "OK");
            }
        }

        //}
        //	await Navigation.PushAsync(new data());
        //{

        //public async void Handle_Tapped(object sender, System.EventArgs e)
        private void ListView_OnItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            Navigation.PushAsync(new ReceiptView((ReceiptDto)e.SelectedItem));
        }
    }
}