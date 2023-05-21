using Catalog.Models;
using Microsoft.EntityFrameworkCore;

namespace Catalog.Data
{
    public class CatalogContext : DbContext
    {
        public CatalogContext(DbContextOptions<CatalogContext> options)
            : base(options)
        {
            Database.EnsureCreated();
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<Category>()
                .HasIndex(category => category.Name)
                .IsUnique();

            builder.Entity<Item>()
                .HasIndex(item => item.Name)
                .IsUnique();

            SeedData(builder);
        }

        public DbSet<Category> Categories { get; set; }

        public DbSet<Item> Items { get; set; }

        private static void SeedData(ModelBuilder builder)
        {
            builder.Entity<Category>()
                .HasData(
                    new Category()
                    {
                        Id = 1,
                        Name = "Category 1",
                        Description = "Description 1"
                    },
                    new Category()
                    {
                        Id = 2,
                        Name = "Category 2",
                        Description = "Description 2"
                    });

            builder.Entity<Item>()
                .HasData(
                    new Item()
                    {
                        Id = 1,
                        Name = "Item 11",
                        Description = "Description 11",
                        CategoryId = 1,
                    },
                    new Item()
                    {
                        Id = 2,
                        Name = "Item 12",
                        Description = "Description 12",
                        CategoryId = 1,
                    },
                    new Item()
                    {
                        Id = 3,
                        Name = "Item 21",
                        Description = "Description 21",
                        CategoryId = 2,
                    },
                    new Item()
                    {
                        Id = 4,
                        Name = "Item 22",
                        Description = "Description 22",
                        CategoryId = 2,
                    });
        }
    }
}