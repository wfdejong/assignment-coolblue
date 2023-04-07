using System.Collections.Generic;
using System.Linq;

namespace Insurance.Api.Domain
{
    public class CartInsurance
    {
        private float _totalInsuranceValue = 0;
        private bool _hasDigitalCamera = false;
        private List<ProductInsurance> _productInsurances = new List<ProductInsurance>();

        public float TotalInsuranceValue => _totalInsuranceValue;

        public IReadOnlyList<ProductInsurance> ProductInsurances => _productInsurances;

        public CartInsurance()
        {
        }

        public void AddProductInsurance(ProductInsurance productInsurance)
        {
            _totalInsuranceValue += productInsurance.InsuranceValue;

            if (!_hasDigitalCamera && productInsurance.ProductTypeName == "Digital cameras")
            {
                _hasDigitalCamera = true;
                _totalInsuranceValue += 500;
            }

            _productInsurances.Add(productInsurance);
        }
    }
}
