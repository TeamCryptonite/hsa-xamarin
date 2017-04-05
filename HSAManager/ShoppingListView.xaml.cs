﻿using System;
using System.Diagnostics;
using HsaServiceDtos;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using System.Collections.ObjectModel;

namespace HSAManager
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ShoppingListView : ContentPage
    {
        private readonly BizzaroClient client = new BizzaroClient();
        private ShoppingListDto shoppingList;

		private ObservableCollection<ShoppingListItemDto> shoppingListItems = new ObservableCollection<ShoppingListItemDto>();

        public ShoppingListView(ShoppingListDto shoppingList)
        {
            InitializeComponent();
            setShoppingList(shoppingList.ShoppingListId);
			foreach (ShoppingListItemDto slItem in shoppingList.ShoppingListItems)
			{
				shoppingListItems.Add(slItem);
			}
        }

        private async void ShoppingListLineItemListView_OnItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            if (e.SelectedItem == null)
                return;
            var shoppingListItem = (ShoppingListItemDto)e.SelectedItem;



            ((ListView)sender).SelectedItem = null;
        }

        private async void setShoppingList(int shoppingListId)
        {
            ShoppingListLineItemListView.BeginRefresh();

            try
            {
                shoppingList = await client.ShoppingLists.GetOneShoppingList(shoppingListId);
                Title = shoppingList.Name;
                ShoppingListLineItemListView.ItemsSource = shoppingListItems;
            }
            catch (Exception ex)
            {
                await DisplayAlert("Shopping List Error", "Could not load selected Shopping List. Please try again.", "OK");
            }

            ShoppingListLineItemListView.EndRefresh();
        }

        private async void ButtonCheckmark_OnClicked(object sender, EventArgs e)
        {
            var button = (Xamarin.Forms.Button)sender;
            var shoppingListItem = button.CommandParameter as ShoppingListItemDto;

            if (shoppingListItem == null)
                return;

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

        private void ButtonEdit_OnClicked(object sender, EventArgs e)
        {
            throw new NotImplementedException();
        }

		public async void OnDelete(object sender, EventArgs e)
		{
			var client = new BizzaroClient();
			var mi = (MenuItem)sender;
			var shoppingListItem = mi.CommandParameter as ShoppingListItemDto;
			shoppingListItems.Remove(shoppingListItem);
			await client.ShoppingLists.DeleteShoppingListItem(shoppingList.ShoppingListId, shoppingListItem.ShoppingListItemId);

		}
    }
}