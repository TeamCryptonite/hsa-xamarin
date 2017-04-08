using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
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
        private readonly BizzaroClient client;
		private bool hasLoaded;
        //public ObservableCollection<ReceiptDto> receipts;
        //private Paginator<ReceiptDto> ReceiptsPaginator;

        public receiptVault()
        {
            InitializeComponent();
			hasLoaded = false;
			System.Diagnostics.Debug.WriteLine("Constructor");
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
                    searchTerm = DefaultSearchTerm;
                else
                    searchTerm = receiptVaultSearch.Text;
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

		protected override void OnAppearing()
		{
			/*if (hasLoaded)
			{
				var client = new BizzaroClient();
				cachedPaginators[DefaultSearchTerm] = client.Receipts.GetListOfReceipts();
			System.Diagnostics.Debug.WriteLine("OnAppearing INSIDE IF");
			}
			hasLoaded = true;
			System.Diagnostics.Debug.WriteLine("OnAppearing");*/
        }

        protected void searchChanged(object sender, TextChangedEventArgs e)
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
            listView.BeginRefresh();
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
            listView.EndRefresh();
        }

        //public async void Handle_Tapped(object sender, System.EventArgs e)
        //{
        //	await Navigation.PushAsync(new data());

        //public async void Handle_Tapped(object sender, System.EventArgs e)
        private async void ListView_OnItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            var selectedReceiptDto = e.SelectedItem as ReceiptDto;
            if (selectedReceiptDto == null)
            {
                await DisplayAlert("Receipt Alert", "Selected Receipt does not have valid data.", "OK");
                return;
            }

            if (!selectedReceiptDto.Provisional && !selectedReceiptDto.WaitingForOcr)
            {
                await Navigation.PushAsync(new ReceiptView((ReceiptDto) e.SelectedItem));
            }
            else if (selectedReceiptDto.Provisional)
            {
                await Navigation.PushAsync(new ReceiptEditing((ReceiptDto) e.SelectedItem));
            }
            else if (selectedReceiptDto.WaitingForOcr)
            {
                await DisplayAlert("OCR Running",
                    "Cannot open selected receipt because the server is still running OCR on it! Check back later!",
                    "OK");
            }
            else
            {
                await DisplayAlert("Receipt Alert",
                    "Selected Receipt has invalid flags. Check with system administrator.", "OK");
            }
        }
		public async void OnDelete(object sender, EventArgs e)
		{
			//var client = new BizzaroClient();
			//var mi = (MenuItem)sender;
			//var shoppingListItem = mi.CommandParameter as ShoppingListItemDto;
			//shoppingListItems.Remove(shoppingListItem);
			//await client.ShoppingLists.DeleteShoppingListItem(shoppingList.ShoppingListId, shoppingListItem.ShoppingListItemId);

		}
    }
}