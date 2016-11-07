using DataAnnotationsExtensions;
using Falcon.Common.UI;
using Falcon.Data.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Falcon.Modules.Home.Models
{
    public class ArticleDetailModel
    {
        public Article ArticleDetail { get; set; }
        public List<Category> Categories { get; set; }
        public List<Article> ArticleChilds { get; set; }
        public List<Article> RepresentArticles { get; set; }
        public ArticleDetailModel()
        {
            ArticleDetail = new Article();
            Categories = new List<Category>();
            ArticleChilds = new List<Article>();
            RepresentArticles = new List<Article>();
        }
    }

    public class CategoryModel
    {
        public Category Category { get; set; }
        public Article RepresentArticle { get; set; }
        public List<Article> ArticleChilds { get; set; }
        public CategoryModel()
        {
            ArticleChilds = new List<Article>();
        }
    }

    public class StandardCategoryTypeModel
    {
        public CategoryType CategoryType { get; set; }
        public List<CategoryModel> Categories { get; set; }
        public StandardCategoryTypeModel()
        {
            Categories = new List<CategoryModel>();
        }
    }

    public class ArticleSearchModel
    {
        public int SupportTypeId { get; set; }
        //public List<SupportType> SupportTypes { get; set; }
        public string Keyword { get; set; }

        [Integer()]
        [Min(1)]
        public int Page { get; set; }
        public List<Article> Articles { get; set; }
        public List<StandardCategoryTypeModel> CategoryTypes { get; set; }
        public PaginationModels PagerModel { get; set; }

        public ArticleSearchModel()
        {
            Page = 1;
            Articles = new List<Article>();
            CategoryTypes = new List<StandardCategoryTypeModel>();
            //SupportTypes = new List<SupportType>();
        }
    }

    public class ArticleListModel
    {
        [Integer()]
        [Min(1)]
        public int Page { get; set; }
        public List<Article> Articles { get; set; }
        public PaginationModels PagerModel { get; set; }

        public ArticleListModel()
        {
            Page = 1;
            Articles = new List<Article>();
        }
    }
}