using Falcon.Data.Domain;
using Falcon.Data.Repository.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Falcon.Data.Repository
{
    public class SieuWebCategoryRepository : BaseRepository<Category>, ISieuWebCategoryRepository
    {
        public SieuWebCategoryRepository(Database database)
            : base(database)
        {
        }

        public SieuWebCategoryRepository(IDatabaseFactory factory)
            : base(factory)
        {
        }
    }
}
