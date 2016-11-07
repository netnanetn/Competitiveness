using System.Web.Mvc;
using Falcon.Mvc;

namespace Falcon.Admin.Modules.Contents
{
    public class ManageSupportsAreaRegistration : AreaRegistration
    {
        public override string AreaName
        {
            get
            {
                return "ManageSupports";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context)
        {
            context.MapRoute(
                "sieuweb_category",
                "admin/categories/{action}/{id}",
                new { controller = "Categories", action = "Index", id = UrlParameter.Optional }
            );
            context.MapRoute(
                "sieuweb_article",
                "admin/articles/{action}/{id}",
                new { controller = "Articles", action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}
