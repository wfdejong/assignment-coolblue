using Insurance.Api.Domain;
using Insurance.Api.Infrasctructure;
using Insurance.Api.Requests;
using Insurance.Api.Responses;
using Microsoft.AspNetCore.Mvc;
using Serilog;
using System;
using System.Threading.Tasks;

namespace Insurance.Api.Controllers;

public class InsuranceController : ControllerBase
{
    private readonly IProductsApi _productApi;
    private readonly ILogger _logger = Log.ForContext("test", nameof(InsuranceController));

	public InsuranceController(IProductsApi productApi)
	{
        _productApi= productApi;
	}

    [HttpPost]
    [Route("api/insurance/product")]
    public async Task<ActionResult<ProductResponse>> GetInsuranceByProduct([FromBody] ProductRequest toInsure)
    {
        try
        {
            //get product
            _logger.Information(string.Format("Getting insurance for product with Id: {0}", toInsure.ProductId));
            var productDto = await _productApi.Get(toInsure.ProductId);

            if (!productDto.IsSuccessStatusCode)
                throw new Exception($"Error in searching product. Service returned {productDto.StatusCode}");

            //get producttype                        
            var productTypeDto = await _productApi.GetProductType(productDto.Result.ProductTypeId);

            if (!productTypeDto.IsSuccessStatusCode)
                throw new Exception($"Error in searching productType. Service returned {productTypeDto.StatusCode}");

            //create Domain object
            var productInsurance = new ProductInsurance(
                toInsure.ProductId, productTypeDto.Result.Name, productTypeDto.Result.CanBeInsured, productDto.Result.SalesPrice);

            //create response
            var productResponse = new ProductResponse(productInsurance);
                                    
            return Ok(productResponse);
        }
        catch(Exception e)
        {
            _logger.Error(e.Message);
            return StatusCode(500, "Something went wrong, please check logs for details");
        }
    }
}

