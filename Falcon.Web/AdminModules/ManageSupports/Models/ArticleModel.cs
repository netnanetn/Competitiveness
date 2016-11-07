using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;
using Falcon.UI.Validation;
using Falcon.Data.Domain;
using Falcon.Common.UI;

namespace Falcon.Admin.Modules.ManageSupports.Models
{    
    public class ArticleModel
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Nhập vào tiêu đề bài viết")]
        [Display(Name="Tiêu đề bài viết")]
        [StringLength(255, ErrorMessage = "Tiêu đề bài viết không dài quá {1} ký tự")]
        public string Name { get; set; }

        [Display(Name = "Đường dẫn")]
        [StringLength(150, ErrorMessage = "Alias không dài quá {1} ký tự")]
        public string Alias { get; set; }

        [Display(Name = "Title SEO")]
        [StringLength(255, ErrorMessage = "Thẻ tiêu đề bài viết không dài quá {1} ký tự")]
        public string MetaTitle { get; set; }

        [Display(Name = "Description SEO")]
        [StringLength(255, ErrorMessage = "Thẻ mô tả bài viết không dài quá {1} ký tự")]
        public string MetaDescription { get; set; }            

        [Required(ErrorMessage = "Nhập vào nội dung bài trợ giúp")]
        [Display(Name="Nội dung")]
        [AllowHtml]
        public string Content { get; set; }
        [Display(Name = "Mô tả ngắn bài viết")]
        [StringLength(2000, ErrorMessage = "Thẻ tiêu đề bài viết không dài quá {1} ký tự")]
        [AllowHtml]
        public string Description { get; set; }
        [Display(Name = "Bài viết nổi bật")]
        public bool IsHighlight { get; set; }
        [Display(Name = "Thẻ từ khóa")]
        public string Keyword { get; set; }
        [Display(Name = "Icon bài viết")]
        public string ImgPath { get; set; }

        public DateTime CreatedOn { get; set; }

        public DateTime? ModifiedOn { get; set; }

        [Display(Name="Hiển thị")]
        public bool Status { get; set; }
        [Display(Name="Danh mục")]
        public int CategoryId { get; set; }
        [Display(Name = "Loại trợ giúp")]   
        public List<Category> ListCategories { get; set; }      
     
        public ArticleModel()
        {
            ListCategories = new List<Category>();           
        }
    }

    public class ListArticleModel
    {
        public List<Article> ListArticles { get; set; }
        public List<Category> ListCategories { get; set; }
        public SelectList ListStatus { get; set; }      
        public string Keyword { get; set; }
        public int Page { get; set; }
        public int Status { get; set; }
        public int CategoryId { get; set; }
        public PaginationModels PagerModel { get; set; }
        public ListArticleModel()
        {
            ListArticles = new List<Article>();
            //ListTypes = new List<SupportType>();
            ListCategories = new List<Category>();
            Page = 1;
        }
    }
}