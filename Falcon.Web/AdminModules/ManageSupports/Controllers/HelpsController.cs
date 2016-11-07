using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Falcon.Services.Helps;
using Falcon.Data.Domain;
using Falcon.Modules.Helps.Models;
using Falcon.Modules.Helps.Helpers;
using Falcon.Themes;
using Falcon.Mvc.Controllers;
using Falcon.Common;

namespace Falcon.Modules.Helps.Controllers
{
    [Layout("Help")]
    public class HelpsController : BaseController
    {
        private readonly IHelpArticleService _helpArticleService;
        private readonly IHelpCategoryService _helpCategoryService;

        public HelpsController(IHelpArticleService helpArticleService, IHelpCategoryService helpCategoryService)
        {
            _helpArticleService = helpArticleService;
            _helpCategoryService = helpCategoryService;
        }

        public ActionResult Index()
        {
            var lstArticle = _helpArticleService.GetAllHelpArticle(1,14);
            Title = "Chăm sóc Khách hàng - Hangtot.com ";
            MetaDescription = "Trung tâm trợ giúp, chăm sóc Khách hàng - Hướng dẫn sử dụng, quy định, FAQ - Sàn giao dịch Thương mại điện tử Hangtot.com";
            ViewData["DataBreadcrumb"] = new HelpArticleSearchModel
            {
                ListHelpCategories = new List<HelpCategory>(),
                Article = null
            };
            ViewData["currentCateId"] = 0;
            return View(lstArticle);
        }

        public ActionResult TopHelps(int? cateId)
        {
            var helpArticles = _helpArticleService.GetHelpArticleByCateId(Convert.ToInt32(cateId), 1, 2);
            return View(helpArticles);
        }

        public ActionResult ListHelpArticlesByCate(int categoryId)
        {
            var helpCategory = _helpCategoryService.GetHelpCategoryById(categoryId);
            if (helpCategory == null || helpCategory.Status == HelpCategoryStatusConst.InActive) {
                return RedirectToAction("Index");
            }
            
            var helpCategoryModel = helpCategory.ToModel();
            if (!helpCategory.HasChild)
            {
                helpCategoryModel.ListHelpArticle = _helpArticleService.GetHelpArticleByCateId(categoryId,1,1000).ToList(); //viet lai method
            }
            else 
            {
                var helpCategories = _helpCategoryService.GetAllHelpCategoryByParentId(categoryId);
                var listHelpCates = new List<HelpCategoryModel>();
                foreach (var helpCate in helpCategories)
                {
                    if (helpCate.Status == HelpCategoryStatusConst.Active)
                    {
                        var helpCateModel = helpCate.ToModel();
                        helpCateModel.ListHelpArticle = _helpArticleService.GetHelpArticleByCateId(helpCate.Id, 1, 5).ToList();
                        listHelpCates.Add(helpCateModel);
                    }
                }
                helpCategoryModel.SubCategory = listHelpCates;
            }

            ViewData["DataBreadcrumb"] = new HelpArticleSearchModel { 
                ListHelpCategories = HelpHelper.GetBreadCrumb(categoryId),
                Article = null
            };
            ViewData["currentCateId"] = categoryId;

            Title = helpCategoryModel.Name + " - Chăm sóc Khách hàng - Hangtot.com ";
            MetaDescription = "Trung tâm trợ giúp, chăm sóc Khách hàng - " + helpCategoryModel.Name + " - Sàn giao dịch Thương mại điện tử Hangtot.com";
             
            return View(helpCategoryModel);
        }


    }
}
