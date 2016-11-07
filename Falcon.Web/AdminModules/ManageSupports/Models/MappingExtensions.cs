using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Falcon.Data.Domain;
using AutoMapper;
using Falcon.Admin.Modules.ManageSupports.Models;

namespace Falcon.Admin.Modules.ManageSupports.Models
{
    public static class MappingExtensions
    {
        #region Supports
        public static CategoryModel ToModel(this Category entity)
        {
            return Mapper.Map<Category, CategoryModel>(entity);
        }
        public static Category ToEntity(this CategoryModel model)
        {
            return Mapper.Map<CategoryModel, Category>(model);
        }

        public static ArticleModel ToModel(this Article entity)
        {
            return Mapper.Map<Article, ArticleModel>(entity);
        }

        public static Article ToEntity(this ArticleModel model)
        {
            return Mapper.Map<ArticleModel, Article>(model);
        }
        #endregion        
        
    }
}