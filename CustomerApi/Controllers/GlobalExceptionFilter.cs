using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace CustomerApi.Controllers
{
    /// <summary>
    /// Represents a global exception filter that handles unhandled exceptions in the application.
    /// </summary>
    public class GlobalExceptionFilter : IExceptionFilter
    {
        private readonly ILogger<GlobalExceptionFilter> _logger;

        /// <summary>
        /// Initializes a new instance of the <see cref="GlobalExceptionFilter"/> class.
        /// </summary>
        /// <param name="logger">The logger to log exceptions.</param>
        public GlobalExceptionFilter(ILogger<GlobalExceptionFilter> logger)
        {
            _logger = logger;
        }

        /// <summary>
        /// Called when an exception occurs in the application.
        /// </summary>
        /// <param name="context">The exception context.</param>
        public void OnException(ExceptionContext context)
        {
            var ex = context.Exception;
            _logger.LogError(ex, "An unhandled exception occurred: {Message}", ex.Message);

            context.Result = new ObjectResult(new 
            { 
                Error = "Internal Server Error",
                Details = ex.Message  // You can choose to include this or remove for more generic error message
            })
            {
                StatusCode = 500
            };
        }
    }
}