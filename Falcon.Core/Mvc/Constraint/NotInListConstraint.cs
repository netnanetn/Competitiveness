using System;
using System.Web;
using System.Web.Routing;
using System.Collections;

namespace Falcon.Mvc
{
    public class NotInListConstraint : IRouteConstraint
    {
        private string[] _values;        

        public NotInListConstraint(params string[] values)
        {
            _values = values;
        }

        public bool Match(HttpContextBase httpContext, Route route, string parameterName, RouteValueDictionary values, RouteDirection routeDirection)
        {
            string value = values[parameterName].ToString();
            foreach (string item in _values)
            {
                if (item.Equals(value, StringComparison.OrdinalIgnoreCase))
                {
                    return false;
                }
            }
            return true;
        }
    }
}