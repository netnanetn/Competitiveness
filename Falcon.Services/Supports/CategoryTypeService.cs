using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using Falcon.Data.Repository;
using Falcon.Data.Domain;
using Falcon.Common;

namespace Falcon.Services.Supports
{
    public class CategoryTypeService : BaseService, ICategoryTypeService
    {
        private readonly ICategoryTypeRepository _categoryTypeRepository;

        public CategoryTypeService(ICategoryTypeRepository categoryTypeRepository)
        {
            _categoryTypeRepository = categoryTypeRepository;
        }
        
        public List<CategoryType> GetAllType()
        {
            return _categoryTypeRepository.Table.OrderByDescending(c => c.Id).ToList();
        }

        public List<CategoryType> GetCategoryTypeByTypeId(int typeId)
        {
            return _categoryTypeRepository.Table.Where(c => c.TypeId == typeId).ToList();
        }
    }
}