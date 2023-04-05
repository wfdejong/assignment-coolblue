namespace Insurance.Api.Domain
{
    public class ProductInsurance
    {
        public int ProductId { get; set; }
        public string ProductTypeName { get; }
        public bool ShouldBeInsured { get; }
        public float InsuranceValue { get; } = 0f;
        public float SalesPrice { get; }

        public ProductInsurance(int productId, string productTypeName, bool shouldBeInsured, float salesPrice)
        {
            ProductId = productId;
            ProductTypeName = productTypeName;
            ShouldBeInsured = shouldBeInsured;
            SalesPrice = salesPrice;

            if (ProductTypeName == "Laptops" || ProductTypeName == "Smartphones" && shouldBeInsured)
                InsuranceValue += 500;

            if (SalesPrice > 500 && SalesPrice < 2000)
                if (shouldBeInsured)
                    InsuranceValue += 1000;
            if (SalesPrice >= 2000)
                if (shouldBeInsured)
                    InsuranceValue += 2000;
        }
    }
}
