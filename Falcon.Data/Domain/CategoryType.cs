using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using Falcon.Data;
using ProtoBuf;

namespace Falcon.Data.Domain
{
    // Pages
    [ProtoContract(ImplicitFields = ImplicitFields.AllPublic, ImplicitFirstTag = 1)]
    public partial class CategoryType : BaseEntity
    {
        public override int Id { get; set; } // Id (Primary key)
        public int TypeId { get; set; }
        public string Alias { get; set; }
        public string Name { get; set; } // Type
        

        public CategoryType()
        {
            InitializePartial();
        }
        partial void InitializePartial();
    }

}
