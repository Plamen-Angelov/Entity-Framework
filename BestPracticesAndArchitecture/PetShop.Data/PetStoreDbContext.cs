using Microsoft.EntityFrameworkCore;
using PetShop.Common;
using PetStore.Models;
using System;

namespace PetShop.Data
{
    public class PetStoreDbContext : DbContext
    {
        public PetStoreDbContext()
        {

        }

        public PetStoreDbContext(DbContextOptions options)
            : base(options)
        {

        }

        public DbSet<Address> Addressss { get; set; }
        public DbSet<Breed> Breeds { get; set; }
        public DbSet<Card> Cards { get; set; }
        public DbSet<Client> Clients { get; set; }
        public DbSet<Pet> Pets { get; set; }
        public DbSet<PetReservation> PetReservations { get; set; }
        public DbSet<PetType> PetTypes { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<ProductSale> ProductSales { get; set; }
        public DbSet<ProductType> ProductTypes { get; set; }
        public DbSet<Service> Services { get; set; }
        public DbSet<Store> Stores { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer(DatabaseConfiguration.CONFIGURATION_STRING);
            }

            base.OnConfiguring(optionsBuilder);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder
                .Entity<ProductSale>(e =>
                {
                    e.HasKey(ps => new { ps.ProductId, ps.ClientId });
                });

            modelBuilder.Entity<PetReservation>()
                .HasOne(e => e.Store)
                .WithMany()
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Store>()
                .HasOne(e => e.Address)
                .WithMany()
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
