using Falcon.Data.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Falcon.Modules.Home.Models
{
    public class FAQModel
    {
        public List<FaqCategoryTypeModel> FaqCategoryTypes { get; set; }
        public FAQModel()
        {
            FaqCategoryTypes = new List<FaqCategoryTypeModel>();
        }
    }

    public class FaqCategoryTypeModel
    {
        public string Name { get; set; }
        public List<FaqCategoryModel> CategoryChilds { get; set; }
        public FaqCategoryTypeModel()
        {
            CategoryChilds = new List<FaqCategoryModel>();
        }
    }

    public class FaqCategoryModel
    {
        public string Name { get; set; }
        public int Id { get; set; }
        public List<Article> Articles { get; set; }
        public FaqCategoryModel()
        {
            Articles = new List<Article>();
        }
    }
}