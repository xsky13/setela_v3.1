using Microsoft.AspNetCore.Diagnostics;

namespace SetelaServerV3._1
{
    public class ExceptionHandler(ILogger<ExceptionHandler> _logger, IHostEnvironment _environment) : IExceptionHandler
    {
        public async ValueTask<bool> TryHandleAsync(HttpContext httpContext, Exception exception, CancellationToken cancellationToken)
        {
            httpContext.Response.StatusCode = 500;
            httpContext.Response.ContentType = "application/json";

            _logger.LogError(exception, "Unhandled exception occurred during request processing.");
            var isDevelopment = _environment.IsDevelopment();

            var errorResponse = new
            {
                StatusCode = httpContext.Response.StatusCode,
                Detail = isDevelopment ? exception.Message : "Un error inesperado ha ocurrido.",
                TraceId = httpContext.TraceIdentifier
            };

            await httpContext.Response.WriteAsJsonAsync(errorResponse, cancellationToken);

            return true;
        }
    }
}
