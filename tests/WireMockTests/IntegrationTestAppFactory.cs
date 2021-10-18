using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Collections.Generic;
using WireMock.Server;

namespace WireMockTests
{
    public class IntegrationTestAppFactory<TStartup> 
        : WebApplicationFactory<TStartup> where TStartup : class
    {
        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            var wireMockServer = WireMockServer.Start();

            builder.ConfigureAppConfiguration(config =>
            {
                config.AddInMemoryCollection(new KeyValuePair<string, string>[]
                {
                    new("Clients:ForeCast", wireMockServer.Urls[0])
                });
            }).ConfigureServices((_, services) => 
            {
                services.AddSingleton(wireMockServer);
            });
        }
    }
}
