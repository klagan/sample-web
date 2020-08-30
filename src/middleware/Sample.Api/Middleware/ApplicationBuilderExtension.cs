namespace Sample.WebApi.Middleware
{
    using Microsoft.AspNetCore.Builder;

    public static class ApplicationBuilderExtension
    {
        public static IApplicationBuilder AddRequestResponseLogger(
            this IApplicationBuilder app
        )
        {
            return app.UseMiddleware<RequestResponseLoggingMiddleware>();
        }
    }
}