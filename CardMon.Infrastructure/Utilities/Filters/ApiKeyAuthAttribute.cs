using CardMon.Core.Interfaces.Repositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Threading.Tasks;

namespace CardMon.Infrastructure.Utilities.Filters
{
    [AttributeUsage(validOn: AttributeTargets.Class | AttributeTargets.Method)]
    public class ApiKeyAuthAttribute : Attribute, IAsyncActionFilter
    {
        private const string apiKeyName = "X-Api-Key";
        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            if (!context.HttpContext.Request.Headers.TryGetValue(apiKeyName, out var apiKeyValue))
            {
                context.Result = new ContentResult
                {
                    StatusCode = 401,
                    Content = "Api Key not provided."
                };
                return;
            }

            var repositoryManager = context.HttpContext.RequestServices.GetRequiredService<IRepositoryManager>();
            var client = await repositoryManager.ClientRepository.GetClientAsync(apiKeyValue, trackChanges: false);
            if (client == null)
            {
                context.Result = new ContentResult
                {
                    StatusCode = 403,
                    Content = "Client is unauthorized."
                };
                return;
            }
            context.HttpContext.Items.Add("apiKey", client);
            await next();
        }
    }
}
