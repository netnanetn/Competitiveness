using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Falcon.Services.Helps;
using Falcon.Data.Domain;
using Falcon.Modules.Helps.Models;
using Falcon.Common;
using Falcon.Mvc.Controllers;
using Falcon.Common.UI;
using Falcon.Themes;
using Falcon.Modules.Helps.Helpers;

namespace Falcon.Modules.Helps.Controllers
{
    [Layout("Help")]
    public class ArticleController : BaseController
    {
        private const int PAGESIZE = 10;
        private readonly IHelpArticleService _helpArticleService;
        private readonly IHelpCategoryService _helpCategoryService;

        public ArticleController(IHelpArticleService helpArticleService, IHelpCategoryService helpCategoryService)
        {
            _helpArticleService = helpArticleService;
            _helpCategoryService = helpCategoryService;
        }

        //
        // GET: /Help/
        public ActionResult Index(int? cateId, int page = 1)
        {
            Title = "Hỗ trợ khách hàng";
            int total = 0;
            IEnumerable<HelpArticle> helpArticles = null;
            if (cateId == null)
            {
                total = _helpArticleService.GetAllHelpArticleCount();
                helpArticles = _helpArticleService.GetAllHelpArticle(page, PAGESIZE);
            }
            else
            {
                int cId = Convert.ToInt32(cateId);
                total = _helpArticleService.GetHelpArticleByCateIdCount(cId);
                helpArticles = _helpArticleService.GetHelpArticleByCateId(cId, page, PAGESIZE);
            }
            var model = new StaticModel
            {
                ListArticle = helpArticles,
                PagerModels = new PaginationModels
                {
                    PageNumber = page,
                    PageSize = PAGESIZE,
                    TotalRecords = total
                }
            };
            return View(model);
        }

        public ActionResult TopHelp()
        {
            IEnumerable<HelpArticle> helpArticles = _helpArticleService.GetAllHelpArticle(1, 7);
            return View(helpArticles);
        }

        //
        // GET: /Help/Details/5
        public ActionResult Details(int id, int? categoryId, string alias)
        {
            var helpArticle = _helpArticleService.GetHelpArticleById(id);
            if (helpArticle == null || helpArticle.Status == HelpArticleStatusConst.InActive)
            {
                //ErrorNotification("Không tìm thấy bài viết nào thỏa mãn");
                return RedirectToAction("Index", "Helps");
            }

            if (categoryId == null || !helpArticle.AliasTitle.Equals(alias, StringComparison.InvariantCultureIgnoreCase) || categoryId != helpArticle.CategoryId)
            {
                return RedirectToActionPermanent("Details", new { id = id, categoryId = helpArticle.CategoryId, alias = helpArticle.AliasTitle });
            }

            var title = helpArticle.Title;
            var helpCategory = _helpCategoryService.GetHelpCategoryById((int)categoryId);
            if (helpCategory != null)
            {
                title += " - " + helpCategory.Name;
            }
            Title = title + " - Trung tâm trợ giúp, chăm sóc Khách hàng - Hangtot.com";
            CanonicalLink = FalconConfig.DomainName + Url.Action("Details", new { id = id, title = helpArticle.AliasTitle });
            MetaKeyword = helpArticle.MetaKeyword + "," + MetaKeyword;
            MetaDescription = helpArticle.MetaDescription;

            var model = new HelpArticleDetailModel()
            {
                Article = helpArticle,
                OtherArticles = _helpArticleService.GetHelpArticleByCateId((int)categoryId, 1, 500).Where(a => a.Id != id).ToList()
            };

            ViewData["DataBreadcrumb"] = new HelpArticleSearchModel
            {
                ListHelpCategories = HelpHelper.GetBreadCrumb(helpArticle.CategoryId),
                Article = helpArticle
            };

            ViewData["currentCateId"] = helpArticle.CategoryId;
            return View(model);
        }

        public ActionResult SearchArticle(string keyword = "")
        {
            keyword = keyword.Trim();
            Title = "Kết quả tìm kiếm với từ khóa '" + keyword + "' - Chăm sóc Khách hàng - Hangtot.com ";
            MetaDescription = "Trung tâm trợ giúp, chăm sóc Khách hàng - Tìm kiếm trợ giúp - Sàn giao dịch Thương mại điện tử Hangtot.com";
            if (!string.IsNullOrEmpty(keyword.Trim()))
            {
                var lstHelpSearch = _helpArticleService.SearchHelpArticleByTitle(keyword).ToList();
                var listHelpArticle = new List<HelpArticleSearchModel>();
                ViewBag.KeyWord = keyword;
                if(lstHelpSearch !=null && lstHelpSearch.Count > 0){
                    foreach (HelpArticle article in lstHelpSearch)
                    {
                        var helpArticleSearchModel = new HelpArticleSearchModel();
                        helpArticleSearchModel.Article = article;
                        helpArticleSearchModel.ListHelpCategories = HelpHelper.GetBreadCrumb(article.CategoryId);
                        listHelpArticle.Add(helpArticleSearchModel);
                    }

                }

                ViewData["DataBreadcrumb"] = new HelpArticleSearchModel
                {
                    ListHelpCategories = new List<HelpCategory>(),
                    Article = null
                };
                ViewData["currentCateId"] = 0;
                return View(listHelpArticle);
            }
            return RedirectToAction("Index");
        }

    }
}
