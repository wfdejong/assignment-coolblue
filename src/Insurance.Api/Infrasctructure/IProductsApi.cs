using Insurance.Api.Dto;
using System.Threading.Tasks;

namespace Insurance.Api.Infrasctructure
{
    public interface IProductsApi
    {
        Task<ProductDto> Get(int id);
        Task<ProductTypeDto> GetProductType(int productTypeId);
    }
}
