using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CustomerApi.Controllers
{
    /// <summary>
    /// Attribute for validating the model state before an action is executed.
    /// </summary>
    public class ValidateModelAttribute : IAsyncActionFilter
    {
        /// <summary>
        /// Called before and after the action executes.
        /// </summary>
        /// <param name="context">The action executing context.</param>
        /// <param name="next">The delegate to execute the next action filter or the action itself.</param>
        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            // Check if the model state is valid
            if (!context.ModelState.IsValid)
            {
                // Extract error messages from the model state
                var errors = context.ModelState.Values.SelectMany(
                    o => o.Errors.Select(
                        e => e.ErrorMessage));

                // Set the result to a BadRequest with custom error result
                context.Result = new BadRequestObjectResult(new CustomErrorResult
                {
                    Succeeded = false,
                    Errors = errors.ToList()
                });

                // Do not proceed to the next action filter or action
                return;
            }

            // Proceed to the next action filter or action
            await next();
        }
    }

    /// <summary>
    /// Represents the result of a failed validation.
    /// </summary>
    public class CustomErrorResult
    {
        /// <summary>
        /// Gets or sets a value indicating whether the request succeeded.
        /// </summary>
        public bool Succeeded { get; set; }

        /// <summary>
        /// Gets or sets the list of error messages.
        /// </summary>
        public IEnumerable<string>? Errors { get; set; }
    }
}