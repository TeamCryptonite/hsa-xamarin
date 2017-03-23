using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Threading.Tasks;
using HsaServiceDtos;
using System.Threading;
using Xamarin.Forms;
using HSAManager.Helpers.BizzaroHelpers;

namespace HSAManager
{
	public partial class productsInStores : ContentPage
	{
		private BizzaroClient client;
		private CancellationTokenSource cts;
		public ObservableCollection<store> returnedStores;
		private List<StoreDto> storeDtos;
		private Paginator<StoreDto> StoresPaginator;

		public class store
		{
			public string Name { get; set; }
			public double Distance { get; set; }
			public int StoreId { get; set; }

			public store(string name, double distance,int storeId)
			{
				Name = name;
				Distance = distance;
				StoreId = storeId;
			}
		}

		public productsInStores(int storeId )
		{
			InitializeComponent();
			returnedStores = new ObservableCollection<store>();
			listView.ItemsSource = returnedStores;

			listView.ItemAppearing += async (sender, e) =>
			{
				if (returnedStores.Count == 0) return;
				if (((store)e.Item).StoreId == returnedStores[returnedStores.Count - 1].StoreId)
					try
					{
						await AddNewStores();
					}
					catch
					{

					}
			};
		}

		protected override async void OnAppearing()
		{
			client = new BizzaroClient();
			StoresPaginator = client.Stores.GetListOfStores();
			try
			{
				await AddNewStores();
			}
			catch
			{
			}
		}

		protected async Task AddNewStores()
		{
			Debug.WriteLine("Addding New Products");
			var storesToAddDto = await StoresPaginator.Next();
			foreach (var storeDto in storesToAddDto)
			{
				var storeCopy = new store(storeDto.Name, 1.1, storeDto.StoreId);
				returnedStores.Add(storeCopy);
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
