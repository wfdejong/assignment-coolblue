using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Insurance.Api.Controllers;
using Insurance.Api.Infrasctructure;
using Insurance.Api.Requests;
using Insurance.Api.Response;
using Insurance.Api.Responses;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Xunit;

namespace Insurance.Tests
{
    public class InsuranceTests : IClassFixture<ControllerTestFixture>
    {
        private readonly ControllerTestFixture _fixture;
        private const string productsApiUrl = "http://localhost:5002";
        private readonly ProductsApi _productsApi;
        private readonly InsuranceController _sut;
        private readonly ISurchargeResository _surchargeRepository;

        public InsuranceTests(ControllerTestFixture fixture)
        {
            _fixture = fixture;
            _productsApi = new ProductsApi(productsApiUrl);
            _surchargeRepository = new SurchargeRepository();
            _sut = new InsuranceController(_productsApi, _surchargeRepository);
        }

        [Fact]
        public async Task CalculateInsurance_GivenSalesPriceBetween500And2000Euros_ShouldAddThousandEurosToInsuranceCost()
        {
            const float expectedInsuranceValue = 1000;

            var request = new ProductRequest
            {
                ProductId = 1,
            };
            
            var result = await _sut.GetInsuranceByProduct(request);
            
            Assert.Equal(
                expected: expectedInsuranceValue,
                actual: ((ProductResponse)((OkObjectResult)result.Result).Value).InsuranceValue
            );
        }

        [Fact]
        public async Task GetInsuranceByProduct_GivenLaptopPriceUnder500Euros_ShouldAddFiveHundredEurosToInsuranceCost()
        {
            const float expectedInsuranceValue = 500;

            var request = new ProductRequest
            {
                ProductId = 2,
            };
            
            var result = await _sut.GetInsuranceByProduct(request);

            Assert.Equal(
                expected: expectedInsuranceValue,
                actual: ((ProductResponse)((OkObjectResult)result.Result).Value).InsuranceValue
            );
        }

        [Fact]
        public async Task GetInsuranceByProduct_GivenSalesPriceUnder500Euros_ShouldAddZeroEurosToInsuranceCost()
        {
            const float expectedInsuranceValue = 0;

            var request = new ProductRequest
            {
                ProductId = 3,
            };
           
            var result = await _sut.GetInsuranceByProduct(request);

            Assert.Equal(
                expected: expectedInsuranceValue,
                actual: ((ProductResponse)((OkObjectResult)result.Result).Value).InsuranceValue
            );
        }

        [Fact]
        public async Task GetInsuranceByProduct_GivenSalesPriceOver2000Euros_ShouldAddTwoThousandEurosToInsuranceCost()
        {
            const float expectedInsuranceValue = 2000;

            var request = new ProductRequest
            {
                ProductId = 4,
            };
            
            var result = await _sut.GetInsuranceByProduct(request);

            Assert.Equal(
                expected: expectedInsuranceValue,
                actual: ((ProductResponse)((OkObjectResult)result.Result).Value).InsuranceValue
            );
        }

        [Fact]
        public async Task GetInsuranceByProduct_GivenPhonePriceBetween500And2000Euros_ShouldResultInFifteenHundredEurosAsInsuranceCost()
        {
            const float expectedInsuranceValue = 1500;

            var request = new ProductRequest
            {
                ProductId = 5,
            };
            
            var result = await _sut.GetInsuranceByProduct(request);

            Assert.Equal(
                expected: expectedInsuranceValue,
                actual: ((ProductResponse)((OkObjectResult)result.Result).Value).InsuranceValue
            );
        }

        [Fact]
        public async Task GetInsuranceByProduct_GivenSalesPriceBetween500And2000EurosAndProductCannotBeInsured_ShouldResultInZeroAsInsuranceCost()
        {
            const float expectedInsuranceValue = 0;

            var request = new ProductRequest
            {
                ProductId = 6,
            };
            
            var result = await _sut.GetInsuranceByProduct(request);

            Assert.Equal(
                expected: expectedInsuranceValue,
                actual: ((ProductResponse)((OkObjectResult)result.Result).Value).InsuranceValue
            );
        }

