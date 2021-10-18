using Api;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using System;
using System.Net.Http;
using System.Threading.Tasks;
using WireMock.RequestBuilders;
using WireMock.ResponseBuilders;
using WireMock.Server;
using Xunit;

namespace WireMockTests
{
    public class UnitTest1 : IClassFixture<IntegrationTestAppFactory<Startup>>
    {
        private readonly IntegrationTestAppFactory<Startup> _factory;
        private readonly WireMockServer _server;

        public UnitTest1(IntegrationTestAppFactory<Startup> factory)
        {
            _factory = factory;
            _server = _factory.Services.GetRequiredService<WireMockServer>();
        }

        public static WeatherForecast MockedResponse()
        {
            return new()
            {
                Date = DateTime.Now,
                TemperatureC = -9,
                TemperatureF = 16,
                Summary = "Cool"
            };
        }

        [Fact]
        public async Task WireMockSample()
        {
            var wireMockServer = WireMockServer.Start();
            wireMockServer
                .Given(Request.Create().WithPath("/any").UsingGet())
                .RespondWith(
                    Response.Create()
                        .WithStatusCode(200)
                        .WithBodyAsJson(MockedResponse())
                );

            var response = await new HttpClient().GetAsync($"{wireMockServer.Urls[0]}/any");
            var jsonResponse = JsonConvert.DeserializeObject<WeatherForecast>(response.Content.ReadAsStringAsync().Result);
            
            Assert.Equal(MockedResponse().ToString(), jsonResponse.ToString());
            wireMockServer.Stop();
        }

        [Fact]
        public async Task WeatherForecastClient_ok_mock_response()
        {
            _server
                .Given(Request.Create().WithPath("/WeatherForecast/basecontroller").UsingGet())
                .RespondWith(
                    Response.Create()
                        .WithStatusCode(200)
                        .WithBodyAsJson(new[] { MockedResponse() })
                );
            var httpClient = _factory.CreateClient();

            var response = await httpClient.GetAsync($"https://localhost:5001/WeatherForecast/client");
            var jsonResponse = JsonConvert.DeserializeObject<WeatherForecast[]>(response.Content.ReadAsStringAsync().Result);

            Assert.Equal(MockedResponse().ToString(), jsonResponse[0].ToString());
            _server.Stop();
        }
    }
}
