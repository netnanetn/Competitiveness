﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Falcon.Data;
using Falcon.Data.Repository;
using Falcon.Data.Domain;

namespace Falcon.Data.Repository
{
    public class CategoryTypeRepository : BaseRepository<CategoryType>, ICategoryTypeRepository
    {
        public CategoryTypeRepository(Database database)
            : base(database)
        {
        }

        public CategoryTypeRepository(IDatabaseFactory factory)
            : base(factory)
        {
        }
    }
}
