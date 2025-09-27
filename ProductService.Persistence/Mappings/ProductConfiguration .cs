using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ProductService.Domain.Entities;

namespace ProductService.Persistence.Mappings
{
        public class ProductConfiguration : IEntityTypeConfiguration<Product>
        {
            public void Configure(EntityTypeBuilder<Product> builder)
            {

                builder.HasKey(p => p.Id);

                builder.Property(p => p.Name)
                       .IsRequired()
                       .HasMaxLength(200)
                       .HasColumnType("nvarchar(200)");


                builder.Property(p => p.IsAvailable)
                       .IsRequired()
                       .HasColumnType("bit");

                builder.Property(p => p.ManufactureEmail)
                       .IsRequired()
                       .HasMaxLength(100)
                       .HasColumnType("nvarchar(100)");

                builder.Property(p => p.ManufacturePhone)
                       .HasMaxLength(20)
                       .HasColumnType("nvarchar(20)");

                builder.Property(p => p.ProduceDate)
                       .IsRequired()
                       .HasColumnType("date");

                builder.HasIndex(p => new { p.ManufactureEmail, p.ProduceDate })
                       .IsUnique();

                builder.HasOne<User>()
                       .WithMany()
                       .HasForeignKey("CreatedByUserId") 
                       .IsRequired();

                builder.Property(p => p.CreatedAt)
                       .IsRequired()
                       .HasColumnType("datetime2(0)");

                builder.Property(p => p.UpdatedAt)
                       .HasColumnType("datetime2(0)");
            }
        }
}
