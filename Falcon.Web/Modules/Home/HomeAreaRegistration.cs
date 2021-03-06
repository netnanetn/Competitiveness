﻿using System.Web.Mvc;

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
               "Comparetitiveness",
               "Comparetitiveness.html",
               new { controller = "Home", action = "Comparetitiveness" }
           );
              context.MapRoute(
               "BuildingData",
               "BuildingData.html",
               new { controller = "Home", action = "BuildingData" }
           );


        }
    }
}
