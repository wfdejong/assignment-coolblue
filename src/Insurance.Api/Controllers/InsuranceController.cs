using Insurance.Api.Domain;
using Insurance.Api.Infrasctructure;
using Insurance.Api.Requests;
using Insurance.Api.Response;
using Insurance.Api.Responses;
using Microsoft.AspNetCore.Mvc;
using Serilog;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Insurance.Api.Controllers;

[Route("insurance")]
public class InsuranceController : ControllerBase
{
    private readonly IProductsApi _productApi;
    private readonly ILogger _logger = Log.ForContext("test", nameof(InsuranceController));

	public InsuranceController(IProductsApi productApi)
	{
        _productApi= productApi;
	}

    [HttpPost]
    [Route("product")]
    public async Task<ActionResult<ProductResponse>> GetInsuranceByProduct([FromBody] ProductRequest toInsure)
    {
        try
        {
            //get productInsurance
            var productInsurance = await GetProductInsurance(toInsure.ProductId);

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

    [HttpPost]
    [Route("cart")]
    public async Task<ActionResult<ProductResponse>> GetInsuranceByCart([FromBody] List<ProductRequest> productRequests)
    {
        try 
        {
            var cartInsurance = new CartInsurance();

            foreach (var productRequest in productRequests)
            {
                //get productInsurance
                cartInsurance.AddProductInsurance(await GetProductInsurance(productRequest.ProductId));
            }

            //create response
            var cartInsuranceResponse = new CartInsuranceResponse(cartInsurance);


            return Ok(cartInsuranceResponse);
        }
        catch (Exception e)        
        {
            _logger.Error(e.Message);
            return StatusCode(500, "Something went wrong, please check logs for details");
        }
    }

    private async Task<ProductInsurance> GetProductInsurance(int productId)
    {
        //get product
        _logger.Information(string.Format("Getting insurance for product with Id: {0}", productId));
        var productDto = await _productApi.Get(productId);

        if (!productDto.IsSuccessStatusCode)
            throw new Exception($"Error in searching product. Service returned {productDto.StatusCode}");

        //get producttype                        
        var productTypeDto = await _productApi.GetProductType(productDto.Result.ProductTypeId);

        if (!productTypeDto.IsSuccessStatusCode)
            throw new Exception($"Error in searching productType. Service returned {productTypeDto.StatusCode}");

        //create and return Domain object
        return new ProductInsurance(
            productId, productTypeDto.Result.Name, productTypeDto.Result.CanBeInsured, productDto.Result.SalesPrice);

        
    }
}

