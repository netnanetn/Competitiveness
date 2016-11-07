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
    public partial class ArticleContent : BaseEntity
    {
        public override int Id { get; set; } // Id (Primary key)
        public string Content { get; set; } // Content
        

        public ArticleContent()
        {
            InitializePartial();
        }
        partial void InitializePartial();
    }

}
