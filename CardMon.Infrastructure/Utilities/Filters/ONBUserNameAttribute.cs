using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Threading.Tasks;

namespace CardMon.Infrastructure.Utilities.Filters
{
    [AttributeUsage(validOn: AttributeTargets.Method)]
    public class ONBUserNameAttribute : Attribute, IAsyncActionFilter
    {
        private const string username = "username";
        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            if (!context.HttpContext.Request.Headers.TryGetValue(username, out var usernameValue))
            {
                context.Result = new ContentResult
                {
                    StatusCode = 401,
                    Content = "Security header violation."
                };
                return;
            }
            context.HttpContext.Items.Add("username", usernameValue);
            await next();
        }
    }
}
