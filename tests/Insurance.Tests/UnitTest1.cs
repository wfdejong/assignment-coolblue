using System;
using Insurance.Api.Controllers;
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

        public InsuranceTests(ControllerTestFixture fixture)
        {
            _fixture = fixture;
        }

        [Fact]
        public void CalculateInsurance_GivenSalesPriceBetween500And2000Euros_ShouldAddThousandEurosToInsuranceCost()
        {
            const float expectedInsuranceValue = 1000;

            var dto = new HomeController.InsuranceDto
            {
                ProductId = 1,
            };
            var sut = new HomeController();

            var result = sut.CalculateInsurance(dto);

            Assert.Equal(
                expected: expectedInsuranceValue,
                actual: result.InsuranceValue
            );
        }

        [Fact]
        public void CalculateInsurance_GivenLaptopPriceUnder500Euros_ShouldAddFiveHundredEurosToInsuranceCost()
        {
            const float expectedInsuranceValue = 500;

            var dto = new HomeController.InsuranceDto
            {
                ProductId = 2,
            };
            var sut = new HomeController();

            var result = sut.CalculateInsurance(dto);

            Assert.Equal(
                expected: expectedInsuranceValue,
                actual: result.InsuranceValue
            );
        }

        [Fact]
        public void CalculateInsurance_GivenSalesPriceUnder500Euros_ShouldAddZeroEurosToInsuranceCost()
        {
            const float expectedInsuranceValue = 0;

            var dto = new HomeController.InsuranceDto
            {
                ProductId = 3,
            };
            var sut = new HomeController();

            var result = sut.CalculateInsurance(dto);

            Assert.Equal(
                expected: expectedInsuranceValue,
                actual: result.InsuranceValue
            );
        }

        [Fact]
        public void CalculateInsurance_GivenSalesPriceOver2000Euros_ShouldAddTwoThousandEurosToInsuranceCost()
        {
            const float expectedInsuranceValue = 2000;

            var dto = new HomeController.InsuranceDto
            {
                ProductId = 4,
            };
            var sut = new HomeController();

            var result = sut.CalculateInsurance(dto);

            Assert.Equal(
                expected: expectedInsuranceValue,
                actual: result.InsuranceValue
            );
        }


        [Fact]
        public void CalculateInsurance_GivenPhonePriceBetween500And2000Euros_ShouldResultInFifteenHundredEurosAsInsuranceCost()
        {
            const float expectedInsuranceValue = 1500;

            var dto = new HomeController.InsuranceDto
            {
                ProductId = 5,
            };
            var sut = new HomeController();

            var result = sut.CalculateInsurance(dto);

            Assert.Equal(
                expected: expectedInsuranceValue,
                actual: result.InsuranceValue
            );
        }

        [Fact]
        public void CalculateInsurance_GivenSalesPriceBetween500And2000EurosAndProductCannotBeInsured_ShouldResultInZeroAsInsuranceCost()
        {
            const float expectedInsuranceValue = 0;

            var dto = new HomeController.InsuranceDto
            {
                ProductId = 6,
            };
            var sut = new HomeController();

            var result = sut.CalculateInsurance(dto);

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
                        "product_types",
                        context =>
                        {
                            var productTypes = new[]
                                               {
                                                   new
                                                   {
                                                       id = 1,
                                                       name = "Test type",
                                                       canBeInsured = true
                                                   },
                                                   new
                                                   {
                                                       id = 2,
                                                       name = "Laptops",
                                                       canBeInsured = true
                                                   },
                                                   new
                                                   {
                                                       id = 3,
                                                       name = "Smartphones",
                                                       canBeInsured = true
                                                   },
                                                   new
                                                   {
                                                       id = 4,
                                                       name = "Test Type2",
                                                       canBeInsured = false
                                                   }
                                               };
                            return context.Response.WriteAsync(JsonConvert.SerializeObject(productTypes));
                        }
                    );
                }
            );
        }
    }
}