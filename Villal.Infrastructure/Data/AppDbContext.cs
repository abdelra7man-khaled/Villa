using Microsoft.EntityFrameworkCore;
using Villal.Domain.Entities;

namespace Villal.Infrastructure.Data
{
    public class AppDbContext : DbContext
    {
        public DbSet<Villa> Villas { get; set; }
        public DbSet<VillaNumber> VillaNumbers { get; set; }


        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            //base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<Villa>().HasData(
                  new Villa
                  {
                      Id = 1,
                      Name = "Royal Villa",
                      Description = "Fusce 11 tincidunt maximus leo, sed scelerisque massa auctor sit amet. Donec ex mauris, hendrerit quis nibh ac, efficitur fringilla enim.",
                      ImageUrl = "https://placehold.co/600x400",
                      Occupancy = 4,
                      Price = 200,
                      Sqft = 550,
                  },
                new Villa
                {
                    Id = 2,
                    Name = "Premium Pool Villa",
                    Description = "Fusce 11 tincidunt maximus leo, sed scelerisque massa auctor sit amet. Donec ex mauris, hendrerit quis nibh ac, efficitur fringilla enim.",
                    ImageUrl = "https://placehold.co/600x401",
                    Occupancy = 4,
                    Price = 300,
                    Sqft = 550,
                },
                new Villa
                {
                    Id = 3,
                    Name = "Luxury Pool Villa",
                    Description = "Fusce 11 tincidunt maximus leo, sed scelerisque massa auctor sit amet. Donec ex mauris, hendrerit quis nibh ac, efficitur fringilla enim.",
                    ImageUrl = "https://placehold.co/600x402",
                    Occupancy = 4,
                    Price = 400,
                    Sqft = 750,
                });

            modelBuilder.Entity<VillaNumber>().HasData(
                new VillaNumber
                {
                    VillaNo = 101,
                    VillaId = 1,
                },
                new VillaNumber
                {
                    VillaNo = 102,
                    VillaId = 1,
                },
                new VillaNumber
                {
                    VillaNo = 103,
                    VillaId = 1,
                },
                new VillaNumber
                {
                    VillaNo = 104,
                    VillaId = 1,
                },
                new VillaNumber
                {
                    VillaNo = 201,
                    VillaId = 2,
                },
                new VillaNumber
                {
                    VillaNo = 202,
                    VillaId = 2,
                },
                new VillaNumber
                {
                    VillaNo = 203,
                    VillaId = 2,
                },
                new VillaNumber
                {
                    VillaNo = 301,
                    VillaId = 3,
                },
                new VillaNumber
                {
                    VillaNo = 302,
                    VillaId = 3,
                });
        }
    }
}
