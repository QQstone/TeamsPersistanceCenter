using System;
using TeamsPersistanceCenter.Api.Infrastructure.Helpers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.AspNetCore.OData.Query;

namespace TeamsPersistanceCenter.Api.Infrastructure.ActionFilters
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
    public sealed class EnableQueryIfSuccessAttribute:EnableQueryAttribute
    {
        public override void OnActionExecuted(ActionExecutedContext actionExecutedContext)
        {
            // Skip OData processing if this is a failure response.
            if (actionExecutedContext.Result is IStatusCodeActionResult result && !HttpStatusCodeHelpers.IsSuccessStatusCode(result.StatusCode))
            {
                // Convert `string` responses to `ProblemDetail` responses.
                if (actionExecutedContext.Result is ObjectResult objectResult && objectResult.Value is string value)
                {
                    actionExecutedContext.Result =
                        ((ControllerBase)actionExecutedContext.Controller).Problem(value, null, result.StatusCode);
                }
            }
            else
                base.OnActionExecuted(actionExecutedContext);
        }
    }
}
