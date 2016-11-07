using System.Web.Mvc;
using Falcon.Mvc;

namespace Falcon.Admin.Modules.Contents
{
    public class ContentsAreaRegistration : AreaRegistration
    {
        public override string AreaName
        {
            get
            {
                return "Contents";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context)
        {
            context.MapRoute(
                "pages_default",
                "admin/pages/{action}/{id}",
                new { controller = "Pages", action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}
