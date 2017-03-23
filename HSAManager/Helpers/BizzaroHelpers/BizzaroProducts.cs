using System;
using System.Threading.Tasks;
using HsaServiceDtos;
using RestSharp.Portable;

namespace HSAManager.Helpers.BizzaroHelpers
{
    public class BizzaroProducts : AbstractBizzaroActions
    {
        public BizzaroProducts() { }
        public BizzaroProducts(string baseUrl) : base(baseUrl) { }

        public Paginator<ProductDto> GetListOfProducts(string query = null)
        {
            var request = new RestRequest("products", Method.GET);
            if (!string.IsNullOrWhiteSpace(query))
                request.AddOrUpdateQueryParameter("query", query);

            return new Paginator<ProductDto>(this, request);
        }

        public async Task<ProductDto> GetOneProduct(int productId)
        {
            if (productId < 1)
                throw new Exception("Product ID must be greater than 0.");
            var request = new RestRequest($"products/{productId}", Method.GET);

            return await CallBizzaro<ProductDto>(request);
        }

        public async Task<ProductDto> PostNewProduct(ProductDto productDto)
        {
            var request = new RestRequest("products", Method.POST);

            return await CallBizzaro<ProductDto>(request, productDto);
        }

        public async Task<StatusOnlyDto> UpdateProduct(int productId, ProductDto updatedProduct)
        {
            var request = new RestRequest("products/{id}", Method.PATCH);
            request.AddUrlSegment("id", productId);

            StatusOnlyDto status = await CallBizzaro(request, updatedProduct);

            return status;
        }
    }
}
