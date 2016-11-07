using System.Web.Mvc;

namespace Falcon.Modules.Home
{
    public class HomeAreaRegistration : AreaRegistration
    {
        public override string AreaName
        {
            get
            {
                return "Home";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context)
        {
            context.MapRoute(
                "Home_default",
                "Home/{controller}/{action}/{id}",
                new { action = "Index", id = UrlParameter.Optional }
            );
            context.MapRoute(
               "CreateSupport",
               "CreateSupport.html",
               new { controller = "Home", action = "CreateSupport" }
           );
            context.MapRoute(
               "Login",
               "Login.html",
               new { controller = "Home", action = "Login" }
           );
            context.MapRoute(
                "Register",
                "Register.html",
             new { controller = "Home", action = "Register" }
           ); 
            context.MapRoute(
                 "CheckStatusSupport",
                  "CheckStatusSupport.html",
            new { controller = "Home", action = "CheckStatusSupport" }
           );
        }
    }
}
