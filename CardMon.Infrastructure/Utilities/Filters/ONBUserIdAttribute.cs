using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Threading.Tasks;

namespace CardMon.Infrastructure.Utilities.Filters
{
    [AttributeUsage(validOn: AttributeTargets.Method)]
    public class ONBUserIdAttribute : Attribute, IAsyncActionFilter
    {
        private const string userId = "userId";
        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            if (!context.HttpContext.Request.Headers.TryGetValue(userId, out var userIdValue))
            {
                context.Result = new ContentResult
                {
                    StatusCode = 401,
                    Content = "Security header violation."
                };
                return;
            }
            context.HttpContext.Items.Add("userId", userIdValue);
            await next();
        }
    }
}
