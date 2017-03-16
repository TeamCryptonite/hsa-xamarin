namespace HSAManager
{
    public class BizzaroClient
    {
        public BizzaroClient(string authToken, string baseUrl = "https://bizzaro.azurewebsites.net/api")
        {
            Receipts = new BizzaroReceipts(authToken, baseUrl);
        }

        public BizzaroReceipts Receipts { get; set; }
    }
}