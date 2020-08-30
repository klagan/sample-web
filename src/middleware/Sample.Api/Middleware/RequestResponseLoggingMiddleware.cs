namespace Sample.Api.Middleware
{
    using System;
    using System.IO;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Http;
    using Microsoft.Extensions.Logging;

    public class RequestResponseLoggingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger _logger;

        public RequestResponseLoggingMiddleware(RequestDelegate next, ILogger<RequestResponseLoggingMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task Invoke(HttpContext context)
        {
            // first, get the incoming request
            var request = await FormatRequest(context.Request);

            // copy a pointer to the original response body stream
            var originalBodyStream = context.Response.Body;

            // create a new memory stream...
            await using var responseBody = new MemoryStream();

            // and use that for the temporary response body
            context.Response.Body = responseBody;

            // continue down the middleware pipeline, eventually returning to this class
            await _next(context);

            // format the response from the server
            var response = await FormatResponse(context.Response);

            _logger.LogDebug(request);
            _logger.LogDebug(response);

            // copy the contents of the new memory stream (which contains the response) to the original stream, which is then returned to the client.
            await responseBody.CopyToAsync(originalBodyStream);
        }

        private async Task<string> FormatRequest(HttpRequest request)
        {
            var body = request.Body;

            // this line allows us to set the reader for the request back at the beginning of its stream.
            request.EnableBuffering();

            // we now need to read the request stream.  First, we create a new byte[] with the same length as the request stream...
            var buffer = new byte[Convert.ToInt32(request.ContentLength)];

            // then we copy the entire request stream into the new buffer.
            await request.Body.ReadAsync(buffer, 0, buffer.Length);

            // we convert the byte[] into a string using UTF8 encoding...
            var bodyAsText = Encoding.UTF8.GetString(buffer);

            var header = (from h in request.Headers from v in h.Value select $"{h.Key} => {v}").ToList();

            // and finally, assign the read body back to the request body, which is allowed because of EnableRewind() (2.2) /EnableBuffering (3.0)
            request.Body = body;

            return $"\nRequest:\n{request.Scheme} {request.Host}{request.Path} {request.QueryString} {bodyAsText}\nHeaders:\n{string.Join("\n", header.ToArray())}";
        }

        private async Task<string> FormatResponse(HttpResponse response)
        {
            // we need to read the response stream from the beginning...
            response.Body.Seek(0, SeekOrigin.Begin);

            // and copy it into a string
            string text = await new StreamReader(response.Body).ReadToEndAsync();

            // we need to reset the reader for the response so that the client can read it.
            response.Body.Seek(0, SeekOrigin.Begin);

            // return the string for the response, including the status code (e.g. 200, 404, 401, etc.)
            return $"{response.StatusCode}: {text}";
        }
    }
}
