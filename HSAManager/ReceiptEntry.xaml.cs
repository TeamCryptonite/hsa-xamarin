using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;
using HsaServiceDtos;
using Xamarin.Forms;
using Plugin.Media;
using Plugin.Media.Abstractions;
using System.Collections.ObjectModel;

namespace HSAManager
{
    public partial class ReceiptEntry : ContentPage
    {
        private readonly ObservableCollection<LineItemDto> lineItemNames = new ObservableCollection<LineItemDto>();

        private readonly ObservableCollection<StoreDto> storeSuggestionsCollection =
            new ObservableCollection<StoreDto>();

        private readonly ReceiptDto thisReceipt = new ReceiptDto();

        private readonly BizzaroClient client = new BizzaroClient();

        public LineItemDto lineItemDto = new LineItemDto();

		private MediaFile file;


        public ReceiptEntry()
        {
            InitializeComponent();
            addItemInfo.ItemsSource = lineItemNames;
            StoreSuggestionsListView.ItemsSource = storeSuggestionsCollection;

			DatePicker.MinimumDate =  new System.DateTime(2000, 1, 1);
			DatePicker.MaximumDate = DateTime.Now;

            changeStoreSuggestionsCollection("");
        }


        private async void addImage(object sender, EventArgs e)
        {
           await CrossMedia.Current.Initialize();
			if (!CrossMedia.Current.IsPickPhotoSupported)
			{
				await DisplayAlert("Oops", "Pick photo is not supported !", "OK");
				return;
			}
			file = await CrossMedia.Current.PickPhotoAsync();
			imageName.Source = ImageSource.FromFile(file.Path);

			if (file == null)
				return;

        }

        private void addItem(object sender, EventArgs e)
        {
            lineItemDto.Product = new ProductDto();
            if (item.Text != "")
            {
                lineItemDto.Product.Name = item.Text;
				addItemInfo.IsVisible = true;
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
                    lineItemDto.Price = Convert.ToDecimal(price.Text);
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
            thisReceipt.LineItems.Add(lineItemDto);
            lineItemNames.Add(lineItemDto);
            lineItemDto = null;
            lineItemDto = new LineItemDto();
        }

		public void OnDelete(object sender, EventArgs e)
		{
			var mi = (MenuItem)sender;
			var lineItem = mi.CommandParameter as LineItemDto;
			thisReceipt.LineItems.Remove(lineItem);
			lineItemNames.Remove(lineItem);
		}

        private async void submitReceipt(object sender, EventArgs e)
        {
            thisReceipt.DateTime = DatePicker.Date;

            if (StoreEntry.Text != "")
            {
                if (thisReceipt.Store == null)
                {
                    thisReceipt.Store = new StoreDto();
                    thisReceipt.Store.Name = StoreEntry.Text;
                }
                foreach (LineItemDto lineItem in lineItemNames)
                {
                    if (lineItem.Product.Name == "")
                    {
                        await DisplayAlert("Alert", "Please make sure you have a name for every item.", "OK");
                        return;
                    }
                }
                if (Application.Current.Properties.ContainsKey("authKey"))
                {
                    var authKey = Application.Current.Properties["authKey"].ToString();
                    var client = new BizzaroClient();
                    try
                    {
                        var tester = await client.Receipts.PostNewReceipt(thisReceipt);
						if (file != null)
						{
							await client.Receipts.UploadReceiptImage(tester.ReceiptId, file.GetStream());
							file.Dispose();
						}
                        await DisplayAlert("Success", "Receipt has been submitted!", "OK");
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
            changeStoreSuggestionsCollection(e.NewTextValue);
        }

        private void Store_OnUnfocused(object sender, FocusEventArgs e)
        {
            if (!StoreEntry.IsFocused || !StoreSuggestionsListView.IsFocused)
                StoreSuggestionsListView.IsVisible = false;
        }

        private void Store_OnFocused(object sender, FocusEventArgs e)
        {
            StoreSuggestionsListView.IsVisible = true;
        }

        private void StoreSuggestionsListView_OnItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            thisReceipt.Store = (StoreDto) e.SelectedItem;
            StoreEntry.Text = thisReceipt.Store.Name;
        }

        private async void changeStoreSuggestionsCollection(string searchQuery)
        {
            var stores = await client.Stores.GetListOfStores(searchQuery).Next();
            storeSuggestionsCollection.Clear();
            foreach (var store in stores)
                storeSuggestionsCollection.Add(store);
        }

        private void Name_Entry_Unfocused(object sender, FocusEventArgs e)
        {
            var entry = (Xamarin.Forms.Entry)sender;
            var name = entry.Text;
            var bindingContext = (Xamarin.Forms.BindableObject)entry.Parent.Parent;
            var lineItem = bindingContext.BindingContext as LineItemDto;
            lineItem.Product.Name = name;
        }

        private void Price_Entry_Unfocused(object sender, FocusEventArgs e)
        {
            var entry = (Xamarin.Forms.Entry)sender;
            var price = entry.Text;
            var bindingContext = (Xamarin.Forms.BindableObject)entry.Parent.Parent;
            var lineItem = bindingContext.BindingContext as LineItemDto;
            try
            {
                if(price != "")
                {
                    lineItem.Price = Convert.ToDecimal(price);
                } else
                {
                    lineItem.Price = 0;
                }
            } catch (Exception ex)
            {
                entry.Text = "";
                DisplayAlert("Oops", "Please enter a valid price. Ex: 1.00, 5.35, 2, etc.", "OK");
            }
        }
    }
}