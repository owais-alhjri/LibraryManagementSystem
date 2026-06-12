using LMS.Application.Common.Exceptions;
using Microsoft.AspNetCore.Mvc;

namespace LMS.API.Middleware
{
    public sealed class ExceptionHandlingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ExceptionHandlingMiddleware> _logger;

        public ExceptionHandlingMiddleware(
            RequestDelegate next,
            ILogger<ExceptionHandlingMiddleware> logger)
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
            catch (AppException ex)
            {
                _logger.LogWarning(ex, "Handled application exception");
                await HandleAppExceptionAsync(context, ex);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unhandled exception occurred");
                await HandleUnhandledExceptionAsync(context, ex);
            }
        }

        private static async Task HandleAppExceptionAsync(
            HttpContext context,
            AppException exception)
        {
            var statusCode = exception switch
            {
                NotFoundException => StatusCodes.Status404NotFound,
                ValidationException => StatusCodes.Status400BadRequest,
                ConflictException => StatusCodes.Status409Conflict,
                _ => StatusCodes.Status400BadRequest
            };

            var problem = new ProblemDetails
            {
                Type = $"https://httpstatuses.com/{statusCode}",
                Title = statusCode switch
                {
                    StatusCodes.Status404NotFound => "Resource not found",
                    StatusCodes.Status400BadRequest => "Validation error",
                    StatusCodes.Status409Conflict => "Conflict",
                    _ => "Application error"
                },
                Status = statusCode,
                Detail = exception.Message,
                Instance = context.Request.Path
            };

            context.Response.StatusCode = statusCode;
            context.Response.ContentType = "application/problem+json";

            await context.Response.WriteAsJsonAsync(problem);
        }

        private static async Task HandleUnhandledExceptionAsync(
            HttpContext context,
            Exception exception)
        {
            var env = context.RequestServices.GetRequiredService<IHostEnvironment>();

            var problem = new ProblemDetails
            {
                Type = "https://httpstatuses.com/500",
                Title = "Internal Server Error",
                Status = StatusCodes.Status500InternalServerError,
                Detail = env.IsDevelopment()
                    ? exception.Message
                    : "An unexpected error occurred.",
                Instance = context.Request.Path
            };

            context.Response.StatusCode = StatusCodes.Status500InternalServerError;
            context.Response.ContentType = "application/problem+json";

            await context.Response.WriteAsJsonAsync(problem);
        }
    }
}
