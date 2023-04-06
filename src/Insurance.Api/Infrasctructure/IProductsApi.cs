using Insurance.Api.Dto;
using System.Threading.Tasks;

namespace Insurance.Api.Infrasctructure
{
    public interface IProductsApi
    {
        Task<Dto<ProductDto>> Get(int id);
        Task<Dto<ProductTypeDto>> GetProductType(int productTypeId);
    }
}
