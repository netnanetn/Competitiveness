using System.Web;
using System.Web.Mvc;
using Falcon.Infrastructure;
using Falcon.Security;

namespace Falcon.Mvc.Controllers
{
    public class AdminAuthorizeAttribute : AuthorizeAttribute
    {
        protected override bool AuthorizeCore(HttpContextBase httpContext)
        {

            return false;

            //previous implementation
            //var workContext = EngineContext.Current.Resolve<IWorkContext>();
            //var user = workContext.CurrentCustomer;
            //bool result = user != null && user.IsAdmin();
            //return result;
        }
    }
}
