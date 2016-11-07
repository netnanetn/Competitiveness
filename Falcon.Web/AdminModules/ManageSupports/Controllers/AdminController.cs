using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Falcon.Mvc.Controllers;
using Falcon.Data.Domain;
using Falcon.Modules.Helps.Models;
using Falcon.Services.Helps;
using Falcon.Common;
using Falcon.Services.Thumbnails;
using System.IO;
using Falcon.Common.UI;
using Falcon.Thrifts.Client;
using Falcon.Common.Extensions;

namespace Falcon.Modules.Helps.Controllers
{
    public class AdminController : AdminBaseController
    {
        private readonly IHelpArticleService _helpArticleService;
        private readonly IHelpCategoryService _helpCategoryService;
        private readonly IThumbnailSettingService _thumbnailSettingService;
        private readonly IThriftClient _thriftClient;
        private const string _rootDir = "~/Upload/";

        public AdminController(IHelpArticleService helpArticleService, IHelpCategoryService helpCategoryService, IThumbnailSettingService thumbnailSettingService, IThriftClient thriftClient)
        {
            _helpArticleService = helpArticleService;
            _helpCategoryService = helpCategoryService;
            _thumbnailSettingService = thumbnailSettingService;
            _thriftClient = thriftClient;
        }

        //
        // GET: /Admin/Categories/

        public ActionResult Index()
        {
            Title = "Quản trị danh mục Hỗ trợ khách hàng";
            ViewData["ToolbarTitle"] = Title;

            var categories = _helpCategoryService.Help_Categories_SelectTreeAll();
            return View(categories);
        }

        public ActionResult HelpArticles(HelpArticleListModel model)
        {
            var pageSize = 20;
            DateTime sDate;

            //valid StartDate
            sDate = string.IsNullOrEmpty(model.StartDate) ? new DateTime(2012, 01, 01) : Convert.ToDateTime(model.StartDate);

            //valid EndDate
            var eDate = string.IsNullOrEmpty(model.EndDate) ? DateTime.Now.AddDays(1) : Convert.ToDateTime(model.EndDate).AddDays(1);

            //valid Keyword
            model.Keyword = string.IsNullOrEmpty(model.Keyword) ? "" : model.Keyword.Trim();

            //Page
            var totalRecord = _helpArticleService.FilterArticleCount(model.Keyword, model.Status, sDate, eDate, model.CategoryId);
            var totalPage = (totalRecord - 1) / pageSize + 1;

            if (model.Page < 1 || model.Page > totalPage)
            {
                return RedirectToAction("HelpArticles");
            }

            model.PagerModel = new PaginationModels
            {
                PageNumber = model.Page,
                PageSize = pageSize,
                TotalRecords = totalRecord
            };

            model.ListCategory = new SelectList(_helpCategoryService.SelectTreeWithIndent(), "Id", "Name", model.CategoryId);

            model.ListArticle = _helpArticleService.FilterArticle(model.Keyword, model.Status, sDate, eDate, model.CategoryId, model.Page, pageSize).ToList();
            foreach (var item in model.ListArticle)
            {
                item.HelpCategory = _helpCategoryService.GetHelpCategoryById(item.CategoryId);
            }

            model.ListStatus = new SelectList(new[]
            {
                new SelectListItem{ Text = "Hiển thị", Value = HelpArticleStatusConst.Active.ToString() },
                new SelectListItem{ Text = "Ẩn", Value = HelpArticleStatusConst.InActive.ToString() }
            }, "Value", "Text", model.Status);

            Title = "Quản trị bài viết";
            ViewData["ToolbarTitle"] = Title;
            return View(model);
        }
        //
        // GET: /Admin/Categories/Details/5

        public ActionResult Details(int id)
        {
            Title = "Chi tiết danh mục hỗ trợ khách hàng";
            ViewData["ToolbarTitle"] = Title;
            return View();
        }

        //
        // GET: /Admin/Categories/Create

        public ActionResult Create()
        {
            Title = "Thêm mới danh mục";
            ViewData["ToolbarTitle"] = Title;

            var model = new HelpCategoryModel()
            {
                Status = HelpCategoryStatusConst.Active,
                CategoryList = new SelectList(_helpCategoryService.Help_Categories_SelectTreeAll(), "Id", "Name")
            };

            model.ListStatus = new SelectList(new[]
            {
                new SelectListItem{ Text = "Hiển thị", Value = HelpCategoryStatusConst.Active.ToString() },
                new SelectListItem{ Text = "Ẩn", Value = HelpCategoryStatusConst.InActive.ToString() }
            }, "Value", "Text");

            return View(model);
        }

