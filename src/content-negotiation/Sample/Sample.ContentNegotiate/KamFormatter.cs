namespace Sample.ContentNegotiate
{
    using System;
    using System.Collections.Generic;
    using System.Text;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Mvc.Formatters;
    using Microsoft.Extensions.Logging;
    using Microsoft.Net.Http.Headers;

    public class KamFormatter : TextOutputFormatter
    {
        public KamFormatter()
        {
            SupportedMediaTypes.Add(MediaTypeHeaderValue.Parse("application/kam"));
            SupportedEncodings.Add(Encoding.UTF8);
            SupportedEncodings.Add(Encoding.Unicode);
        }

        protected override bool CanWriteType(
            Type type
        )
        {
            if (typeof(WeatherForecast).IsAssignableFrom(type) 
                || typeof(IEnumerable<WeatherForecast>)
                    .IsAssignableFrom(type))
            {
                return base.CanWriteType(type);
            }
            return false;
        }

        public override async Task WriteResponseBodyAsync(
            OutputFormatterWriteContext context,
            Encoding selectedEncoding
        )
        {
            // var serviceProvider = context.HttpContext.RequestServices;
            // var logger = serviceProvider.GetService(typeof(ILogger<VcardOutputFormatter>)) as ILogger;

            var response = context.HttpContext.Response;

            var buffer = new StringBuilder();

            var weatherForecasts = context.Object as IEnumerable<WeatherForecast>;

            foreach (var forecast in weatherForecasts)
            {
                FormatMyObject(buffer, forecast, null);
            }
            
            await response.WriteAsync(buffer.ToString());
        }
        
        private static void FormatMyObject(StringBuilder buffer, WeatherForecast forecast, ILogger logger = null)
        {
            buffer.AppendLine("Version: 2.1");
            buffer.AppendLine($"Temparature C/F: {forecast.TemperatureC};{forecast.TemperatureF}");
            buffer.AppendLine($"Date: {forecast.Date}");
            buffer.AppendLine($"Summary: {forecast.Summary}");
            buffer.AppendLine("Love Kam xxx");
            buffer.AppendLine("");
            // logger.LogInformation("Writing {FirstName} {LastName}", contact.FirstName, contact.LastName);
        }
    }
}