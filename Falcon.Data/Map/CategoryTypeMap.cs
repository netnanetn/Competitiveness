using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using Falcon.Data;
using Falcon.Data.Domain;

namespace Falcon.Data.Map
{
    // Pages
    internal partial class CategoryTypeMap : EntityTypeConfiguration<CategoryType>
    {
        public CategoryTypeMap(string schema = "dbo")
        {
            ToTable(schema + ".CategoryTypes");
            HasKey(x => x.Id);

            Property(x => x.Id).HasColumnName("Id").IsRequired().HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);
            Property(x => x.TypeId).HasColumnName("TypeId").IsRequired();
            Property(x => x.Name).HasColumnName("Name").IsRequired().HasMaxLength(100);
            Property(x => x.Alias).HasColumnName("Alias").IsRequired().HasMaxLength(100);
            
            InitializePartial();
        }
        partial void InitializePartial();
    }

}
