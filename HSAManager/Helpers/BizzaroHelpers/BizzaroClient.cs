﻿using restsharpportabletesting.Helpers.BizzaroHelpers;

namespace HSAManager
{
    public class BizzaroClient
    {
        public BizzaroReceipts Receipts { get; set; }
        public BizzaroStores Stores { get; set; }
        public BizzaroProducts Products { get; set; }

        public BizzaroClient()
        {
            Receipts = new BizzaroReceipts();
            Stores = new BizzaroStores();
            Products = new BizzaroProducts();
        }

        public BizzaroClient(string baseUrl)
        {
            Receipts = new BizzaroReceipts(baseUrl);
            Stores = new BizzaroStores(baseUrl);
            Products = new BizzaroProducts(baseUrl);
        }
    }
}