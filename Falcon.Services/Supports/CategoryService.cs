using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using Falcon.Data.Repository;
using Falcon.Data.Domain;
using Falcon.Common;

namespace Falcon.Services.Supports
{
    public class CategoryService : BaseService, ICategoryService
    {
        private readonly ICategoryRepository _categoryRepository;

        public CategoryService(ICategoryRepository categoryRepository)
        {
            this._categoryRepository = categoryRepository;
        }

        public Category GetCategoryById(int Id)
        {
            return _categoryRepository.Table.SingleOrDefault(c => c.Id == Id);
        }

        public int AddCategory(Category category)
        {
            _categoryRepository.Add(category);
            return category.Id;
        }

        public void UpdateCategory(Category category)
        {

            _categoryRepository.SubmitChanges();
        }

        public void RemoveCategory(Category category)
        {

            _categoryRepository.Remove(category);
        }

        public IEnumerable<Category> GetAllCategory()
        {
            return _categoryRepository.Table.OrderBy(c => c.OrderNumber).ToList();
        }
        public List<Category> GetActiveCategory()
        {
            return _categoryRepository.Table.Where(c => c.Status == true).ToList();
        }
        public Category GetByAlias(string alias)
        {
            return _categoryRepository.Table.Where(c => c.Alias == alias).FirstOrDefault();
        }

        public List<Category> FilterCategory(int typeId, int categoryTypeId, int pageIndex, int pageSize)
        {
            return _categoryRepository.QuerySP<Category>("Categories_FilterAdmin", new {TypeId = typeId, CategoryTypeId = categoryTypeId, PageIndex = pageIndex, PageSize = pageSize }).ToList();
        }

        public Category GetByAlias(string alias, int typeId, int categoryTypeId)
        {
            throw new NotImplementedException();
        }

        public int FilterCategoryCount(int typeId, int categoryTypeId)
        {
            throw new NotImplementedException();
        }

        public List<Category> GetByCategoryType(int type)
        {
            throw new NotImplementedException();
        }

        public List<Category> GetCategoryBySupportTypeId(int supportTypeId)
        {
            throw new NotImplementedException();
        }

        //public int FilterCategoryCount(int typeId, int categoryTypeId)
        //{
        //    return _categoryRepository.Table.Where(c => c.TypeId == typeId && c.CategoryTypeId == categoryTypeId).ToList().Count();
        //}

        //public List<Category> GetByCategoryType(int type)
        //{
        //    return _categoryRepository.Table.Where(c => c.CategoryTypeId == type && c.Status == true).OrderBy(c => c.OrderNumber).ToList();
        //}

        //public List<Category> GetCategoryBySupportTypeId(int supportTypeId)
        //{
        //    return _categoryRepository.Table.Where(c => c.TypeId == supportTypeId && c.Status == true).OrderBy(c => c.OrderNumber).ToList();
        //}
    }
}