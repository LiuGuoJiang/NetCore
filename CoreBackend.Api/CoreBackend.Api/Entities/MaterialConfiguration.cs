using CoreBackend.Api.Dtos;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CoreBackend.Api.Entities
{
    public class MaterialConfiguration:IEntityTypeConfiguration<Material>
    {
        public void Configure(EntityTypeBuilder<Material> builder)
        {
            builder.HasKey(x => x.Id);
            builder.Property(x => x.Name).IsRequired().HasMaxLength(50);
            builder.HasOne(x => x.Product).WithMany(x => x.Materials).HasForeignKey(x => x.ProductId).OnDelete(DeleteBehavior.Cascade);
        }
    }
}
