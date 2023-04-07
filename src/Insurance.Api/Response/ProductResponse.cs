using Insurance.Api.Domain;

namespace Insurance.Api.Responses;

public class ProductResponse
{
    public int ProductId { get; }
    public float InsuranceValue { get; }

    public ProductResponse(ProductInsurance productInsurance)
    {
        ProductId = productInsurance.ProductId;
        InsuranceValue = productInsurance.InsuranceValue;
    }
}