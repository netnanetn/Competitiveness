using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using Falcon.Data;
using Falcon.Data.Domain;

namespace Falcon.Data.Map
{
    // Pages
    internal partial class CategoryMap : EntityTypeConfiguration<Category>
    {
        public CategoryMap(string schema = "dbo")
        {
            ToTable(schema + ".Categories");
            HasKey(x => x.Id);
            Property(x => x.Id).HasColumnName("Id").IsRequired().HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);
            Property(x => x.Name).HasColumnName("Name").IsRequired().HasMaxLength(255);
            Property(x => x.Alias).HasColumnName("Alias").IsRequired().HasMaxLength(150);
            Property(x => x.CreatedOn).HasColumnName("CreatedOn").IsRequired();
            Property(x => x.ModifiedOn).HasColumnName("ModifiedOn").IsOptional();
            Property(x => x.Description).HasColumnName("Description").IsOptional().HasMaxLength(255);
            Property(x => x.Status).HasColumnName("Status").IsRequired();
            Property(x => x.OrderNumber).HasColumnName("OrderNumber").IsRequired();
            Property(x => x.ImagePath).HasColumnName("ImagePath").IsOptional().HasMaxLength(255);
            InitializePartial();
        }
        partial void InitializePartial();
    }

}
