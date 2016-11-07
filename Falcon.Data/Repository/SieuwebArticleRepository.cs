using Falcon.Data.Domain;
using Falcon.Data.Repository.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Falcon.Data.Repository
{
    public class SieuwebArticleRepository : BaseRepository<Article>, ISieuwebArticleRepository
    {
        public SieuwebArticleRepository(Database database)
            : base(database)
        {
        }

        public SieuwebArticleRepository(IDatabaseFactory factory)
            : base(factory)
        {
        }
    }
}
