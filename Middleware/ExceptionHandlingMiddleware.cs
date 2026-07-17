using System.Net;
using System.Text.Json;

namespace IndurFarmsAPI.Middleware
{
    public class ExceptionHandlingMiddleware
    {
        private readonly RequestDelegate _requestDelegate;
        private readonly ILogger<ExceptionHandlingMiddleware> _logger;

        public ExceptionHandlingMiddleware(RequestDelegate requestDelegate, ILogger<ExceptionHandlingMiddleware> logger)
        {
            _requestDelegate = requestDelegate;
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext httpContext)
        {
            try
            {
                await _requestDelegate(httpContext);
            }
            catch(Exception ex)
            {
                _logger.LogError(
                    ex,
                    "Unhandled exception occurred. TraceId: {TraceId}",
                    httpContext.TraceIdentifier);

                httpContext.Response.ContentType = "application/json";
                httpContext.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                var response = new
                {
                    Message = "An unexpected error occurred.",
                    TraceId = httpContext.TraceIdentifier
                };

                await httpContext.Response.WriteAsync(
                    JsonSerializer.Serialize(response));
            }
        }
    }
}
