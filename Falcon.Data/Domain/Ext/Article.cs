using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;

namespace Falcon.Data.Domain
{
    public partial class Article
    {
        [NotMapped]
        public string Content { get; set; }
    }
}
