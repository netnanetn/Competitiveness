using System;
using System.Collections.Generic;
using System.Text;
using Falcon.Data.Domain;
using Falcon.Common;

namespace Falcon.Services.Supports
{
    public interface ICategoryTypeService : IService
	{
        List<CategoryType> GetAllType();
        List<CategoryType> GetCategoryTypeByTypeId(int typeId);
	}
}