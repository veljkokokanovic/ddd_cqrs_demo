using domainD;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Primitives;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace UI.Gateway.Attributes
{
    public class EstablishCommandContextAttribute : ActionFilterAttribute
    {
        public override Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            OperationContext.CorrelationId = Guid.NewGuid();
            OperationContext.UserId = Constants.DefaultUserId;

            context.HttpContext.Response.Headers.Add(
                new KeyValuePair<string, StringValues>(KnownHeaders.CorrelationId, new StringValues(OperationContext.CorrelationId.ToString())));
            return next();
        }
    }
}
