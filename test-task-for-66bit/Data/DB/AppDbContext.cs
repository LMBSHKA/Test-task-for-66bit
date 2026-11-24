using Microsoft.EntityFrameworkCore;
using Models.Entities;

namespace Data.DB
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        public DbSet<Country> Countries => Set<Country>();
        public DbSet<EquipmentItem> EquipmentItems => Set<EquipmentItem>();
        public DbSet<EquipmentType> EquipmentTypes => Set<EquipmentType>();
        public DbSet<Manufacturer> Manufacturers => Set<Manufacturer>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            var item = modelBuilder.Entity<EquipmentItem>();

            item.HasOne(x => x.EquipmentType)
                .WithMany(x => x.EquipmentItems)
                .HasForeignKey(x => x.EquipmentTypeId)
                .OnDelete(DeleteBehavior.Restrict);

            item.HasOne(x => x.Manufacturer)
                .WithMany(x => x.EquipmentItems)
                .HasForeignKey(x => x.ManufacturerId)
                .OnDelete(DeleteBehavior.Restrict);

            item.HasOne(x => x.SupplyCountry)
                .WithMany(x => x.EquipmentItems)
                .HasForeignKey(x => x.SupplyCountryId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
