using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace CustomerApi.Controllers
{
    public class ValidateModelAttribute : IAsyncActionFilter
    {
        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            if (!context.ModelState.IsValid)
            {
                var errors = context.ModelState.Values.SelectMany(
                    o => o.Errors.Select(
                        e => e.ErrorMessage));

                context.Result = new BadRequestObjectResult(new CustomErrorResult
                {
                    Succeeded = false,
                    Errors = errors.ToList()
                });

                return;
            }

            await next();
        }
    }

    public class CustomErrorResult
    {
        public bool Succeeded { get; set; }

        public IEnumerable<string> Errors { get; set; }
    }
}