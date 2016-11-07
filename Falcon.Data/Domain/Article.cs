using ProtoBuf;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Falcon.Data.Domain
{
    [ProtoContract(ImplicitFields = ImplicitFields.AllPublic, ImplicitFirstTag = 1)]
    public partial class Article : BaseEntity
    {
        public override int Id { get; set; } // Id (Primary key)
        public string Name { get; set; } // Name
        public string Alias { get; set; } // Alias
        public int CategoryId { get; set; } // CategoryId
        public DateTime CreatedOn { get; set; } // CreatedOn
        public DateTime? ModifiedOn { get; set; } // ModifiedOn
        public DateTime? PublishedOn { get; set; }
        public bool Status { get; set; }
        public string MetaTitle { get; set; }
        public string MetaDescription { get; set; }
        public string Description { get; set; }
        public string ImgPath { get; set; }
        public bool IsHighligh { get; set; }
        public string Keyword { get; set; }

      

        public Article()
        {
            CreatedOn = System.DateTime.Now;
            InitializePartial();
        }
        partial void InitializePartial();
    }
}
