namespace HSAManager
{
    public class BizzaroClient
    {
        public BizzaroClient(string baseUrl = "https://bizzaro.azurewebsites.net/api")
        {
            Receipts = new BizzaroReceipts(baseUrl);
        }

        public BizzaroReceipts Receipts { get; set; }
    }
}