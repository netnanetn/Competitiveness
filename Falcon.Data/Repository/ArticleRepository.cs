using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Falcon.Data;
using Falcon.Data.Repository;
using Falcon.Data.Domain;

namespace Falcon.Data.Repository
{
    public class ArticleRepository : BaseRepository<Article>, IArticleRepository
    {
        public ArticleRepository(Database database)
            : base(database)
        {
        }

        public ArticleRepository(IDatabaseFactory factory)
            : base(factory)
        {
        }
    }
}
