using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Falcon.Data;
using Falcon.Data.Repository;
using Falcon.Data.Domain;

namespace Falcon.Data.Repository
{
    public class CategoryRepository : BaseRepository<Category>, ICategoryRepository
    {
        public CategoryRepository(Database database)
            : base(database)
        {
        }

        public CategoryRepository(IDatabaseFactory factory)
            : base(factory)
        {
        }
    }
}
