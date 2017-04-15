using HSAManager.Helpers.BizzaroHelpers;

namespace HSAManager
{
    public class BizzaroClient
    {
        public BizzaroClient()
        {
            Receipts = new BizzaroReceipts();
            Stores = new BizzaroStores();
            Products = new BizzaroProducts();
            ShoppingLists = new BizzaroShoppingLists();
            Aggregate = new BizzaroAggregate();

        }

        public BizzaroClient(string baseUrl)
        {
            Receipts = new BizzaroReceipts(baseUrl);
            Stores = new BizzaroStores(baseUrl);
            Products = new BizzaroProducts(baseUrl);
            ShoppingLists = new BizzaroShoppingLists(baseUrl);
            Aggregate = new BizzaroAggregate(baseUrl);
        }

        public BizzaroReceipts Receipts { get; set; }
        public BizzaroStores Stores { get; set; }
        public BizzaroProducts Products { get; set; }
        public BizzaroShoppingLists ShoppingLists { get; set; }
        public BizzaroAggregate Aggregate { get; set; }
    }
}