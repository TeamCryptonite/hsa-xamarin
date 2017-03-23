using System;
using System.Threading.Tasks;
using HsaServiceDtos;
using RestSharp.Portable;

namespace HSAManager.Helpers.BizzaroHelpers
{
    public class BizzaroShoppingLists : AbstractBizzaroActions
    {
        public BizzaroShoppingLists() { }
        public BizzaroShoppingLists(string baseUrl) : base(baseUrl) { }

        public Paginator<ShoppingListDto> GetListOfShoppingLists(string query = null)
        {
            var request = new RestRequest("shoppinglists", Method.GET);
            if (!string.IsNullOrWhiteSpace(query))
                request.AddOrUpdateQueryParameter("query", query);

            return new Paginator<ShoppingListDto>(this, request);
        }

        public async Task<ShoppingListDto> GetOneShoppingList(int shoppinglistId)
        {
            if (shoppinglistId < 1)
                throw new Exception("ShoppingList ID must be greater than 0.");
            var request = new RestRequest($"shoppinglists/{shoppinglistId}", Method.GET);

            return await CallBizzaro<ShoppingListDto>(request);
        }

        public async Task<ShoppingListDto> PostNewShoppingList(ShoppingListDto shoppinglistDto)
        {
            var request = new RestRequest("shoppinglists", Method.POST);

            return await CallBizzaro<ShoppingListDto>(request, shoppinglistDto);
        }

        public async Task<StatusOnlyDto> UpdateShoppingList(int shoppinglistId, ShoppingListDto updatedShoppingList)
        {
            var request = new RestRequest("shoppinglists/{id}", Method.PATCH);
            request.AddUrlSegment("id", shoppinglistId);

            StatusOnlyDto status = await CallBizzaro(request, updatedShoppingList);

            return status;
        }
    }
}
