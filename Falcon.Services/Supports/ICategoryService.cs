using System;
using System.Collections.Generic;
using System.Text;
using Falcon.Data.Domain;
using Falcon.Common;

namespace Falcon.Services.Supports
{
    public interface ICategoryService : IService
	{
		Category GetCategoryById(int Id);
		int AddCategory(Category entry);
		void UpdateCategory(Category entry);
		void RemoveCategory(Category entry);

        List<Category> GetActiveCategory();

        IEnumerable<Category> GetAllCategory();
        Category GetByAlias(string alias);
        List<Category> FilterCategory(int typeId, int categoryTypeId, int pageIndex, int pageSize);
        int FilterCategoryCount(int typeId, int categoryTypeId);

        List<Category> GetByCategoryType(int type);
        List<Category> GetCategoryBySupportTypeId(int supportTypeId);
	}
}