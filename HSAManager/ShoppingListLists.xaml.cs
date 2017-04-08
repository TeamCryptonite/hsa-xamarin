using System.Collections.ObjectModel;
using HsaServiceDtos;
using HSAManager.Helpers.BizzaroHelpers;
using Xamarin.Forms;
using System;

namespace HSAManager
{
	public partial class ShoppingListLists : ContentPage
	{
		private readonly BizzaroClient client = new BizzaroClient();

		private readonly ObservableCollection<ShoppingListDto> shoppingListCollection =
			new ObservableCollection<ShoppingListDto>();

		private Paginator<ShoppingListDto> shoppingListPaginator;
		private bool loaded = true;
		public ShoppingListLists()
		{
			InitializeComponent();

			ShoppingListListView.ItemsSource = shoppingListCollection;

			// Get paginator of shopping lists
			shoppingListPaginator = client.ShoppingLists.GetListOfShoppingLists();
			// Get first set of shopping lists
			GetNextPageOfShoppingLists();

			//Set up loading next page of shopping lists
			ShoppingListListView.ItemAppearing += (sender, e) =>
			{
				if (shoppingListCollection.Count == 0)
					return;
				if (((ShoppingListDto)e.Item).ShoppingListId ==
					shoppingListCollection[shoppingListCollection.Count - 1].ShoppingListId)
					GetNextPageOfShoppingLists();
			};
		}

		private async void GetNextPageOfShoppingLists()
		{
			ShoppingListListView.BeginRefresh();
			foreach (var shoppingList in await shoppingListPaginator.Next())
				shoppingListCollection.Add(shoppingList);
			ShoppingListListView.EndRefresh();
		}

		private async void ShoppingListListView_OnItemSelected(object sender, SelectedItemChangedEventArgs e)
		{
			if (e.SelectedItem == null)
				return;
			((ListView)sender).SelectedItem = null;
			await Navigation.PushAsync(new ShoppingListView((ShoppingListDto)e.SelectedItem));
		}

		public async void OnDelete(object sender, EventArgs e)
		{
			var client = new BizzaroClient();
			var mi = (MenuItem)sender;
			var shoppingList = mi.CommandParameter as ShoppingListDto;
			shoppingListCollection.Remove(shoppingList);
			await client.ShoppingLists.DeleteShoppingList(shoppingList.ShoppingListId);

		}

		private async void Entry_Unfocused(object sender, FocusEventArgs e)
		{
			var client = new BizzaroClient();
			var entry = (Xamarin.Forms.Entry)sender;
			var bindingContext = (Xamarin.Forms.BindableObject)entry.Parent;
			var shoppingList = bindingContext.BindingContext as ShoppingListDto;
			shoppingList.Name = entry.Text;
			await client.ShoppingLists.UpdateShoppingList(shoppingList.ShoppingListId, shoppingList);
		}
	

		protected override void OnAppearing()
		{
			if (!loaded){
				var client = new BizzaroClient();
				shoppingListPaginator = client.ShoppingLists.GetListOfShoppingLists();
			}
			loaded = false;
		}
	}
}