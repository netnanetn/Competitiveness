using Falcon.Data.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Falcon.Services.Supports
{
   public interface ISieuWebCategoryService : IService
    {
        IEnumerable<Category> GetAllSieuWebCategory();
    }
}
