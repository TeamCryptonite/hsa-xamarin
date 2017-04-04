using System;
using HsaServiceDtos;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace HSAManager
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ShoppingListView : ContentPage
    {
        private readonly BizzaroClient client = new BizzaroClient();
        private ShoppingListDto shoppingList;

        public ShoppingListView(ShoppingListDto shoppingList)
        {
            InitializeComponent();
            setShoppingList(shoppingList.ShoppingListId);
        }

        private async void ShoppingListLineItemListView_OnItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            if (e.SelectedItem == null)
                return;
            var shoppingListItem = (ShoppingListItemDto) e.SelectedItem;
            if (shoppingListItem.Checked.HasValue == false)
            {
                shoppingListItem.Checked = false;
                return;
            }
            switch (shoppingListItem.Checked)
            {
                case true:
                    shoppingListItem.Checked = false;
                    break;
                case false:
                    shoppingListItem.Checked = true;
                    break;
                default:
                    shoppingListItem.Checked = false;
                    break;
            }


            ((ListView) sender).SelectedItem = null;
            try
            {
                await client.ShoppingLists.UpdateShoppingListItem(shoppingList.ShoppingListId, shoppingListItem);
            }
            catch (Exception ex)
            {
                await DisplayAlert("Exception", "Could not save changes. Please try again.", "Ok");
                setShoppingList(shoppingList.ShoppingListId);
            }
        }

        private async void setShoppingList(int shoppingListId)
        {
            ShoppingListLineItemListView.BeginRefresh();

            try
            {
                shoppingList = await client.ShoppingLists.GetOneShoppingList(shoppingListId);
                Title = shoppingList.Name;
                ShoppingListLineItemListView.ItemsSource = shoppingList.ShoppingListItems;
            }
            catch (Exception ex)
            {
                await DisplayAlert("Shopping List Error", "Could not load selected Shopping List. Please try again.", "OK");
            }

            ShoppingListLineItemListView.EndRefresh();
        }
    }
}