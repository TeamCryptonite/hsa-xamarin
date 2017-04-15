using System;
using System.Threading.Tasks;
using HsaServiceDtos;
using Newtonsoft.Json.Linq;
using RestSharp.Portable;

namespace HSAManager.Helpers.BizzaroHelpers
{
    public class BizzaroAggregate : AbstractBizzaroActions
    {
        public BizzaroAggregate()
        {
        }

        public BizzaroAggregate(string baseUrl) : base(baseUrl)
        {
        }

        public async Task<string> GetSpendingOverTime()
        {
            var request = new RestRequest("receiptaggregate/spendingovertime", Method.GET);

            var json = await CallBizzaroJArray(request);
            return json.ToString();
        }

        
    }
}