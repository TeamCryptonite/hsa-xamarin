using System;
using System.Threading.Tasks;
using System.Web.SessionState;
using HsaServiceDtos;
using RestSharp.Portable;

namespace HSAManager
{
    public class BizzaroStores : AbstractBizzaroActions
    {
        public BizzaroStores()
        {
        }

        public BizzaroStores(string baseUrl) : base(baseUrl)
        {
        }
        public Paginator<StoreDto> GetListOfStores(string query = null, int? productId = null, int? radius = null,
            double? userLat = null, double? userLong = null)
        {
            var request = new RestRequest("stores", Method.GET);
            if (!string.IsNullOrWhiteSpace(query))
                request.AddOrUpdateQueryParameter("query", query);
            if(productId.HasValue)
                request.AddOrUpdateQueryParameter("productid", productId);
            if (radius.HasValue && userLat.HasValue && userLong.HasValue)
            {
                request.AddOrUpdateQueryParameter("radius", radius);
                request.AddOrUpdateQueryParameter("userlat", userLat);
                request.AddOrUpdateQueryParameter("userlong", userLong);
            } else if (radius.HasValue || userLat.HasValue || userLong.HasValue)
                throw new Exception("Store location query must include radius, userLat, and userLong.");

            return new Paginator<StoreDto>(this, request);
        }

        public async Task<StoreDto> GetOneStore(int storeId)
        {
            if (storeId < 1)
                throw new Exception("Store ID must be greater than 0.");
            var request = new RestRequest($"stores/{storeId}", Method.GET);

            return await CallBizzaro<StoreDto>(request);
        }

        public async Task<StoreDto> PostNewStore(StoreDto storeDto)
        {
            var request = new RestRequest("stores", Method.POST);

            return await CallBizzaro<StoreDto>(request, storeDto);
        }


        public async Task<StatusOnlyDto> UpdateStore(int storeId, StoreDto updatedStore)
        {
            var request = new RestRequest("stores/{id}", Method.PATCH);
            request.AddUrlSegment("id", storeId);

            StatusOnlyDto status = await CallBizzaro(request, updatedStore);

            return status;
        }
    }
}