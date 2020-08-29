namespace Samples.HttpClientCon.Model
{
    using System;

    /// <summary>
    /// Copied straight from the local service.  Ideally this would be in a shared library
    /// </summary>
    public class WeatherForecast
    {
        public DateTime Date { get; set; }

        public int TemperatureC { get; set; }

        public int TemperatureF => 32 + (int) (TemperatureC / 0.5556);

        public string Summary { get; set; }
    }
}