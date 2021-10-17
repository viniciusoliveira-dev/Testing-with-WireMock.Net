using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace Api
{
    public class WeatherForecastServices
    {
        private readonly IHttpClientFactory _client;
        

        public WeatherForecastServices(IHttpClientFactory client)
        {
            _client = client;
        }

        public IEnumerable<WeatherForecast> GetBaseWeatherForecast()
        {
            var rng = new Random();
            var summaries = new[]
            {
                "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
            };

            return Enumerable.Range(1, 5).Select(index => new WeatherForecast
            {
                Date = DateTime.Now.AddDays(index),
                TemperatureC = rng.Next(-20, 55),
                Summary = summaries[rng.Next(summaries.Length)]
            })
            .ToArray();
        }

        public async Task<WeatherForecast[]> GetWeatherForecastAsync()
        {
            var httpClient = _client.CreateClient(name: "WeatherForecastClient");
            
            var response = await httpClient.GetAsync("WeatherForecast/basecontroller");
         
            return JsonConvert.DeserializeObject<WeatherForecast[]>(response.Content.ReadAsStringAsync().Result);
        }
    }
}
