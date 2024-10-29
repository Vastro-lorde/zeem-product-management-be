using System.Net;
using System.Text.Json;

namespace ZeemProductManagementTest.Services
{
    public class GlobalExceptionHandlerMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<GlobalExceptionHandlerMiddleware> _logger;

        public GlobalExceptionHandlerMiddleware(RequestDelegate next, ILogger<GlobalExceptionHandlerMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An unhandled exception occurred.");
                await HandleExceptionAsync(context, ex);
            }
        }

        private static Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            // Set the status code
            context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
            context.Response.ContentType = "application/json";

            // Create a friendly error message
            var response = new
            {
                Message = "An unexpected error occurred. Please try again later.",
                Details = exception.Message // Optional: remove in production to avoid exposing details
            };

            // Serialize the response
            var responseContent = JsonSerializer.Serialize(response);
            return context.Response.WriteAsync(responseContent);
        }
    }
}
