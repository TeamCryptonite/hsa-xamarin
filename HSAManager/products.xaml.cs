using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using HsaServiceDtos;
using HSAManager.Helpers.BizzaroHelpers;
using Plugin.Geolocator;
using Xamarin.Forms;

namespace HSAManager
{
    public partial class products : ContentPage
    {
        private const string DefaultSearchTerm = "PRODUCT";
        private readonly Dictionary<string, Paginator<ProductDto>> cachedPaginators;
        private readonly Dictionary<string, ObservableCollection<ProductDto>> cachedSearchResults;
        private readonly BizzaroClient client;

        public products()
        {
            InitializeComponent();

            cachedSearchResults = new Dictionary<string, ObservableCollection<ProductDto>>();
            cachedSearchResults[DefaultSearchTerm] = new ObservableCollection<ProductDto>();

            cachedPaginators = new Dictionary<string, Paginator<ProductDto>>();

            // Set listView settings
            listView.ItemsSource = cachedSearchResults[DefaultSearchTerm];

            listView.ItemAppearing += (sender, e) =>
            {
                string searchTerm;
                if (string.IsNullOrWhiteSpace(productSearch.Text))
                    searchTerm = DefaultSearchTerm;
                else
                    searchTerm = productSearch.Text;
                if (cachedSearchResults[searchTerm].Count == 0)
                    return;
                if (((ProductDto) e.Item).ProductId ==
                    cachedSearchResults[searchTerm][cachedSearchResults[searchTerm].Count - 1].ProductId)
                    AddNewProducts(searchTerm);
            };

            if (Application.Current.Properties.ContainsKey("authKey"))
            {
                //string authKey = Application.Current.Properties["authKey"].ToString();
                client = new BizzaroClient();
                Debug.WriteLine("Test");
                cachedPaginators[DefaultSearchTerm] = client.Products.GetListOfProducts();
                Debug.WriteLine("Test");
                //ProductsPaginator = client.Products.GetListOfProducts();
                AddNewProducts(DefaultSearchTerm);
            }
        }

        protected override void OnAppearing()
        {
        }

        protected void searchChanged(object sender, TextChangedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(e.NewTextValue))
            {
                listView.ItemsSource = cachedSearchResults[DefaultSearchTerm];
                return;
            }
            if (!cachedSearchResults.ContainsKey(e.NewTextValue))
                cachedSearchResults[e.NewTextValue] = new ObservableCollection<ProductDto>();
            if (!cachedPaginators.ContainsKey(e.NewTextValue))
                cachedPaginators[e.NewTextValue] = client.Products.GetListOfProducts(e.NewTextValue);

            listView.ItemsSource = cachedSearchResults[e.NewTextValue];

            if (cachedSearchResults[e.NewTextValue].Count < 1)
                AddNewProducts(e.NewTextValue);
        }

        protected async void AddNewProducts(string queryString)
        {
            Debug.WriteLine("Addding New Products");
            try
            {
                var productsToAdd = await cachedPaginators[queryString].Next();
                foreach (var product in productsToAdd)
                    cachedSearchResults[queryString].Add(product);
            }
            catch (Exception ex)
            {
                await DisplayAlert("Alert", ex.Message, "OK");
            }
        }

        private async void ListView_OnItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            try
            {
                var locator = CrossGeolocator.Current;
                locator.DesiredAccuracy = 50;

                if (!locator.IsGeolocationEnabled)
                {
                    await DisplayAlert("Location Error", "Location Permissions Not Set", "OK");
                    return;
                }

                var position = await locator.GetPositionAsync(10000);
                await Navigation.PushAsync(new productsInStores(((ProductDto) e.SelectedItem).ProductId,
                    position.Latitude, position.Longitude));
            }
            catch (Exception ex)
            {
                await DisplayAlert("Location Error", ex.Message, "OK");
            }
        }
    }
}