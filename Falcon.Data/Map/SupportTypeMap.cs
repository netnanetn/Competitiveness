using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using Falcon.Data;
using Falcon.Data.Domain;

namespace Falcon.Data.Map
{
    // Pages
    //internal partial class SupportTypeMap : EntityTypeConfiguration<SupportType>
    //{
    //    public SupportTypeMap(string schema = "dbo")
    //    {
    //        ToTable(schema + ".SupportTypes");
    //        HasKey(x => x.Id);

    //        Property(x => x.Id).HasColumnName("Id").IsRequired().HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);
    //        Property(x => x.Type).HasColumnName("Type").IsRequired().HasMaxLength(50);
            
    //        InitializePartial();
    //    }
    //    partial void InitializePartial();
    //}

}
