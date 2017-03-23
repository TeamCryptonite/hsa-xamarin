using System;
using System.Collections.Generic;
using System.Diagnostics;
using HsaServiceDtos;
using Xamarin.Forms;

namespace HSAManager
{
	public partial class shoppingList : ContentPage
	{
        BizzaroClient client = new BizzaroClient();
		public shoppingList()
		{
			InitializeComponent();
		}

	    private async void Button_OnClicked(object sender, EventArgs e)
	    {
            var newSL = new ShoppingListDto()
            {
                DateTime = DateTime.Now,
                Name = "A New Shopping List",
                Description = "Nothing exciting here",
                ShoppingListItems = new List<ShoppingListItemDto>()
                {
                    new ShoppingListItemDto()
                    {
                        Checked = false,
                        ProductName = "TestProd",
                        Quantity = 1
                    },
                    new ShoppingListItemDto()
                    {
                        Checked = true,
                        ProductName = "Prod2",
                        Quantity = 14
                    }
                }
            };
	        var test = await client.ShoppingLists.PostNewShoppingList(newSL);

            Debug.WriteLine("BreakPoint");
	    }
	}
}
