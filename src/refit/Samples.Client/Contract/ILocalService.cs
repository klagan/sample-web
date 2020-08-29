namespace Samples.HttpClientCon.Contract
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Model;
    using Refit;

    public interface ILocalService
    {
        [Get("/weatherforecast")]
        [Headers("Authorization: Bearer")]
        Task<IEnumerable<WeatherForecast>> Get();
    }
}