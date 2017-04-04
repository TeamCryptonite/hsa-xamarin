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
            foreach (var shoppingList in await shoppingListPaginator.Next())
                shoppingListCollection.Add(shoppingList);
            ShoppingListListView.EndRefresh();
        }

        private async void ShoppingListListView_OnItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            if (e.SelectedItem == null)
                return;
            ((ListView)sender).SelectedItem = null;
            await Navigation.PushAsync(new ShoppingListView((ShoppingListDto) e.SelectedItem));
        }
    }
}