        [Fact]
        public async Task GetInsuranceByCart_WithMultipleItems_CalculatesTotal()
        {
            const float totalExpected = 3500;

            var request = new List<ProductRequest>
            {
                new ProductRequest{ProductId = 4},
                new ProductRequest { ProductId= 5 }
            };
                        
            var result = await _sut.GetInsuranceByCart(request);

            Assert.Equal(
                expected: totalExpected,
                actual: ((CartInsuranceResponse)((OkObjectResult)result.Result).Value).TotalInsuranceValue
            );
        }

        [Fact]
        public async Task GetInsuranceByCart_WithProductCannotBeInsured_CalculatesTotal()
        {
            const float totalExpected = 1500;

            var request = new List<ProductRequest>
            {
                new ProductRequest{ProductId = 5},
                new ProductRequest { ProductId= 6 }
            };
                       
            var result = await _sut.GetInsuranceByCart(request);

            Assert.Equal(
                expected: totalExpected,
                actual: ((CartInsuranceResponse)((OkObjectResult)result.Result).Value).TotalInsuranceValue
            );
        }

        [Fact]
        public async Task GetInsuranceByCart_WithCamera_AddsExtra500toTotal()
        {
            const float totalExpected = 3000;

            var request = new List<ProductRequest>
            {
                new ProductRequest{ProductId = 5},
                new ProductRequest { ProductId= 7 }
            };

            var result = await _sut.GetInsuranceByCart(request);

            Assert.Equal(
                expected: totalExpected,
                actual: ((CartInsuranceResponse)((OkObjectResult)result.Result).Value).TotalInsuranceValue
            );
        }

        [Fact]
        public async Task AddSurchargeToProductType_WithSurcharge_AddsSurchargeToProduct()
        {
            const float expectedInsuranceValue = 1120;

            var request = new ProductRequest
            {
                ProductId = 1,
            };

            var surchargeRequest = new SurchargeRequest
            {
                ProductTypeName = "Test type",
                Surcharge = 120
            };

            _sut.AddSurchargeToProductType(surchargeRequest);

            var result = await _sut.GetInsuranceByProduct(request);

            Assert.Equal(
                expected: expectedInsuranceValue,
                actual: ((ProductResponse)((OkObjectResult)result.Result).Value).InsuranceValue
            );
        }

        [Fact]
        public async Task AddSurchargeToProductType_WithDoubleSurcharge_AddsSurchargeToProduct()
        {
            const float expectedInsuranceValue = 1150;

            var request = new ProductRequest
            {
                ProductId = 1,
            };

            var surchargeRequest1 = new SurchargeRequest
            {
                ProductTypeName = "Test type",
                Surcharge = 120
            };

            var surchargeRequest2 = new SurchargeRequest
            {
                ProductTypeName = "Test type",
                Surcharge = 150
            };

            _sut.AddSurchargeToProductType(surchargeRequest1);
            _sut.AddSurchargeToProductType(surchargeRequest2);

            var result = await _sut.GetInsuranceByProduct(request);

            Assert.Equal(
                expected: expectedInsuranceValue,
                actual: ((ProductResponse)((OkObjectResult)result.Result).Value).InsuranceValue
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
                        "products/7",
                        context =>
                        {
                            var product = new
                            {
                                id = 7,
                                name = "Digital Camera",
                                productTypeId = 5,
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
                    ep.MapGet(
                        "product_types/5",
                        context =>
                        {
                            var productType = new
                            {
                                id = 5,
                                name = "Digital cameras",
                                canBeInsured = true
                            };
                            return context.Response.WriteAsync(JsonConvert.SerializeObject(productType));
                        }
                    );
                }
            );
        }
    }
}