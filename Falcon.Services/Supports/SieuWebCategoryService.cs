using Falcon.Data.Repository.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Falcon.Data.Domain;

namespace Falcon.Services.Supports
{
 public class SieuWebCategoryService : BaseService, ISieuWebCategoryService
    {
        private readonly ISieuWebCategoryRepository _sieuWebCategoryRepository;
        public SieuWebCategoryService(ISieuWebCategoryRepository sieuWebCategoryRepository)
        {
            this._sieuWebCategoryRepository = sieuWebCategoryRepository;
        }

        public IEnumerable<Category> GetAllSieuWebCategory()
        {
            return _sieuWebCategoryRepository.Table.OrderByDescending(c => c.OrderNumber).ToList();       
        }
    }
}
