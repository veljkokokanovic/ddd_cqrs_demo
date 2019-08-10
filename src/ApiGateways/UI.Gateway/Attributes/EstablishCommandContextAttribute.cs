using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Primitives;

namespace UI.Gateway.Attributes
{
    public class EstablishCommandContextAttribute : ActionFilterAttribute
    {
        public override Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            context.HttpContext.Response.Headers.Add(new KeyValuePair<string, StringValues>(KnownHeaders.CorrelationId,
                new StringValues(Guid.NewGuid().ToString())));
            return next();
        }
    }
}
