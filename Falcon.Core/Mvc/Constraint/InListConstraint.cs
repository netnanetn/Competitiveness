using System;
using System.Web;
using System.Web.Routing;
using System.Collections;

namespace Falcon.Mvc
{
    public class InListConstraint : IRouteConstraint
    {
        private string[] _values;

        public InListConstraint(params string[] values)
        {
            _values = values;
        }

        public bool Match(HttpContextBase httpContext, Route route, string parameterName, RouteValueDictionary values, RouteDirection routeDirection)
        {
            string value = values[parameterName].ToString();
            return ((IList)_values).Contains(value);
        }
    }
}