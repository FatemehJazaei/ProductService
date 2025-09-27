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
        public class UserConfiguration : IEntityTypeConfiguration<User>
        {
            public void Configure(EntityTypeBuilder<User> builder)
            {

                builder.HasKey(u => u.Id);


                builder.Property(u => u.UserName)
                       .IsRequired()
                       .HasMaxLength(50)          
                       .IsUnicode(false);         

                builder.Property(u => u.PasswordHash)
                       .IsRequired()
                       .HasMaxLength(128)         
                       .IsUnicode(false);         

                builder.Property(u => u.CreatedAt)
                       .HasColumnType("datetime2(0)")  
                       .IsRequired();


                builder.HasMany(u => u.Products)
                       .WithOne(p => p.CreatedByUser)
                       .HasForeignKey(p => p.CreatedByUserId)
                       .OnDelete(DeleteBehavior.Cascade);
            }
        }

}
