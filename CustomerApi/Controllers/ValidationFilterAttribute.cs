using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Logging;
using System.Linq;
using System.Threading.Tasks;

namespace CustomerApi.Controllers
{
    /// <summary>
    /// Represents an attribute that validates the model state of an action before execution.
    /// </summary>
    public class ValidateModelAttribute : IAsyncActionFilter
    {
        private readonly ILogger<ValidateModelAttribute> _logger;

        public ValidateModelAttribute(ILogger<ValidateModelAttribute> logger)
        {
            _logger = logger;
        }

        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            if (!context.ModelState.IsValid)
            {
                var errors = context.ModelState.Values.SelectMany(
                    o => o.Errors.Select(
                        e => e.ErrorMessage)).ToList();

                _logger.LogWarning("Model state is invalid: {Errors}", string.Join(", ", errors));

                context.Result = new BadRequestObjectResult(new CustomErrorResult
                {
                    Succeeded = false,
                    Errors = errors
                });
                return;
            }
            await next();
        }
    }
}