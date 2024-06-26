using System;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Moq;
using Moq.Protected;
using Xunit;
using Microsoft.Extensions.Configuration;
using BackendTest.Services.Billings;
using System.Collections.Generic;

namespace BackendTest.Tests.Services
{
    public class BillingService_Test
    {
        [Fact]
        public async Task GetBillingDataAsync_ReturnsSuccess()
        {
            // Arrange
            var mockHttpMessageHandler = new Mock<HttpMessageHandler>();
            mockHttpMessageHandler
                .Protected()
                .Setup<Task<HttpResponseMessage>>(
                    "SendAsync",
                    ItExpr.IsAny<HttpRequestMessage>(),
                    ItExpr.IsAny<CancellationToken>())
                .ReturnsAsync(new HttpResponseMessage
                {
                    StatusCode = HttpStatusCode.OK,
                    Content = new StringContent("Mocked response")
                });

            var mockHttpClientFactory = new Mock<IHttpClientFactory>();
            var client = new HttpClient(mockHttpMessageHandler.Object);
            mockHttpClientFactory.Setup(_ => _.CreateClient(It.IsAny<string>())).Returns(client);

            var inMemorySettings = new Dictionary<string, string> {
                {"ExternalServices:BillingApi:BaseUrl", "http://fakeapi.com"}
            };

            IConfiguration configuration = new ConfigurationBuilder()
                .AddInMemoryCollection(inMemorySettings)
                .Build();

            var billingService = new BillingService(mockHttpClientFactory.Object, configuration);

            // Act
            var response = await billingService.GetBillingDataAsync();

            // Assert
            Assert.True(response.IsSuccessStatusCode);
        }
    }
}
