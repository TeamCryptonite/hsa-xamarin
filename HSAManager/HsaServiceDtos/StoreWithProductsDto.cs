using System.Collections.Generic;

namespace HsaServiceDtos
{
    public sealed class StoreWithProductsDto
    {
        public StoreWithProductsDto()
        {
            this.Products = new List<ProductDto>();
        }
        public int StoreId { get; set; }
        public string Name { get; set; }
        public IEnumerable<ProductDto> Products { get; set; }
    }
}