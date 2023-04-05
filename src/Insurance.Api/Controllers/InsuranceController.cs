using Insurance.Api.Domain;
using Insurance.Api.Infrasctructure;
using Insurance.Api.Requests;
using Insurance.Api.Responses;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Insurance.Api.Controllers;

public class InsuranceController
{
    private readonly IProductsApi _productApi;

	public InsuranceController(IProductsApi productApi)
	{
        _productApi= productApi;
	}

    [HttpPost]
    [Route("api/insurance/product")]
    public async Task<ProductResponse> GetInsuranceByProduct([FromBody] ProductRequest toInsure)
    {
        //get product
        var product = await _productApi.Get(toInsure.ProductId);

        //get producttype
        var productType = await _productApi.GetProductType(product.ProductTypeId);
        
        var productInsurance = new ProductInsurance(toInsure.ProductId, productType.Name, productType.CanBeInsured, product.SalesPrice);

        return new ProductResponse(productInsurance);
    }
}

