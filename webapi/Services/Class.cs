using Microsoft.AspNetCore.Http;

namespace webapi.Services
{
    public class CustomResult : IResult
    {
        private readonly int _statusCode;
        private readonly string _location;

        public CustomResult(int statusCode, string location)
        {
            _statusCode = statusCode;
            _location = location;
        }

        public async Task ExecuteAsync(HttpContext context)
        {
            context.Response.StatusCode = _statusCode;
            context.Response.Headers["Location"] = _location;

            // If you want to include a response body, you can do so here.
            // Example: await context.Response.WriteAsync("Redirecting...");

            await Task.CompletedTask;
        }

    }
}
