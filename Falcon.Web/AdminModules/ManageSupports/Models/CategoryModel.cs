using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Falcon.Admin.Modules.ManageSupports.Models
{
    public class CategoryModel
    {
        public int Id { get; set; }

        [Display(Name = "Tên danh mục")]
        [Required(ErrorMessage = "Nhập vào tên danh mục")]
        [StringLength(255, ErrorMessage = "Tên danh mục không dài quá {1} ký tự")]
        public string Name { get; set; }


        [StringLength(150, ErrorMessage = "Alias không dài quá {1} ký tự")]
        public string Alias { get; set; }
        public DateTime CreatedOn { get; set; }

        public DateTime ModifiedOn { get; set; }

        [Display(Name = "Mô tả")]
        [StringLength(255, ErrorMessage = "Mô tả danh mục không dài quá {1} ký tự")]
        public string Description { get; set; }
        [Display(Name = "Hiển thị")]
        public bool Status { get; set; }
        [Display(Name = "Thứ tự")]
        public int OrderNumber { get; set; }
        [Display(Name = "I con danh mục")]
        public string ImagePath { get; set; }
    }
     public class ListCategoryModel
        {
            public List<CategoryModel> ListCategories { get; set; }
            public ListCategoryModel()
            {
            ListCategories = new List<CategoryModel>();

              
            }
        }   
}