using Insurance.Api.Dto;
using System.Threading.Tasks;

namespace Insurance.Api.Infrasctructure
{
    public class ProductsApi : BaseApi, IProductsApi
    {        
        public ProductsApi(string apiAddress) : base(apiAddress) { }

        public async Task<Dto<ProductDto>> Get(int id)
        {
            return await Get<ProductDto>(string.Format("/products/{0:G}", id));            
        }

        public async Task<Dto<ProductTypeDto>> GetProductType(int productTypeId)
        {
            return await Get<ProductTypeDto>(string.Format("/product_types/{0:G}", productTypeId));            
        }
    }
}