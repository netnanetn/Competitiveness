using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Mvc.Html;

using Falcon.Mvc.Controllers;
using Falcon.Security;

using Falcon.Services.Pages;
using Falcon.Data.Domain;
using Falcon.Admin.Modules.ManageSupports.Models;
using Falcon.UI.Html.Admin;
using Falcon.Services.Supports;
using Falcon.Common.UI;
using Falcon.Common;
using System.IO;
//using StackExchange.Profiling;

namespace Falcon.Admin.Modules.Contents.Controllers
{
    public class ArticlesController : AdminBaseController
    {
        private readonly ICategoryService _categoryService;
        private readonly IArticleService _articleService;
        private const string _rootDir = "~/Upload/Video/";

        public ArticlesController(ICategoryService categoryService, IArticleService articleService)
        {
            _categoryService = categoryService;
            _articleService = articleService;
        }
        public ActionResult Index(ListArticleModel model)
        {
            var pageSize = 5;
            //valid Keyword
            model.Keyword = string.IsNullOrEmpty(model.Keyword) ? "" : model.Keyword.Trim();
            bool? status = null;
            if (model.Status == ArticleStatusConst.Active)
            {
                status = true;
            }
            else if (model.Status == ArticleStatusConst.InActive)
            {
                status = false;
            }

            //Page
            var totalRecord = _articleService.FilterArticleCount(model.Keyword, status, model.CategoryId);
            var totalPage = (totalRecord - 1) / pageSize + 1;

            if (model.Page < 1 || model.Page > totalPage)
            {
                return RedirectToAction("Index", new { categoryId = model.CategoryId });
            }

            model.PagerModel = new PaginationModels
            {
                PageNumber = model.Page,
                PageSize = pageSize,
                TotalRecords = totalRecord
            };

            model.ListCategories = _categoryService.GetActiveCategory();
            model.ListArticles = _articleService.FilterArticle(model.Keyword, status, model.CategoryId, model.Page, pageSize).ToList();
            //var category = _categoryService.GetCategoryById(model.CategoryId);
            //if (category != null)
            //{
            //    model.CategoryTypeId = category.CategoryTypeId;
            //}
            model.ListStatus = new SelectList(new[]
            {
                new SelectListItem{ Text = "Hiển thị", Value = ArticleStatusConst.Active.ToString() },
                new SelectListItem{ Text = "Ẩn", Value = ArticleStatusConst.InActive.ToString() }
            }, "Value", "Text", model.Status);

            Title = "Danh sách bìa viết trợ giúp";
            ViewData["ToolbarTitle"] = Title;
            return View(model);
        }

        [HttpGet]
        public ActionResult Create(int categoryId = 0)
        {
            Title = "Thêm mới bài viết trợ giúp";
            ViewData["ToolbarTitle"] = Title;
            var model = new ArticleModel()
            {
                ListCategories = _categoryService.GetAllCategory().ToList(),

            };
            var category = _categoryService.GetCategoryById(categoryId);
            if (category != null)
            {
                model.CategoryId = categoryId;

            }

            return View(model);
        }

        [HttpPost]
        public ActionResult Create(ArticleModel model, HttpPostedFileBase datafile)
        {
            string command = Request.Form["submit"].ToString();
            switch (command)
            {
                case "Save":
                case "SaveAndContinueEdit":
                    var alias = Util.GetSEOAlias(model.Name);
                    var checkArticle = _articleService.GetArticleByAlias(alias, model.CategoryId);
                    if (checkArticle != null)
                    {
                        ModelState.AddModelError("Name", "Alias đã được sử dụng");
                    }
                    if (ModelState.IsValid)
                    {
                        if (model.CategoryId > 0)
                        {
                            var category = _categoryService.GetCategoryById(model.CategoryId);
                            if (category == null)
                            {
                                ErrorNotification("Danh mục không tồn tại.");
                                return View(model);
                            }
                        }
                        var now = DateTime.Now;
                        if (datafile != null)
                        {
                            string[] fileExtensions = { ".jpg", ".jpeg", ".gif", ".png" };
                            string extension = Path.GetExtension(datafile.FileName).ToLower();
                            if (fileExtensions.Contains(extension))
                            {
                                string currentDir = _rootDir;
                                var filePath = _rootDir + datafile.FileName;
                                if (!Directory.Exists(Server.MapPath(currentDir)))
                                {
                                    Directory.CreateDirectory(Server.MapPath(currentDir));
                                }
                                var path = Path.Combine(Server.MapPath(currentDir), Path.GetFileName(datafile.FileName));
                                datafile.SaveAs(path);
                                model.ImgPath = filePath;
                            }
                        }
                        if (model.Description == null) model.Description = "";
                        try
                        {
                            var article = model.ToEntity();
                            article.CategoryId = model.CategoryId;
                            article.Name = model.Name;
                            article.CreatedOn = now;
                            article.ModifiedOn = now;
                            article.Content = model.Content;
                            article.Status = model.Status;
                            article.Description = model.Description;
                            article.ImgPath = model.ImgPath;
                            article.MetaTitle = model.MetaTitle;
                            article.MetaDescription = model.MetaDescription;
                            article.Alias = Util.GetSEOAlias(model.Name);
                            model.IsHighlight = model.IsHighlight;
                            _articleService.AddArticle(article);

                            SuccessNotification("Thêm mới bài viết thành công!");
                            if (command == "SaveAndContinueEdit")
                            {
                                Title = "Thêm mới bài viết : " + article.Name;
                            }
                            else
                            {
                                return RedirectToAction("Index");
                            }

                        }
                        catch (Exception e)
                        {
                            ErrorNotification(e.ToString());
                        }
                    }
                    else
                    {
                        AddModelStateErrors();
                    }
                    break;
                default:
                    ErrorNotification("Không rõ phương thức submit dữ liệu");
                    return RedirectToAction("Index");
            }
            model.ListCategories = _categoryService.GetAllCategory().ToList();

            Title = "Thêm mới danh mục";
            ViewData["ToolbarTitle"] = Title;
            return View(model);
        }

