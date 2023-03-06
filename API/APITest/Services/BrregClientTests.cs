using API.Models.OrganizationInfos.ValueObjects;
using API.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Moq;
using Moq.Protected;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace APITest.Services
{
    public class BrregClientTests
    {
        private Mock<HttpClient> _httpClientMock;
        private Mock<IConfiguration> _configurationMock;
        private Mock<HttpMessageHandler> _httpMessageHandlerMock;
        private BrregClient _brregClient;
        private HttpClient _httpClient;
        private Mock<ILogger<BrregClient>> _logger;

        [SetUp]
        public void SetUp()
        {
            _logger = new Mock<ILogger<BrregClient>>();
            _httpClientMock = new Mock<HttpClient>();
            _configurationMock = new Mock<IConfiguration>();
            _configurationMock.Setup(c => c["BrregClient"]).Returns("https://data.brreg.no/enhetsregisteret/api");
            _httpMessageHandlerMock = new Mock<HttpMessageHandler>();
            _httpClient = new HttpClient(_httpMessageHandlerMock.Object);
            _brregClient = new BrregClient(_httpClient, _configurationMock.Object, _logger.Object);
        }
        [Test]
        public async Task GetOrganization_OnlyTwoRequestsAtSameTime()
        {
            var orgNo1 = "810463902";
            var orgNo2 = "810684402";
            var response1 = new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = new StringContent(@"{
                                ""Organisasjonsnummer"": ""810463902"",
                                ""Navn"": ""TEST COMPANY"",
                                ""Konkurs"": false,
                                ""Naeringskode1"": {
                                    ""Beskrivelse"": ""IT-konsulentvirksomhet"",
                                    ""Kode"": ""62.01""
                                    }
                                }")
            };
            var response2 = new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = new StringContent(@"{
                                ""Organisasjonsnummer"": ""810684402"",
                                ""Navn"": ""ANOTHER TEST COMPANY"",
                                ""Konkurs"": false,
                                ""Naeringskode1"": {
                                    ""Beskrivelse"": ""Engroshandel med elektrisk materiell"",
                                    ""Kode"": ""46.52""
                                }
                            }")
            };
            _httpMessageHandlerMock.Protected()
                .Setup<Task<HttpResponseMessage>>("SendAsync", ItExpr.Is<HttpRequestMessage>(r => r.RequestUri.PathAndQuery.EndsWith(orgNo1)), ItExpr.IsAny<CancellationToken>())
                .ReturnsAsync(response1);
            _httpMessageHandlerMock.Protected()
                .Setup<Task<HttpResponseMessage>>("SendAsync", ItExpr.Is<HttpRequestMessage>(r => r.RequestUri.PathAndQuery.EndsWith(orgNo2)), ItExpr.IsAny<CancellationToken>())
                .ReturnsAsync(response2);

            var brregClient = new BrregClient(_httpClientMock.Object, _configurationMock.Object, _logger.Object);

            var tasks = new[]
            {
                brregClient.GetOrganization(orgNo1),
                brregClient.GetOrganization(orgNo2),
                brregClient.GetOrganization("999999999"),
                brregClient.GetOrganization("811550582")
            };
            var taskResults = await Task.WhenAll(tasks);

            _httpClientMock.Verify(
                m => m.SendAsync(It.IsAny<HttpRequestMessage>(), It.IsAny<CancellationToken>()),
                Times.AtMost(2)
            );
            Assert.NotNull(taskResults[0]);
            Assert.AreEqual(taskResults[0].BrregNavn, "E BERENTSEN AS");
            Assert.AreEqual(taskResults[0].Organisasjonsform.Kode, "AS");
            Assert.NotNull(taskResults[1]);
            Assert.Null(taskResults[2]);
            Assert.NotNull(taskResults[3]);
        }

    }
}
