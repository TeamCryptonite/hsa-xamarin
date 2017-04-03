using System;
using System.Threading.Tasks;
using HsaServiceDtos;
using RestSharp.Portable;

namespace HSAManager.Helpers.BizzaroHelpers
{
    public class BizzaroStores : AbstractBizzaroActions
    {
        public BizzaroStores()
        {
        }

        public BizzaroStores(string baseUrl) : base(baseUrl)
        {
        }

        public Paginator<StoreDto> GetListOfStores(string query = null, int? productId = null, int? radius = null
            , LocationDto userLocation = null)
        {
            var request = new RestRequest("stores", Method.GET);
            if (!string.IsNullOrWhiteSpace(query))
                request.AddOrUpdateQueryParameter("query", query);
            if (productId.HasValue)
                request.AddOrUpdateQueryParameter("productid", productId);
            if (userLocation != null)
            {
                request.AddOrUpdateQueryParameter("userLat", userLocation.Latitude);
                request.AddOrUpdateQueryParameter("userLong", userLocation.Longitude);
            }

            if (radius.HasValue && userLocation != null)
                request.AddOrUpdateQueryParameter("radius", radius);

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

            var status = await CallBizzaro(request, updatedStore);

            return status;
        }
    }
}