using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Falcon.Tasks;
using AutoMapper;
using Falcon.Data.Domain;
using Falcon.Admin.Modules.ManageSupports.Models;

namespace Falcon.Admin.Modules.ManageSupports
{
    public class AutoMapperStartupTask : IStartupTask
    {

        public void Execute()
        {
            ViceVersa<Category, CategoryModel>();
            ViceVersa<Article, ArticleModel>();            
        }

        protected virtual void ViceVersa<T1, T2>()
        {
            Mapper.CreateMap<T1, T2>();
            Mapper.CreateMap<T2, T1>();
        }

        public int Order
        {
            get { return 0; }
        }
    }
}
