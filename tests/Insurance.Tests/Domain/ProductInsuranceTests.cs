using Insurance.Api.Domain;
using Xunit;

namespace Insurance.Tests.Domain
{
    public class ProductInsuranceTests
    {
        [Theory]
        [InlineData(500, "Laptops", true, 1, 0)]
        [InlineData(0, "Laptops", false, 1, 0)]
        [InlineData(500, "Smartphones", true, 1, 0)]
        [InlineData(0, "Smartphones", false, 1, 0)]
        [InlineData(0, "Anything", true, 1, 0)]
        [InlineData(1000, "Anything", true, 1000, 0)]
        [InlineData(0, "Anything", false, 1000, 0)]
        [InlineData(2000, "Anything", true, 2000, 0)]
        [InlineData(0, "Anything", false, 2000, 0)]
        [InlineData(1500, "Smartphones", true, 1000, 0)]
        [InlineData(1500, "Laptops", true, 1000, 0)]
        [InlineData(15, "Anything", true, 1, 15)]
        [InlineData(1020, "Anything", true, 1000, 20)]

        public void ProductInsurance_WithProductType_CalculatesInsuranceValue(float expected, 
            string productTypeName, bool shouldBeInsured, float salesPrice, float surcharge) 
        {
            //act
            var _sut = new ProductInsurance(1, productTypeName, shouldBeInsured, salesPrice, surcharge);

            //assert
            Assert.Equal(expected, _sut.InsuranceValue);
        }
    }
}
