using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Runtime.CompilerServices;
using HsaServiceDtos;
using Xamarin.Forms;

namespace HSAManager
{
    public partial class ReceiptEditing : ContentPage
    {
        //private readonly ObservableCollection<LineItemDto> lineItemNames = new ObservableCollection<LineItemDto>();

        private BizzaroClient client = new BizzaroClient();
        private ReceiptDto receipt;

        private readonly ObservableCollection<StoreDto> storeSuggestionsCollection =
            new ObservableCollection<StoreDto>();

        public ReceiptEditing(ReceiptDto receipt)
        {
            try
            {
                InitializeComponent();
                this.receipt = receipt;
                if (!string.IsNullOrWhiteSpace(receipt.ImageUrl))
                {
                    try
                    {
                        receiptImage.Source = ImageSource.FromUri(new Uri(receipt.ImageUrl));
                    }
                    catch (Exception ex)
                    {
                        receiptImage.Source = null;
                    }
                }
                if (receipt.LineItems == null)
                    receipt.LineItems = new List<LineItemDto>();
                LineItemListView.ItemsSource = receipt.LineItems;
                StoreSuggestionsListView.ItemsSource = storeSuggestionsCollection;

                DatePicker.MinimumDate = new System.DateTime(2000, 1, 1);
                DatePicker.MaximumDate = DateTime.Now;
                DatePicker.Date = receipt.DateTime ?? DateTime.Now;

                store.Text = receipt.Store?.Name;
            }
            catch (Exception ex)
            {
                DisplayAlert("Fail", ex.Message, "OK");
                Navigation.RemovePage(this);
            }
        }

        private void addItem(object sender, EventArgs e)
        {
            LineItemDto lineItem = new LineItemDto();
            lineItem.Product = new ProductDto();
            if (item.Text != "")
            {
                lineItem.Product.Name = item.Text;
            }
            else
            {
                DisplayAlert("Alert", "You must enter a product name", "OK");
                return;
            }
            if (price.Text != "")
            {
                try
                {
                    lineItem.Price = Convert.ToDecimal(price.Text);
                }
                catch
                {
                    DisplayAlert("Alert", "You must enter a number for the price", "OK");
                }
            }
            else
            {
                DisplayAlert("Alert", "You must enter a number for the price", "OK");
                return;
            }
            item.Text = "";
            price.Text = "";
            receipt.LineItems.Add(lineItem);
        }

        private async void submitReceipt(object sender, EventArgs e)
        {
            receipt.DateTime = DatePicker.Date;

            if (store.Text != "")
            {
                if (receipt.Store == null)
                {
                    receipt.Store = new StoreDto();
                    receipt.Store.Name = store.Text;
                }
                if (Application.Current.Properties.ContainsKey("authKey"))
                {
                    var authKey = Application.Current.Properties["authKey"].ToString();
                    var client = new BizzaroClient();
                    try
                    {
                        await client.Receipts.UpdateReceipt(receipt.ReceiptId, receipt);
                        var lineItemsToRemove = new List<LineItemDto>();
                        var lineItemsToAdd = new List<LineItemDto>();
                        foreach (var lineItem in receipt.LineItems)
                        {
                            if (lineItem.LineItemId > 0)
                                await client.Receipts.UpdateReceiptListItem(receipt.ReceiptId, lineItem);

                            else
                            {
                                lineItemsToAdd.Add(await client.Receipts.AddReceiptListItem(receipt.ReceiptId, lineItem));
                                lineItemsToRemove.Remove(lineItem);
                            }
                        }
                        foreach(var lineItem in lineItemsToRemove)
                            receipt.LineItems.Remove(lineItem);
                        foreach(var lineItem in lineItemsToAdd)
                            receipt.LineItems.Add(lineItem);

                        await DisplayAlert("Success", "Receipt has been submitted!", "OK");
                        receipt.Provisional = false;
                        await Navigation.PopAsync(true);
                    }
                    catch (Exception ex)
                    {
                        await DisplayAlert("Alert", ex.Message, "OK");
                    }
                }
            }
            else
            {
                await DisplayAlert("Alert", "You must enter a store name", "OK");
            }
        }

        private async void Store_OnTextChanged(object sender, TextChangedEventArgs e)
        {

            var stores = await client.Stores.GetListOfStores(e.NewTextValue).Next();
            storeSuggestionsCollection.Clear();
            foreach (var store in stores)
            {
                storeSuggestionsCollection.Add(store);
            }
        }

        private void Store_OnUnfocused(object sender, FocusEventArgs e)
        {
            if (!store.IsFocused || !StoreSuggestionsListView.IsFocused)
                StoreSuggestionsListView.IsVisible = false;
        }

        private void Store_OnFocused(object sender, FocusEventArgs e)
        {

            StoreSuggestionsListView.IsVisible = true;
        }

        private void StoreSuggestionsListView_OnItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            if (e.SelectedItem == null)
                return;
            receipt.Store = (StoreDto)e.SelectedItem;
            store.Text = receipt.Store.Name;
            StoreSuggestionsListView.SelectedItem = null;
        }
    }
}