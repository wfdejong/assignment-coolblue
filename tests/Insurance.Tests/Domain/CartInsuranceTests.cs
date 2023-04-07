using Insurance.Api.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Insurance.Tests.Domain
{
    public class CartInsuranceTests
    {
        [Theory, MemberData(nameof(CartInsuranceTestData))]
        public void CartInsurance_WithMultipleProducts_CalculatesTotal(float expected, List<ProductInsurance> productInsurances)
        {
            //arrange
            var _sut = new CartInsurance();

            //act
            foreach(var productInsurance in productInsurances) 
            {
                _sut.AddProductInsurance(productInsurance);
            }

            Assert.Equal(expected, _sut.TotalInsuranceValue);
        }

        public static IEnumerable<object[]> CartInsuranceTestData =>
            new List<object[]>
        {
            new object[]
            {
                2000f,
                new List<ProductInsurance>() {
                    new ProductInsurance(1, "Anything", true, 1000),
                    new ProductInsurance(1, "Anything", true, 1000)
                }
            },
            new object[]
            {
                1000f,
                new List<ProductInsurance>() {
                    new ProductInsurance(1, "Anything", true, 500),
                    new ProductInsurance(1, "Anything", true, 1000)
                }
            },
            new object[]
            {
                4000f,
                new List<ProductInsurance>() {
                    new ProductInsurance(1, "Anything", true, 2000),
                    new ProductInsurance(1, "Anything", true, 2000)
                }
            },
            new object[]
            {
                2000f,
                new List<ProductInsurance>() {
                    new ProductInsurance(1, "Laptops", true, 500),
                    new ProductInsurance(1, "Smartphones", true, 1000)
                }
            },
            new object[]
            {
                3000f,
                new List<ProductInsurance>() {
                    new ProductInsurance(1, "Laptops", true, 1000),
                    new ProductInsurance(1, "Smartphones", true, 1000)
                }
            },
            new object[]{
                5000f,
                new List<ProductInsurance>() {
                    new ProductInsurance(1, "Laptops", true, 2000),
                    new ProductInsurance(1, "Smartphones", true, 2000)
                }
            },
            new object[]{
                1500f,
                new List<ProductInsurance>() {
                    new ProductInsurance(1, "Laptops", false, 500),
                    new ProductInsurance(1, "Smartphones", true, 1000)
                }
            },
            new object[]{
                5000f,
                new List<ProductInsurance>() {
                    new ProductInsurance(1, "Laptops", true, 500),
                    new ProductInsurance(1, "Smartphones", true, 1000),
                    new ProductInsurance(1, "Anything", true, 1000),
                    new ProductInsurance(1, "Anything", true, 5000),
                    new ProductInsurance(1, "Anything", false, 1000)
                }
            },
            new object[]{
                2000f,
                new List<ProductInsurance>() {
                    new ProductInsurance(1, "Laptops", true, 500),
                    new ProductInsurance(1, "Digital cameras", true, 1000)
                }
            },
            new object[]{
                2500f,
                new List<ProductInsurance>() {
                    new ProductInsurance(1, "Digital cameras", true, 1000),
                    new ProductInsurance(1, "Digital cameras", true, 1000)
                }
            }
        };
    }
}
