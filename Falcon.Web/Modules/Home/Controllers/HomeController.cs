using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

using Falcon.Mvc.Controllers;
using Falcon.Infrastructure;
using Falcon.Services.Supports;
using Falcon.Modules.Home.Models;
using Falcon.Common;
using Common.Logging;
using Falcon.Data.Domain;
using Falcon.Common.UI;
using System.Collections.ObjectModel;
using System.IO;

namespace Falcon.Modules.Home.Controllers
{
    public class HomeController : BaseController
    {
       
        //private readonly IArticleService _articleService;
        //private readonly ICategoryTypeService _categoryTypeService;
        //private readonly ICategoryService _categoryService;
        //private readonly ISupportTypeService _supportTypeService;
        //private readonly ILog _logger = LogManager.GetCurrentClassLogger();
        //private readonly int _PageSize = 9;

        //public HomeController(IArticleService articleService, ICategoryTypeService categoryTypeService, ICategoryService categoryService, ISupportTypeService supportTypeService)
        //{
        //    _articleService = articleService;
        //    _categoryTypeService = categoryTypeService;
        //    _categoryService = categoryService;
        //    _supportTypeService = supportTypeService;
        //}
        //
        // GET: /Home/Home/       
        [Layout("Home")]
        public ActionResult Index()
        {
            // CreateTicketWithAttachment fr = new CreateTicketWithAttachment();
            //fr.CreateTicketFreshdesk();

            Title = "Tài liệu hướng dẫn";
            MetaDescription = "Tài liệu hướng dẫn";
            CanonicalLink = FalconConfig.DomainName + Url.Action("Index");
            ViewData["CSS"] = "default";
            return View();
        }
        [Layout("Home")]
        public ActionResult Comparetitiveness()
        {
            Title = "Chào mừng bạn đến với Trung tâm trợ giúp khách hàng Bizweb";
            MetaDescription = "Tài liệu hướng dẫn";
            CanonicalLink = FalconConfig.DomainName + Url.Action("CreateSupport");
            ViewData["CSS"] = "default";
            return View();
        }
        [Layout("Home")]
        public ActionResult BuildingData()
        {
            Title = "Xây dựng bộ dữ liệu và trọng số";
            MetaDescription = "Xây dựng dữ liệu";
            CanonicalLink = FalconConfig.DomainName + Url.Action("BuildingData");
            ViewData["CSS"] = "default";
            return View();
        }
    }
}
