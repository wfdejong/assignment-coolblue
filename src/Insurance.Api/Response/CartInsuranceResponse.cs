
using Insurance.Api.Domain;
using Insurance.Api.Responses;
using System.Collections.Generic;

namespace Insurance.Api.Response
{
    public class CartInsuranceResponse
    {
        public float TotalInsuranceValue { get; }

        public IReadOnlyCollection<ProductResponse> ProductInsurances {get;}

        public CartInsuranceResponse(CartInsurance cartInsurance)
        {
            TotalInsuranceValue = cartInsurance.TotalInsuranceValue;

            var productInsurances = new List<ProductResponse>();
            
            foreach(var productInsurance in cartInsurance.ProductInsurances)
            {
                productInsurances.Add(new ProductResponse(productInsurance));
            }

            ProductInsurances = productInsurances;
        }
    }
}
