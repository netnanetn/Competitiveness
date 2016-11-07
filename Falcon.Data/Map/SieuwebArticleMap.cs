using Falcon.Data.Domain;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Falcon.Data.Map
{
    internal partial class SieuwebArticleMap : EntityTypeConfiguration<Article>
    {
        public SieuwebArticleMap(string schema = "dbo")
        {
            ToTable(schema + ".SupportSieuwebArticles");
            HasKey(x => x.Id);
            Property(x => x.Id).HasColumnName("Id").IsRequired().HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);
            Property(x => x.Name).HasColumnName("Name").IsRequired().HasMaxLength(255);
            Property(x => x.Alias).HasColumnName("Alias").IsRequired().HasMaxLength(150);
            Property(x => x.CategoryId).HasColumnName("CategoryId").IsRequired();
            Property(x => x.CreatedOn).HasColumnName("CreatedOn").IsRequired();
            Property(x => x.ModifiedOn).HasColumnName("ModifiedOn").IsOptional();
            Property(x => x.PublishedOn).HasColumnName("PublishedOn").IsOptional();
            Property(x => x.Status).HasColumnName("Status").IsRequired();
            Property(x => x.MetaTitle).HasColumnName("MetaTitle").IsRequired();
            Property(x => x.Keyword).HasColumnName("Keyword").IsOptional();
            Property(x => x.MetaDescription).HasColumnName("MetaDescription").IsRequired();
         

            InitializePartial();
        }
        partial void InitializePartial();
    }
}
