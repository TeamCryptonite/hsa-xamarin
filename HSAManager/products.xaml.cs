using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Threading.Tasks;
using HsaServiceDtos;
using System.Threading;
using Xamarin.Forms;

namespace HSAManager
{
	public partial class products : ContentPage
	{
		private BizzaroClient client;
        private CancellationTokenSource cts;
        public ObservableCollection<ProductDto> returnedProducts;
		private Paginator<ProductDto> ProductsPaginator;

		public products()
		{
			InitializeComponent();
			returnedProducts = new ObservableCollection<ProductDto>();
			listView.ItemsSource = returnedProducts;

			listView.ItemAppearing += async (sender, e) =>
			{
				if (returnedProducts.Count == 0) return;
				cts = new CancellationTokenSource();
				if (((ProductDto)e.Item).ProductId == returnedProducts[returnedProducts.Count - 1].ProductId)
					try
					{
						await AddNewProducts(cts.Token);
					}catch{
                    
					}
			};
		}

		protected override async void OnAppearing()
		{
			client = new BizzaroClient();
			ProductsPaginator = client.Products.GetListOfProducts();
			cts = new CancellationTokenSource();
			try
			{
				await AddNewProducts(cts.Token);
			}
			catch
			{
			}
		}

		protected async void searchChanged(object sender, TextChangedEventArgs e)
        {
            cts.Cancel();
			returnedProducts.Clear();
			ProductsPaginator = client.Products.GetListOfProducts(e.NewTextValue);
            cts = new CancellationTokenSource();
            try
            {
                await AddNewProducts(cts.Token);
            }
            catch
            {
            }
        }

        protected async Task AddNewProducts(CancellationToken cToken)
        {
            Debug.WriteLine("Addding New Products");
			var productsToAdd = await ProductsPaginator.Next();
            cToken.ThrowIfCancellationRequested();
			foreach (var product in productsToAdd)
            {
                cToken.ThrowIfCancellationRequested();
				returnedProducts.Add(product);
            }
        }

		public void Handle_ItemSelected(object sender, SelectedItemChangedEventArgs e)
		{
			//System.Diagnostics.Debug.WriteLine();

			var p = e.SelectedItem as ProductDto;
			int pid = p.ProductId;
			Debug.WriteLine(pid);
		}

	}
}