        //
        // POST: /Admin/Categories/Create

        [HttpPost]
        public ActionResult Create(HelpCategoryModel model)
        {
            if (ModelState.IsValid)
            {
                var now = DateTime.Now;
                try
                {
                    var helpsCategory = model.ToEntity();
                    helpsCategory.Name = model.Name;
                    helpsCategory.ParentId = model.ParentId;
                    helpsCategory.Alias = Util.GetSEOAlias(model.Name);
                    helpsCategory.Description = model.Description;
                    helpsCategory.Created = now;
                    helpsCategory.Modified = now;
                    helpsCategory.OrderNumber = model.OrderNumber;
                    helpsCategory.Status = model.Status;
                    helpsCategory.Image = "";
                    helpsCategory.DefaultAdImage = "";

                    _helpCategoryService.AddHelpCategory(helpsCategory);
                    return RedirectToAction("Index");
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

            model.CategoryList = new SelectList(_helpCategoryService.Help_Categories_SelectTreeAll(), "Id", "Name", model.ParentId);
            model.ListStatus = new SelectList(new[]
            {
                new SelectListItem{ Text = "Hiển thị", Value = HelpCategoryStatusConst.Active.ToString() },
                new SelectListItem{ Text = "Ẩn", Value = HelpCategoryStatusConst.InActive.ToString() }
            }, "Value", "Text", model.Status);

            Title = "Thêm mới danh mục";
            ViewData["ToolbarTitle"] = Title;
            return View(model);
        }

        //
        // GET: /Admin/Categories/Edit/5

        // [SourceCodeFile("ImageBrowser Controller", "~/Controllers/Editor/ImageBrowserController.cs")]
        public ActionResult Edit(int id)
        {
            var helpsCategory = _helpCategoryService.GetHelpCategoryById(id);

            if (helpsCategory == null)
            {
                ErrorNotification("Không tìm thấy trang tĩnh nào thỏa mãn");
                return RedirectToAction("Index");
            }

            var lstParentCategory = _helpCategoryService.Help_Categories_SelectTreeAll().ToList();
            var currentCate = lstParentCategory.SingleOrDefault(c => c.Id == id);

            if (currentCate != null)
            {
                lstParentCategory.Remove(currentCate);
                lstParentCategory.RemoveAll(c => c.Lineage.Contains("/" + currentCate.Id + "/"));
            }

            var categoryModel = helpsCategory.ToModel();

            categoryModel.CategoryList = new SelectList(lstParentCategory, "Id", "Name", categoryModel.ParentId);
            categoryModel.ListStatus = new SelectList(new[]
            {
                new SelectListItem{ Text = "Hiển thị", Value = HelpCategoryStatusConst.Active.ToString() },
                new SelectListItem{ Text = "Ẩn", Value = HelpCategoryStatusConst.InActive.ToString() }
            }, "Value", "Text", categoryModel.Status);

            Title = "Chỉnh sửa danh mục hỗ trợ : " + helpsCategory.Name;
            ViewData["ToolbarTitle"] = Title;
            return View(categoryModel);
        }

        //
        // POST: /Admin/Categories/Edit/5

        [HttpPost]
        public ActionResult Edit(HelpCategoryModel model)
        {
            string command = Request.Form["submit"].ToString();
            switch (command)
            {
                case "Save":
                case "SaveAndContinueEdit":
                    if (ModelState.IsValid)
                    {
                        var now = DateTime.Now;
                        try
                        {
                            var category = _helpCategoryService.GetHelpCategoryById(model.Id);
                            if (category == null)
                            {
                                ErrorNotification("Không tìm thấy trang tĩnh nào thỏa mãn");
                                return RedirectToAction("Index");
                            }
                            category.ParentId = model.ParentId;
                            category.Name = model.Name;
                            category.Alias = Util.GetSEOAlias(model.Name);
                            category.Description = model.Description;
                            category.Modified = now;
                            category.OrderNumber = model.OrderNumber;
                            category.Status = model.Status;

                            _helpCategoryService.UpdateHelpCategory(category);
                            SuccessNotification("Cập nhật thông tin thành công");
                            if (command == "SaveAndContinueEdit")
                            {
                                Title = "Chỉnh sửa danh mục hỗ trợ: " + category.Name;
                                //return View(pageModel);
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

            ViewData["ToolbarTitle"] = Title;
            var lstParentCategory = _helpCategoryService.Help_Categories_SelectTreeAll();
            var currentCate = lstParentCategory.SingleOrDefault(c => c.Id == model.Id);
            if (currentCate != null)
            {
                lstParentCategory.Remove(currentCate);
                lstParentCategory.RemoveAll(c => c.Lineage.Contains("/" + currentCate.Id + "/"));
            }
            model.CategoryList = new SelectList(lstParentCategory, "Id", "Name", model.ParentId);
            return View(model);
        }

        public ActionResult CreateArticle()
        {
            Title = "Thêm mới bài viết";
            ViewData["ToolbarTitle"] = Title;

            var model = new HelpArticleModel()
            {
                CategoryList = new SelectList(_helpCategoryService.Help_Categories_SelectTreeAll(), "Id", "Name"),
                IsFeatured = false,
                Status = HelpArticleStatusConst.Active,
                IsSpecial = false
            };

            model.ListStatus = new SelectList(new[]
            {
                new SelectListItem{ Text = "Hiển thị", Value = HelpArticleStatusConst.Active.ToString() },
                new SelectListItem{ Text = "Ẩn", Value = HelpArticleStatusConst.InActive.ToString() }
            }, "Value", "Text", model.Status);

            return View(model);
        }

        //
        // POST: /Admin/Helps/Create

        [HttpPost]
        public ActionResult CreateArticle(HelpArticleModel model, HttpPostedFileBase articleImage)
        {
            model.CategoryList = new SelectList(_helpCategoryService.Help_Categories_SelectTreeAll(), "Id", "Name", model.CategoryId);
            model.ListStatus = new SelectList(new[]
            {
                new SelectListItem{ Text = "Hiển thị", Value = HelpArticleStatusConst.Active.ToString() },
                new SelectListItem{ Text = "Ẩn", Value = HelpArticleStatusConst.InActive.ToString() }
            }, "Value", "Text", model.Status);

            Title = "Thêm mới danh mục";
            ViewData["ToolbarTitle"] = Title;
            if (ModelState.IsValid)
            {
                if (model.CategoryId > 0)
                {
                    var category = _helpCategoryService.GetHelpCategoryById(model.CategoryId);
                    if (category != null)
                    {
                        var childCates = _helpCategoryService.GetAllHelpCategoryByParentId(category.Id);
                        if (childCates != null && childCates.Count() > 0)
                        {
                            ErrorNotification("Không được tạo bài viết ở danh mục cha");
                            return View(model);
                        }
                    }
                    else
                    {
                        ErrorNotification("Danh mục không tồn tại.");
                        return View(model);
                    }
                }
                var now = DateTime.Now;
                try
                {
                    var helpArticle = model.ToEntity();
                    helpArticle.CategoryId = model.CategoryId;
                    helpArticle.Title = model.Title;

                    if (articleImage != null)
                    {
                        string[] fileExtensions = { ".jpg", ".jpeg", ".gif", ".png" };
                        string extension = Path.GetExtension(articleImage.FileName).ToLower();
                        if (fileExtensions.Contains(extension))
                        {

                            string currentDir = _rootDir + now.Year + "/" + now.Month + "/" + now.Day + "/";
                            if (!_thriftClient.CheckFolderIsExist(currentDir))
                            {
                                _thriftClient.CreateFolderByService(currentDir);
                            }
                            string filename = currentDir + Util.ComputeMD5Hash(articleImage.FileName + now.Ticks) + extension;
                            helpArticle.Image = filename;
                            _thriftClient.UploadFileByService(filename, articleImage.InputStream.ToBytes());
                            var thumbnail = new Thumbnail(_thumbnailSettingService, _thriftClient);
                            thumbnail.CreateAllThumbnails(filename);
                        }
                    }

                    helpArticle.Created = now;
                    helpArticle.Modified = now;
                    helpArticle.Abstract = model.Abstract;
                    helpArticle.Body = model.Body;
                    helpArticle.Status = model.Status;
                    helpArticle.IsFeatured = model.IsFeatured;
                    helpArticle.AliasTitle = Util.GetSEOAlias(model.Title);
                    helpArticle.MetaKeyword = model.MetaKeyword;
                    helpArticle.MetaDescription = Util.SubString(helpArticle.Abstract, 169);
                    helpArticle.IsSpecial = model.IsSpecial;

                    _helpArticleService.AddHelpArticle(helpArticle);

                    SuccessNotification("Thêm mới bài viết thành công!");
                    return RedirectToAction("HelpArticles");
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


            return View(model);
        }

        //
        // GET: /Admin/Helps/Edit/5

        public ActionResult EditArticle(int id)
        {
            var helpArticle = _helpArticleService.GetHelpArticleById(id);
            if (helpArticle == null)
            {
                ErrorNotification("Không tìm thấy bài viết nào thỏa mãn");
                return RedirectToAction("ListHelpArticle");
            }

            var model = helpArticle.ToModel();
            model.CategoryList = new SelectList(_helpCategoryService.Help_Categories_SelectTreeAll(), "Id", "Name", model.CategoryId);
            model.ListStatus = new SelectList(new[]
            {
                new SelectListItem{ Text = "Hiển thị", Value = HelpArticleStatusConst.Active.ToString() },
                new SelectListItem{ Text = "Ẩn", Value = HelpArticleStatusConst.InActive.ToString() }
            }, "Value", "Text", model.Status);

            Title = "Chỉnh sửa bài viết";
            ViewData["ToolbarTitle"] = Title;
            return View(model);
        }

        //
        // POST: /Admin/Helps/Edit/5

        [HttpPost]
        public ActionResult EditArticle(HelpArticleModel model, HttpPostedFileBase articleImage)
        {
            model.CategoryList = new SelectList(_helpCategoryService.Help_Categories_SelectTreeAll(), "Id", "Name", model.CategoryId);
            model.ListStatus = new SelectList(new[]
            {
                new SelectListItem{ Text = "Hiển thị", Value = HelpArticleStatusConst.Active.ToString() },
                new SelectListItem{ Text = "Ẩn", Value = HelpArticleStatusConst.InActive.ToString() }
            }, "Value", "Text", model.Status);

            ViewData["ToolbarTitle"] = Title;
            string command = Request.Form["submit"].ToString();
            switch (command)
            {
                case "Save":
                case "SaveAndContinueEdit":
                    if (ModelState.IsValid)
                    {
                        try
                        {
                            var helpArticle = _helpArticleService.GetHelpArticleById(model.Id);
                            if (helpArticle == null)
                            {
                                ErrorNotification("Không tìm thấy trang tĩnh nào thỏa mãn");
                                return RedirectToAction("HelpArticles");
                            }

                            helpArticle.CategoryId = model.CategoryId;
                            helpArticle.Title = model.Title;

                            if (articleImage != null)
                            {
                                helpArticle.Image = "~/Upload/" + articleImage.FileName;
                                _thriftClient.UploadFileByService(helpArticle.Image, articleImage.InputStream.ToBytes());
                                var thumbnail = new Thumbnail(_thumbnailSettingService, _thriftClient);
                                thumbnail.CreateAllThumbnails(articleImage.FileName);
                            }

                            helpArticle.Modified = DateTime.Now;
                            helpArticle.Abstract = model.Abstract;
                            helpArticle.Body = model.Body;
                            helpArticle.IsFeatured = model.IsFeatured;
                            helpArticle.Status = model.Status;
                            helpArticle.AliasTitle = Util.GetSEOAlias(model.Title);
                            helpArticle.MetaKeyword = model.MetaKeyword;
                            helpArticle.MetaDescription = Util.SubString(helpArticle.Abstract, 169);
                            helpArticle.IsSpecial = model.IsSpecial;

                            _helpArticleService.UpdateHelpArticle(helpArticle);
                            SuccessNotification("Cập nhật thông tin thành công");

                            if (command == "SaveAndContinueEdit")
                            {
                                Title = "Chỉnh sửa bài viết : " + helpArticle.Title;
                            }
                            else
                            {
                                return RedirectToAction("HelpArticles");
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
                    return RedirectToAction("DeleteArticle", new { id = model.Id });

                default:
                    ErrorNotification("Không rõ phương thức submit dữ liệu");
                    return RedirectToAction("HelpArticles");

            }


            return View(model);
        }

        //
        // GET: /Admin/Categories/Delete/5

        public ActionResult Delete(int id)
        {
            var category = _helpCategoryService.GetHelpCategoryById(id);
            if (category == null)
            {
                ErrorNotification("Không tìm thấy trang tĩnh nào thỏa mãn");
                return RedirectToAction("Index");
            }

            if (category.HasChild == true)
            {
                ErrorNotification("Không thể xóa danh mục có chứa các danh mục khác");
                return RedirectToAction("Index");
            }

            var lstArticle = _helpArticleService.GetHelpArticleByCateIdCount(id);
            if (lstArticle > 0)
            {
                ErrorNotification("Không thể xóa danh mục có chứa bài viết");
                return RedirectToAction("Index");
            }

            Title = "Xóa mục hỗ trợ: " + category.Name;
            ViewData["ToolbarTitle"] = Title;
            return View(category);
        }

        //
        // POST: /Admin/Categories/Delete/5

        [HttpPost]
        public ActionResult Delete(int id, FormCollection collection)
        {
            Title = "Xóa mục hỗ trợ :";
            string command = Request.Form["submit"].ToString();
            switch (command)
            {
                case "Delete":
                    var category = _helpCategoryService.GetHelpCategoryById(id);
                    if (category == null)
                    {
                        ErrorNotification("Không tìm thấy trang tĩnh nào thỏa mãn");
                        return RedirectToAction("Index");
                    }
                    if (category.HasChild == true)
                    {
                        ErrorNotification("Không thể xóa danh mục có chứa các danh mục khác");
                        return RedirectToAction("Index");
                    }

                    var lstArticle = _helpArticleService.GetHelpArticleByCateIdCount(id);
                    if (lstArticle > 0)
                    {
                        ErrorNotification("Không thể xóa danh mục có chứa bài viết");
                        return RedirectToAction("Index");
                    }
                    Title += category.Name;
                    _helpCategoryService.RemoveHelpCategory(category);
                    SuccessNotification("Xóa danh mục hỗ trợ thành công");
                    return RedirectToAction("Index");
                default:
                    ErrorNotification("Không rõ phương thức submit dữ liệu");
                    return RedirectToAction("Index");
            }
        }
        public ActionResult DeleteArticle(int id)
        {

            var article = _helpArticleService.GetHelpArticleById(id);

            if (article == null)
            {
                ErrorNotification("Không tìm thấy bài viết nào thỏa mãn");
                return RedirectToAction("ListHelpArticle");
            }

            Title = "Xóa bài viết: " + article.Title;
            ViewData["ToolbarTitle"] = Title;
            return View(article);
        }

        //
        // POST: /Admin/Categories/Delete/5

        [HttpPost]
        public ActionResult DeleteArticle(int id, FormCollection collection)
        {
            Title = "Xóa bài viết hỗ trợ: ";
            string command = Request.Form["submit"].ToString();
            switch (command)
            {
                case "Delete":
                    var article = _helpArticleService.GetHelpArticleById(id);
                    if (article == null)
                    {
                        ErrorNotification("Không tìm thấy trang tĩnh nào thỏa mãn");
                        return RedirectToAction("ListHelpArticle");
                    }

                    Title += article.Title;
                    _helpArticleService.RemoveHelpArticle(article);

                    SuccessNotification("Xóa bài viết hỗ trợ thành công");
                    return RedirectToAction("ListHelpArticle");
                default:
                    ErrorNotification("Không rõ phương thức submit dữ liệu");
                    return RedirectToAction("ListHelpArticle");
            }
        }

        public ActionResult DeleteArticles(string strId)
        {
            if (!strId.Equals(""))
            {
                strId = strId.Trim('|');
                string[] arrId = strId.Split('|');
                foreach (var adId in arrId)
                {
                    var obj = new HelpArticle();
                    int aId = 0;
                    Int32.TryParse(adId, out aId);
                    obj = _helpArticleService.GetHelpArticleById(Convert.ToInt32(aId));
                    if (obj != null)
                    {
                        _helpArticleService.RemoveHelpArticle(obj);
                    }
                    else
                    {
                        return Content("error: Không thể xóa bài viết này!");
                    }
                }
                return Content("Xóa bài viết thành công!");
            }
            else
            {
                return Content("error: Bạn phải chọn ít nhất một bài viết để xóa!");
            }

        }

        [HttpGet]
        public ActionResult FilterCategories(string keyword, int status, string startDate, string endDate)
        {
            DateTime sDate;
            DateTime eDate;
            sDate = string.IsNullOrEmpty(startDate) ? _helpCategoryService.GetMinCreateOn() : Convert.ToDateTime(startDate);
            sDate = new DateTime(sDate.Year, sDate.Month, sDate.Day, 0, 0, 0, 0);

            eDate = string.IsNullOrEmpty(endDate) ? DateTime.Now : Convert.ToDateTime(endDate);
            eDate = new DateTime(eDate.Year, eDate.Month, eDate.Day, 23, 59, 59, 999);
            keyword = string.IsNullOrEmpty(keyword) ? "" : keyword.Trim();

            var results = _helpCategoryService.FilterCategory(keyword, status, sDate, eDate);

            Title = string.IsNullOrEmpty(keyword) ? "Kết quả tìm kiếm" : "Kế quả tìm kiếm: " + keyword;

            ViewData["ToolbarTitle"] = Title;
            return View("Index", results);

        }
    }
}
