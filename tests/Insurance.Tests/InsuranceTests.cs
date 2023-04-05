using System;
using System.Threading.Tasks;
using Insurance.Api.Controllers;
using Insurance.Api.Infrasctructure;
using Insurance.Api.Requests;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Hosting;
using Newtonsoft.Json;
using Xunit;

namespace Insurance.Tests
{
    public class InsuranceTests : IClassFixture<ControllerTestFixture>
    {
        private readonly ControllerTestFixture _fixture;
        private const string productsApiUrl = "http://localhost:5002";

        public InsuranceTests(ControllerTestFixture fixture)
        {
            _fixture = fixture;
        }

        [Fact]
        public async Task CalculateInsurance_GivenSalesPriceBetween500And2000Euros_ShouldAddThousandEurosToInsuranceCost()
        {
            const float expectedInsuranceValue = 1000;

            var dto = new ProductRequest
            {
                ProductId = 1,
            };
            var productApi = new ProductsApi(productsApiUrl);
            var sut = new InsuranceController(productApi);

            var result = await sut.GetInsuranceByProduct(dto);

            Assert.Equal(
                expected: expectedInsuranceValue,
                actual: result.InsuranceValue
            );
        }

        [Fact]
        public async Task CalculateInsurance_GivenLaptopPriceUnder500Euros_ShouldAddFiveHundredEurosToInsuranceCost()
        {
            const float expectedInsuranceValue = 500;

            var dto = new ProductRequest
            {
                ProductId = 2,
            };
            var productApi = new ProductsApi(productsApiUrl);
            var sut = new InsuranceController(productApi);

            var result = await sut.GetInsuranceByProduct(dto);

            Assert.Equal(
                expected: expectedInsuranceValue,
                actual: result.InsuranceValue
            );
        }

        [Fact]
        public async Task CalculateInsurance_GivenSalesPriceUnder500Euros_ShouldAddZeroEurosToInsuranceCost()
        {
            const float expectedInsuranceValue = 0;

            var dto = new ProductRequest
            {
                ProductId = 3,
            };
            var productApi = new ProductsApi(productsApiUrl);
            var sut = new InsuranceController(productApi);

            var result = await sut.GetInsuranceByProduct(dto);

            Assert.Equal(
                expected: expectedInsuranceValue,
                actual: result.InsuranceValue
            );
        }

        [Fact]
        public async Task CalculateInsurance_GivenSalesPriceOver2000Euros_ShouldAddTwoThousandEurosToInsuranceCost()
        {
            const float expectedInsuranceValue = 2000;

            var dto = new ProductRequest
            {
                ProductId = 4,
            };
            var productApi = new ProductsApi(productsApiUrl);
            var sut = new InsuranceController(productApi);

            var result = await sut.GetInsuranceByProduct(dto);

            Assert.Equal(
                expected: expectedInsuranceValue,
                actual: result.InsuranceValue
            );
        }


        [Fact]
        public async Task CalculateInsurance_GivenPhonePriceBetween500And2000Euros_ShouldResultInFifteenHundredEurosAsInsuranceCost()
        {
            const float expectedInsuranceValue = 1500;

            var dto = new ProductRequest
            {
                ProductId = 5,
            };
            var productApi = new ProductsApi(productsApiUrl);
            var sut = new InsuranceController(productApi);

            var result = await sut.GetInsuranceByProduct(dto);

            Assert.Equal(
                expected: expectedInsuranceValue,
                actual: result.InsuranceValue
            );
        }

        [Fact]
        public async Task CalculateInsurance_GivenSalesPriceBetween500And2000EurosAndProductCannotBeInsured_ShouldResultInZeroAsInsuranceCost()
        {
            const float expectedInsuranceValue = 0;

            var dto = new ProductRequest
            {
                ProductId = 6,
            };
            var productApi = new ProductsApi(productsApiUrl);
            var sut = new InsuranceController(productApi);

            var result = await sut.GetInsuranceByProduct(dto);

            Assert.Equal(
                expected: expectedInsuranceValue,
                actual: result.InsuranceValue
            );
        }
    }

