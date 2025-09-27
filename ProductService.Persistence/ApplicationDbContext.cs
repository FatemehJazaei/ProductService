using System;
using Microsoft.EntityFrameworkCore;
using ProductService.Domain.Entities;

namespace ProductService.Persistence
{
        public class ApplicationDbContext : DbContext
        {
            public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
            {
            }

        // DbSets
        public DbSet<Product> Products { get; set; }
            public DbSet<User> Users { get; set; }

            protected override void OnModelCreating(ModelBuilder builder)
            {
                base.OnModelCreating(builder);

                // Fluent API configurations
                builder.ApplyConfigurationsFromAssembly(typeof(ApplicationDbContext).Assembly);
            }
        }
}
