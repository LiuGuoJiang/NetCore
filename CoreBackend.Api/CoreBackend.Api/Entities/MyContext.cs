using CoreBackend.Api.Dtos;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CoreBackend.Api.Entities
{
    public class MyContext:DbContext
    {
        public MyContext(DbContextOptions<MyContext> options) 
            : base(options)
        {
            Database.Migrate();
        }
        public DbSet<Product> Products { get; set; }
        public DbSet<Material> Materials { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new ProductConfiguration());
            modelBuilder.ApplyConfiguration(new MaterialConfiguration());
        }
    }
}
