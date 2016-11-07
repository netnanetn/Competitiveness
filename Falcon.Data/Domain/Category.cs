using ProtoBuf;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Falcon.Data.Domain
{
    // Pages
    [ProtoContract(ImplicitFields = ImplicitFields.AllPublic, ImplicitFirstTag = 1)]
    public partial class Category : BaseEntity
    {
        public override int Id { get; set; } // Id (Primary key)
        public string Name { get; set; } // Name
        public string Alias { get; set; } // Alias
        public DateTime CreatedOn { get; set; } // CreatedOn
        public DateTime? ModifiedOn { get; set; } // ModifiedOn
        public string Description { get; set; } // Description
        public bool Status { get; set; }
        public int OrderNumber { get; set; }
        public string ImagePath { get; set; }
        public Category()
        {
            CreatedOn = System.DateTime.Now;
            InitializePartial();
        }
        partial void InitializePartial();
    }

}
