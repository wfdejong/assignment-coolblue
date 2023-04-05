using Insurance.Api.Domain;

namespace Insurance.Api.Responses;

public class ProductResponse
{
    public int ProductId { get; set; }
    public float InsuranceValue { get; set; }

    public ProductResponse(ProductInsurance productInsurance)
    {
        ProductId = productInsurance.ProductId;
        InsuranceValue = productInsurance.InsuranceValue;
    }
}