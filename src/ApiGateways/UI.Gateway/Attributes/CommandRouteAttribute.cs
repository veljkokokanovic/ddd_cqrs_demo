using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ActionConstraints;

namespace UI.Gateway.Attributes
{
    public class CommandRouteAttribute : RouteAttribute, IActionConstraintFactory
    {
        private readonly IActionConstraint _constraint;

        public CommandRouteAttribute(string template)
            : base(template)
            => _constraint = new CommandConstraint();

        public bool IsReusable => true;

        public IActionConstraint CreateInstance(IServiceProvider services)
            => _constraint;
    }
}
