using CardMon.Core.Interfaces.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Linq;

namespace CardMon.Infrastructure.Utilities.Filters
{
    [AttributeUsage(validOn: AttributeTargets.Method)]
    public class RequestValidationAttribute : Attribute, IActionFilter
    {
        private readonly ILoggerManager _logger;

        public RequestValidationAttribute(ILoggerManager logger) =>
            _logger = logger;

        public void OnActionExecuting(ActionExecutingContext context)
        {
            var action = context.RouteData.Values["action"];
            var controller = context.RouteData.Values["controller"];
            var param = context.ActionArguments.SingleOrDefault(x => x.Value.ToString()
            .Contains("Request")).Value;
            if (param == null)
            {
                _logger.LogError($"Request from client is null. Controller: {controller}," +
                    $" Action: {action}");
                context.Result = new BadRequestObjectResult($"Request can't be null. " +
                    $"Controller: {controller}, Action: {action}");
                return;
            }

            if (!context.ModelState.IsValid)
            {
                _logger.LogError($"Invalid request object model state. Controller: " +
                    $"{controller}, Action: {action}");
                context.Result = new UnprocessableEntityObjectResult(context.ModelState);
            }
        }

        public void OnActionExecuted(ActionExecutedContext context) { }
    }
}