        public ActionResult Edit(int id)
        {
            var article = _articleService.GetArticleById(id);
            if (article == null)
            {
                ErrorNotification("Không tìm thấy bài viết nào thỏa mãn");
                return RedirectToAction("ListArticle");
            }

            var model = article.ToModel();

            model.ListCategories = _categoryService.GetAllCategory().ToList();

            Title = "Chỉnh sửa bài viết";
            ViewData["ToolbarTitle"] = Title;
            return View(model);
        }
        [HttpPost]
        public ActionResult Edit(ArticleModel model, HttpPostedFileBase datafile)
        {
            string command = Request.Form["submit"].ToString();
            switch (command)
            {
                case "Save":
                case "SaveAndContinueEdit":
                    if (ModelState.IsValid)
                    {
                        try
                        {
                            var article = _articleService.GetArticleById(model.Id);
                            if (article == null)
                            {
                                ErrorNotification("Không tìm thấy bài viết nào thỏa mãn");
                                return RedirectToAction("Index", new { categoryId = model.CategoryId });
                            }
                            if (datafile != null)
                            {
                                string[] fileExtensions = { ".jpg", ".jpeg", ".gif", ".png" };
                                string extension = Path.GetExtension(datafile.FileName).ToLower();
                                if (fileExtensions.Contains(extension))
                                {
                                    string currentDir = _rootDir;
                                    var filePath = _rootDir + datafile.FileName;
                                    if (!Directory.Exists(Server.MapPath(currentDir)))
                                    {
                                        Directory.CreateDirectory(Server.MapPath(currentDir));
                                    }
                                    var path = Path.Combine(Server.MapPath(currentDir), Path.GetFileName(datafile.FileName));
                                    datafile.SaveAs(path);
                                    model.ImgPath = filePath;
                                }
                            }
                            if (model.Description == null) model.Description = "";
                            article.CategoryId = model.CategoryId;

                            article.Name = model.Name;
                            article.Description = model.Description;
                            article.ImgPath = model.ImgPath;
                            article.IsHighligh = model.IsHighlight;
                            article.ModifiedOn = DateTime.Now;
                            article.Content = model.Content;
                            article.Status = model.Status;
                            article.MetaTitle = model.MetaTitle;
                            article.MetaDescription = model.MetaDescription;
                            article.Alias = model.Alias;

                            _articleService.UpdateArticle(article);
                            SuccessNotification("Cập nhật thông tin thành công");

                            if (command == "SaveAndContinueEdit")
                            {
                                Title = "Chỉnh sửa bài viết : " + article.Name;
                            }
                            else
                            {
                                return RedirectToAction("Index");
                            }

                        }
                        catch (Exception e)
                        {
                            Title = "Chỉnh sửa danh mục hỗ trợ";
                            ErrorNotification(e.ToString());
                        }
                    }

                    break;
                case "Delete":
                    return RedirectToAction("Delete", new { id = model.Id });

                default:
                    ErrorNotification("Không rõ phương thức submit dữ liệu");
                    return RedirectToAction("Index");

            }

            model.ListCategories = _categoryService.GetAllCategory().ToList();

            ViewData["ToolbarTitle"] = Title;
            return View(model);
        }
        public ActionResult Delete(int id)
        {

            var article = _articleService.GetArticleById(id);

            if (article == null)
            {
                ErrorNotification("Không tìm thấy bài viết nào thỏa mãn");
                return RedirectToAction("Index");
            }

            Title = "Xóa bài viết: " + article.Name;
            ViewData["ToolbarTitle"] = Title;
            return View(article);
        }

        [HttpPost]
        public ActionResult Delete(int id, FormCollection collection)
        {
            Title = "Xóa bài viết hỗ trợ: ";
            string command = Request.Form["submit"].ToString();
            switch (command)
            {
                case "Delete":
                    var article = _articleService.GetArticleById(id);
                    if (article == null)
                    {
                        ErrorNotification("Không tìm thấy bài viết nào thỏa mãn");
                        return Redirect("/admin");
                    }

                    Title += article.Name;
                    _articleService.RemoveArticle(article);

                    SuccessNotification("Xóa bài viết hỗ trợ thành công");
                    return RedirectToAction("Index", new { categoryId = article.CategoryId });
                default:
                    ErrorNotification("Không rõ phương thức submit dữ liệu");
                    return RedirectToAction("Index");
            }
        }
    }
}
