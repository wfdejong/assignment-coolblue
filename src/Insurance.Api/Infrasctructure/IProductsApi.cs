using Insurance.Api.Dto;
using System.Threading.Tasks;

namespace Insurance.Api.Infrasctructure
{
    public interface IProductsApi
    {
        Task<ApiDto<ProductDto>> Get(int id);
        Task<ApiDto<ProductTypeDto>> GetProductType(int productTypeId);
    }
}
