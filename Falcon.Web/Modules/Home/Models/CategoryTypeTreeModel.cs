using Falcon.Data.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Falcon.Modules.Home.Models
{
    public class CategoryTypeTreeModel
    {
        public int CurrentArticleId { get; set; }
        public List<CategoryType> CategoryTypes { get; set; }
        public List<Article> ArticleChilds { get; set; }
        public CategoryTypeTreeModel()
        {
            CategoryTypes = new List<CategoryType>();
            ArticleChilds = new List<Article>();
        }
    }

    public class CategoryTypeModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public List<Category> Categories { get; set; }
        public List<Article> RepresentArticles { get; set; }
        public CategoryTypeModel()
        {
            Categories = new List<Category>();
            RepresentArticles = new List<Article>();
        }
    }

}