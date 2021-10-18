using System;

namespace Api
{
    public class WeatherForecast
    {
        public DateTime Date { get; set; }

        public int TemperatureC { get; set; }

        public int TemperatureF { get; set; }

        public string Summary { get; set; }

        public WeatherForecast()
        {
            TemperatureF = CalculateTemperatureF();
        }

        private int CalculateTemperatureF()
        {
            return 32 + (int)(TemperatureC / 0.5556);
        }
    }
}
