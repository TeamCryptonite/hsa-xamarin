﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
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
            StartListViewActivityIndicator(cachedSearchResults[queryString].Count < 1 ? true : false);
            //listView.BeginRefresh();
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
            //listView.EndRefresh();
            StopListViewActivityIndicator();
        }

        private async void ListView_OnItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            if (e.SelectedItem == null)
                return;

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

            listView.SelectedItem = null;
        }
		public async void OnDelete(object sender, EventArgs e)
		{
            var receiptToDelete = ((MenuItem)sender).CommandParameter as ReceiptDto;
		    if (receiptToDelete == null)
		    {
		        await DisplayAlert("Error", "Receipt is not valid!", "OK");
		        return;
		    }

            await client.Receipts.DeleteReceipt(receiptToDelete.ReceiptId);
            RefreshEverything();

		}

        private void ListView_OnRefreshing(object sender, EventArgs e)
        {
            RefreshEverything();
        }

        private async void RefreshEverything()
        {
            StartListViewActivityIndicator(true);
            cachedPaginators.Clear();
            cachedSearchResults.Clear();

            listView.ItemsSource = null;

            var searchTerm = receiptVaultSearch.Text;

            if (string.IsNullOrWhiteSpace(searchTerm))
            {
                searchTerm = DefaultSearchTerm;
                cachedSearchResults[DefaultSearchTerm] = new ObservableCollection<ReceiptDto>();
                cachedPaginators[DefaultSearchTerm] = client.Receipts.GetListOfReceipts();
                AddNewReceipts(DefaultSearchTerm);
            }
            else
            {
                if (!cachedSearchResults.ContainsKey(searchTerm))
                    cachedSearchResults[searchTerm] = new ObservableCollection<ReceiptDto>();
                if (!cachedPaginators.ContainsKey(searchTerm))
                    cachedPaginators[searchTerm] = client.Receipts.GetListOfReceipts(searchTerm);

                

                if (cachedSearchResults[searchTerm].Count < 1)
                    AddNewReceipts(searchTerm);
            }
            listView.ItemsSource = cachedSearchResults[searchTerm];
            listView.EndRefresh();
        }

        private void StartListViewActivityIndicator(bool hideListView = false)
        {
            if (hideListView)
                listView.IsVisible = false;
            ListViewActivityIndicator.IsVisible = true;
            ListViewActivityIndicator.IsRunning = true;
        }

        private void StopListViewActivityIndicator()
        {
            listView.IsVisible = true;
            ListViewActivityIndicator.IsVisible = false;
            ListViewActivityIndicator.IsRunning = false;
        }
    }
}