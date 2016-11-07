using System;
using System.Web;
using System.Web.Routing;

namespace Falcon.Mvc
{
    public class StartsWithConstraint : IRouteConstraint
    {
        private string _match = String.Empty;

        public StartsWithConstraint(string match)
        {
            _match = match;
        }

        public bool Match(HttpContextBase httpContext, Route route, string parameterName, RouteValueDictionary values, RouteDirection routeDirection)
        {
            return values[parameterName].ToString().StartsWith(_match);
        }
    }
}