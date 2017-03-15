using RestSharp.Portable.HttpClient;

namespace HSAManager
{
    public abstract class AbstractBizzaroActions
    {
        protected readonly string authToken;
        protected readonly RestClient client;

        protected AbstractBizzaroActions(string authToken, string  baseUrl)
        {
            this.authToken = authToken;
            client = new RestClient(baseUrl);
        }
    }
}
