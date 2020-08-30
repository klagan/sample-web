namespace Sample.Api.Middleware
{
    using Microsoft.AspNetCore.Builder;

    public static class ApplicationBuilderExtension
    {
        public static IApplicationBuilder UseRequestResponseLogger(
            this IApplicationBuilder app
        )
        {
            return app.UseMiddleware<RequestResponseLoggingMiddleware>();
        }
    }
}