    public class ControllerTestFixture : IDisposable
    {
        private readonly IHost _host;

        public ControllerTestFixture()
        {
            _host = new HostBuilder()
                   .ConfigureWebHostDefaults(
                        b => b.UseUrls("http://localhost:5002")
                              .UseStartup<ControllerTestStartup>()
                    )
                   .Build();

            _host.Start();
        }

        public void Dispose() => _host.Dispose();
    }

    public class ControllerTestStartup
    {
        public void Configure(IApplicationBuilder app)
        {
            app.UseRouting();
            app.UseEndpoints(
                ep =>
                {
                    ep.MapGet(
                        "products/1",
                        context =>
                        {
                            int productId = 1;
                            var product = new
                            {
                                id = productId,
                                name = "Test Product",
                                productTypeId = 1,
                                salesPrice = 750
                            };
                            return context.Response.WriteAsync(JsonConvert.SerializeObject(product));
                        }
                    );
                    ep.MapGet(
                        "products/2",
                        context =>
                        {
                            var product = new
                            {
                                id = 2,
                                name = "Test Laptop",
                                productTypeId = 2,
                                salesPrice = 200
                            };
                            return context.Response.WriteAsync(JsonConvert.SerializeObject(product));
                        }
                    );
                    ep.MapGet(
                        "products/3",
                        context =>
                        {
                            var product = new
                            {
                                id = 3,
                                name = "Test Product",
                                productTypeId = 1,
                                salesPrice = 200
                            };
                            return context.Response.WriteAsync(JsonConvert.SerializeObject(product));
                        }
                    );
                    ep.MapGet(
                        "products/4",
                        context =>
                        {
                            var product = new
                            {
                                id = 4,
                                name = "Test Product",
                                productTypeId = 1,
                                salesPrice = 2000
                            };
                            return context.Response.WriteAsync(JsonConvert.SerializeObject(product));
                        }
                    );
                    ep.MapGet(
                        "products/5",
                        context =>
                        {
                            var product = new
                            {
                                id = 5,
                                name = "Test Product",
                                productTypeId = 3,
                                salesPrice = 1000
                            };
                            return context.Response.WriteAsync(JsonConvert.SerializeObject(product));
                        }
                    );
                    ep.MapGet(
                        "products/6",
                        context =>
                        {
                            var product = new
                            {
                                id = 6,
                                name = "Test Product",
                                productTypeId = 4,
                                salesPrice = 1000
                            };
                            return context.Response.WriteAsync(JsonConvert.SerializeObject(product));
                        }
                    );
                    ep.MapGet(
                        "product_types/1",
                        context =>
                        {
                            var productType = new
                            {
                                id = 1,
                                name = "Test type",
                                canBeInsured = true
                            };
                            return context.Response.WriteAsync(JsonConvert.SerializeObject(productType));
                        }
                    );
                    ep.MapGet(
                        "product_types/2",
                        context =>
                        {
                            var productType = new
                            {
                                id = 2,
                                name = "Laptops",
                                canBeInsured = true
                            };
                            return context.Response.WriteAsync(JsonConvert.SerializeObject(productType));
                        }
                    );
                    ep.MapGet(
                        "product_types/3",
                        context =>
                        {
                            var productType = new
                            {
                                id = 3,
                                name = "Smartphones",
                                canBeInsured = true
                            };                      
                            return context.Response.WriteAsync(JsonConvert.SerializeObject(productType));
                        }
                    );
                    ep.MapGet(
                        "product_types/4",
                        context =>
                        {
                            var productType = new
                            {
                                id = 4,
                                name = "Test Type2",
                                canBeInsured = false
                            };
                            return context.Response.WriteAsync(JsonConvert.SerializeObject(productType));
                        }
                    );
                }
            );
        }
    }
}