namespace HSAManager
{
    public class BizzaroClient
    {
        public BizzaroReceipts Receipts { get; set; }

        public BizzaroClient(string authToken, string baseUrl = "https://bizzaro.azurewebsites.net/api")
        {
            Receipts = new BizzaroReceipts(authToken, baseUrl);
        }
    }
}