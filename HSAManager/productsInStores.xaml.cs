using Plugin.Geolocator;
using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Threading;
using HsaServiceDtos;
using HSAManager.Helpers.BizzaroHelpers;
using Xamarin.Forms;


namespace HSAManager
{
	public partial class productsInStores : ContentPage
	{
		private BizzaroClient client;
		private CancellationTokenSource cts;
		public ObservableCollection<StoreDto> StoresCollection;
		private Paginator<StoreDto> StoresPaginator;
	    private int storeId;
		private double latitude;
		private double longtitude;


		public productsInStores(int storeId, double latitude, double longitude)
		{
			InitializeComponent();

		    this.storeId = storeId;
			StoresCollection = new ObservableCollection<StoreDto>();
			listView.ItemsSource = StoresCollection;

            listView.ItemAppearing += (sender, e) =>
            {
                if (StoresCollection.Count == 0) return;
                if (((StoreDto) e.Item).StoreId == StoresCollection[StoresCollection.Count - 1].StoreId)
                    try
                    {
                        AddNewStores();
                    }
                    catch (Exception ex)
                    {
                        DisplayAlert("Exception", ex.Message, "OK");
                    }
            };

            client = new BizzaroClient();

			// Using a default location for now. Will need to pull in user location later.
			StoresPaginator = client.Stores.GetListOfStores(productId:storeId, userLocation:new LocationDto()
            {
				Latitude = latitude,
				Longitude = longitude
            });
            try
            {
                AddNewStores();
            }
            catch (Exception ex)
            {
                DisplayAlert("Exception", ex.Message, "OK");
            }

            // Set up listview
            var template = new DataTemplate(() =>
            {
                var stackLayout = new StackLayout
                {
                    Orientation = StackOrientation.Horizontal
                };


                var nameLabel = new Label
                {
                    HorizontalOptions = LayoutOptions.StartAndExpand
                };
                nameLabel.SetBinding(Label.TextProperty, "Name");

                var distanceLabel = new Label
                {
                    HorizontalOptions = LayoutOptions.EndAndExpand
                };
                distanceLabel.SetBinding(Label.TextProperty, "DistanceToUser");
                distanceLabel.BindingContextChanged +=
                    (sender, e) => { distanceLabel.Text = $"{double.Parse(distanceLabel.Text):0.##} miles"; };

                stackLayout.Children.Add(nameLabel);
                stackLayout.Children.Add(distanceLabel);

                return new ViewCell {View = stackLayout};
            });

            listView.ItemTemplate = template;
        }

		protected async void AddNewStores()
		{
			Debug.WriteLine("Addding New Products");
			var storesToAddDto = await StoresPaginator.Next();
			foreach (var storeDto in storesToAddDto)
			{
				StoresCollection.Add(storeDto);
			}
            if (StoresCollection.Count == 0)
            {
                await DisplayAlert("Uh, Oh!", "No Stores with this product were found in your area...", "OK"); //add try increasing your radius when we allow that to be a thing.
                await Navigation.PopAsync(true);
            }
        }

        public void Handle_ItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            //var p = e.SelectedItem as ProductDto;
            //int pid = p.ProductId;
            //Navigation.PushAsync(new productsInStores(pid));
        }
    }
}