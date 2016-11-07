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
using Falcon.Common;
using Falcon.Common.UI;
using System.IO;
//using StackExchange.Profiling;

namespace Falcon.Admin.Modules.Contents.Controllers
{
    public class CategoriesController : AdminBaseController
    {
        private readonly ICategoryService _categoryService;
        private readonly IArticleService _articleService;
        private const string _rootDir = "~/Upload/Video/";
        public CategoriesController(ICategoryService categoryService, IArticleService articleService)
        {
            _categoryService = categoryService;
            _articleService = articleService;
        }
        //
        // GET: /Admin/Pages/
        [HttpPost]
        public ActionResult UpdateRecord(string positions, List<int> ids)
        {
            try
            {
                var categorys = _categoryService.GetAllCategory();

                for (int i = 1; i <= ids.Count; i++)
                {
                    var category = categorys.FirstOrDefault(c => c.Id == ids[i - 1]);
                    category.OrderNumber = i;
                    _categoryService.UpdateCategory(category);
                }
                return Json("Sccess", JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(ex.Message, JsonRequestBehavior.AllowGet);
            }
        }
        public ActionResult Index(ListCategoryModel model)
        {
            Title = "Quản trị danh mục trợ giúp ";
            ViewData["ToolbarTitle"] = Title;
            var listSieuWebCategories = _categoryService.GetAllCategory();
            foreach (var item in listSieuWebCategories)
            {
                var sieuWebCategoryModel = item.ToModel();
                model.ListCategories.Add(sieuWebCategoryModel);

            }
            return View(model);
        }
        public ActionResult Create()
        {
            Title = "Thêm mới danh mục";
            ViewData["ToolbarTitle"] = Title;
            var model = new CategoryModel();
            return View(model);

        }
        [HttpPost]
        public ActionResult Create(CategoryModel model, HttpPostedFileBase datafile)
        {
            string command = Request.Form["submit"].ToString();
            switch (command)
            {
                case "Save":
                case "SaveAndContinueEdit":
                    var alias = Util.GetSEOAlias(model.Alias);
                    var checkCate = _categoryService.GetByAlias(alias);
                    var CategoryOderby = _categoryService.GetAllCategory().OrderByDescending(c => c.OrderNumber).ToList();
                    var count = 1;
                    if(CategoryOderby != null && CategoryOderby.Count >0)
                    {
                        count = CategoryOderby[0].OrderNumber + 1;
                    }
                    if (checkCate != null)
                    {
                        ModelState.AddModelError("Name", "Alias đã được sử dụng");
                    }
                    if (ModelState.IsValid)
                    {
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
                                model.ImagePath = filePath;
                            }
                        }
                        try
                        {
                            Category category = new Category();
                            category.OrderNumber = model.OrderNumber;
                            category.Name = model.Name;
                            category.Alias = Util.GetSEOAlias(model.Name);
                            category.Description = model.Description;
                            category.Status = model.Status;
                            category.OrderNumber = count;
                            category.CreatedOn = DateTime.Now;
                            category.ImagePath = model.ImagePath;
                            var id=   _categoryService.AddCategory(category);
                            if (command == "SaveAndContinueEdit")
                            {
                                return RedirectToAction("Edit", new { id = id });   
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

            Title = "Thêm mới danh mục";
            ViewData["ToolbarTitle"] = Title;
            return View(model);

        }
        public ActionResult Edit(int id)
        {
            Category category = _categoryService.GetCategoryById(id);
            if (category == null)
            {
                ErrorNotification("Không tìm thấy danh mục thỏa mãn");
                return RedirectToAction("Index");
            }
            var model = category.ToModel();
            Title = "Chỉnh sửa thông tin danh mục " + category.Name;
            ViewData["ToolbarTitle"] = Title;
            return View(model);
        }

        [HttpPost]
        public ActionResult Edit(CategoryModel model, HttpPostedFileBase datafile)
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
                            Category category = _categoryService.GetCategoryById(model.Id);
                            if (category == null)
                            {
                                ErrorNotification("Không tìm thấy danh mục thỏa mãn");
                                return RedirectToAction("Index");
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
                                    model.ImagePath = filePath;
                                }
                            }
                            category.Name = model.Name;
                           
                            category.Description = model.Description;
                            category.Status = model.Status;
                            if (!string.IsNullOrEmpty(model.ImagePath))
                            {
                                category.ImagePath = model.ImagePath;
                            }
                            category.ModifiedOn = DateTime.Now;


                            _categoryService.UpdateCategory(category);

                            if (command == "SaveAndContinueEdit")
                            {
                                Title = "Chỉnh sửa thông tin danh mục trợ giúp: " + category.Name;
                                //return View(pageModel);
                            }
                            else
                            {
                                return RedirectToAction("Index");
                            }
                        }
                        catch (Exception e)
                        {
                            Title = "Chỉnh sửa thông tin danh mục";
                            ErrorNotification(e.ToString());
                        }
                    }
                    else
                    {
                        AddModelStateErrors();
                    }

                    break;
                case "Delete":
                    return RedirectToAction("Delete", new { id = model.Id });

                default:
                    ErrorNotification("Không rõ phương thức submit dữ liệu");
                    return RedirectToAction("Index");

            }

            ViewData["ToolbarTitle"] = Title;

            return View(model);
        }

        public ActionResult Delete(int id)
        {
            Category category = _categoryService.GetCategoryById(id);
            if (category == null)
            {
                ErrorNotification("Không tìm thấy danh mục nào thỏa mãn");
                return RedirectToAction("Index");
            }
            Title = "Xóa danh mục: " + category.Name;
            ViewData["ToolbarTitle"] = Title;
            return View(category);
        }
        [HttpPost]
        public ActionResult Delete(int id, FormCollection collection)
        {
            string command = Request.Form["submit"].ToString();
            switch (command)
            {
                case "Delete":
                    Category category = _categoryService.GetCategoryById(id);
                    if (category == null)
                    {
                        ErrorNotification("Không tìm thấy danh mục thỏa mãn");
                        return RedirectToAction("Index");
                    }
                    var count = _articleService.GetArticleByCateIdCount(id);
                    if (count > 0)
                    {
                        ErrorNotification("Danh mục có bài viết, không thể xóa!");
                        return RedirectToAction("Index");
                    }
                    _categoryService.RemoveCategory(category);
                    SuccessNotification("Xóa danh mục thành công");
                    return RedirectToAction("Index");
                default:
                    ErrorNotification("Không rõ phương thức submit dữ liệu");
                    return RedirectToAction("Index");
            }
        }

    }
}
