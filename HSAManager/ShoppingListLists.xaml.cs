using System;
using System.Collections.ObjectModel;
using HsaServiceDtos;
using HSAManager.Helpers.BizzaroHelpers;
using Xamarin.Forms;

namespace HSAManager
{
    public partial class ShoppingListLists : ContentPage
    {
        private readonly BizzaroClient client = new BizzaroClient();

        private readonly ObservableCollection<ShoppingListDto> shoppingListCollection =
            new ObservableCollection<ShoppingListDto>();

        private bool loaded = true;

        private readonly Paginator<ShoppingListDto> shoppingListPaginator;

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
                if (((ShoppingListDto) e.Item).ShoppingListId ==
                    shoppingListCollection[shoppingListCollection.Count - 1].ShoppingListId)
                    GetNextPageOfShoppingLists();
            };
        }

        private async void GetNextPageOfShoppingLists()
        {
            ShoppingListListView.BeginRefresh();
            try { 
            foreach (var shoppingList in await shoppingListPaginator.Next())
                shoppingListCollection.Add(shoppingList);
            }
            catch (Exception ex)
            {
                await DisplayAlert("Error", ex.Message, "OK");
            }
            ShoppingListListView.EndRefresh();
        }

        private async void ShoppingListListView_OnItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            if (e.SelectedItem == null)
                return;
            ((ListView) sender).SelectedItem = null;
            await Navigation.PushAsync(new ShoppingListView((ShoppingListDto) e.SelectedItem));
        }

        public async void OnDelete(object sender, EventArgs e)
        {
            var mi = (MenuItem) sender;
            var shoppingList = mi.CommandParameter as ShoppingListDto;
            shoppingListCollection.Remove(shoppingList);
            try
            {
                await client.ShoppingLists.DeleteShoppingList(shoppingList.ShoppingListId);
            }
            catch (Exception ex)
            {
                await DisplayAlert("Error", ex.Message, "OK");
            }
        }

        private async void Entry_Unfocused(object sender, FocusEventArgs e)
        {
            var entry = (Entry) sender;
            var bindingContext = (BindableObject) entry.Parent;
            var shoppingList = bindingContext.BindingContext as ShoppingListDto;

            if (shoppingList == null)
                return;

            shoppingList.Name = entry.Text;
            try
            {
                await client.ShoppingLists.UpdateShoppingList(shoppingList.ShoppingListId, shoppingList);
            }
            catch (Exception ex)
            {
                await DisplayAlert("Error", ex.Message, "OK");
            }

            //var newShoppingList = bindingContext.BindingContext as ShoppingListDto;
            //shoppingListCollection.Add(newShoppingList);
        }

        //Removed by Pearse....what was this for?
        //protected override void OnAppearing()
        //{
        //	if (!loaded){
        //		var client = new BizzaroClient();
        //		shoppingListPaginator = client.ShoppingLists.GetListOfShoppingLists();
        //	}
        //	loaded = false;
        //}

        private async void addNewShoppingList(object sender, EventArgs e)
        {
            var newShoppingList = new ShoppingListDto();
            newShoppingList.Name = newShoppingListName.Text;
            ShoppingListListView.BeginRefresh();
            try
            {
                shoppingListCollection.Add(await client.ShoppingLists.PostNewShoppingList(newShoppingList));
            }
            catch (Exception ex)
            {
                await DisplayAlert("Error", ex.Message, "OK");
            }
            ShoppingListListView.EndRefresh();
            newShoppingListName.Text = "";
        }
    }
}