using System.Text.Json;
using EmployeeAPI.Core.Models;

namespace EmployeeAPI.Middleware
{   
    public class ExceptionMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ExceptionMiddleware> _logger;

        public ExceptionMiddleware(
            RequestDelegate next,
            ILogger<ExceptionMiddleware> logger)
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
                _logger.LogError(ex, ex.Message);

                await HandleExceptionAsync(context, ex);
            }
        }

        private static async Task HandleExceptionAsync(
            HttpContext context,
            Exception exception)
        {
            context.Response.ContentType = "application/json";

            var response = exception switch
            {
                ConflictException => new
                {
                    StatusCode = StatusCodes.Status409Conflict,
                    Message = exception.Message
                },

                NotFoundException => new
                {
                    StatusCode = StatusCodes.Status404NotFound,
                    Message = exception.Message
                },

                ValidationException => new
                {
                    StatusCode = StatusCodes.Status400BadRequest,
                    Message = exception.Message
                },

                _ => new
                {
                    StatusCode = StatusCodes.Status500InternalServerError,
                    Message = "An unexpected error occurred."
                }
            };

            context.Response.StatusCode = response.StatusCode;

            await context.Response.WriteAsync(
                JsonSerializer.Serialize(response));
        }
    }
}