using Insurance.Api.Dto;
using System.Threading.Tasks;

namespace Insurance.Api.Infrasctructure
{
    public class ProductsApi : BaseApi, IProductsApi
    {        
        public ProductsApi(string apiAddress) : base(apiAddress) { }

        public async Task<ProductDto> Get(int id)
        {
            var response = await Get<ProductDto>(string.Format("/products/{0:G}", id));
            return response.Result;
        }

        public async Task<ProductTypeDto> GetProductType(int productTypeId)
        {
            var response = await Get<ProductTypeDto>(string.Format("/product_types/{0:G}", productTypeId));
            return response.Result;
        }
    }
}