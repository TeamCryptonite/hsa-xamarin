using System;
using System.Threading.Tasks;
using HsaServiceDtos;
using Newtonsoft.Json.Linq;
using RestSharp.Portable;

namespace HSAManager.Helpers.BizzaroHelpers
{
    public class BizzaroAggregate : AbstractBizzaroActions
    {
        public enum TimePeriod
        {
            YearMonth, Day, Month, Year
        }
        public BizzaroAggregate()
        {
        }

        public BizzaroAggregate(string baseUrl) : base(baseUrl)
        {
        }

        public async Task<string> GetSpendingOverTime(DateTime? startDateStr = null, DateTime? endDateStr = null, TimePeriod? timePeriod = null)
        {
            var request = new RestRequest("receiptaggregate/spendingovertime", Method.GET);
            if (startDateStr.HasValue)
                request.AddOrUpdateQueryParameter("startDateStr", startDateStr.Value.ToString());
            if(endDateStr.HasValue)
                request.AddOrUpdateQueryParameter("endDateStr", endDateStr.Value.ToString());
            if (timePeriod.HasValue)
                request.AddOrUpdateQueryParameter("timePeriod", timePeriod.ToString().ToLower());

            var json = await CallBizzaroJArray(request);
            return json.ToString();
        }

        
    }
}