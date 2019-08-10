using Microsoft.AspNetCore.Mvc.Abstractions;
using Microsoft.AspNetCore.Mvc.ActionConstraints;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Primitives;
using Microsoft.Net.Http.Headers;
using System;
using System.Linq;
using System.Net.Http;
using System.Text.RegularExpressions;

namespace UI.Gateway.Attributes
{
    public class CommandConstraint : ActionMethodSelectorAttribute
    {
        public override bool IsValidForRequest(RouteContext routeContext, ActionDescriptor action)
        {
            var isCorrectHttpMethod = routeContext.HttpContext.Request.Method == HttpMethod.Post.Method;
            var foundContentType = routeContext.HttpContext.Request.Headers.TryGetValue(HeaderNames.ContentType, out var contentTypeValue);
            var foundModelName =TryExtractContentTypeDomainModelParameter(contentTypeValue, out var modelTypeName);
            var foundActionForModel = string.Equals(action.Parameters.SingleOrDefault()?.ParameterType.Name, modelTypeName, StringComparison.InvariantCultureIgnoreCase);
            return isCorrectHttpMethod && foundContentType && foundModelName && foundActionForModel;
        }

        private static bool TryExtractContentTypeDomainModelParameter(StringValues contentType, out string modelTypeName)
        {
            modelTypeName = string.Empty;

            var regex = new Regex(@"domain-model=(?<type>[\w]+);*", RegexOptions.Compiled);
            var match = regex.Match(contentType);

            if (match.Success && match.Groups["type"] != null)
            {
                modelTypeName = match.Groups["type"].Value;
            }

            return !string.IsNullOrWhiteSpace(modelTypeName);
        }
    }
}
