using Microsoft.EntityFrameworkCore;

namespace Macska
{
    public class MacskaContext : DbContext
    {
        public DbSet<Macska> Macskak { get; set; }
        public DbSet<Fajtak> Fajtak { get; set; }
        public DbSet<Szarmazas> Szarmazas { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(
                "Server=(localdb)\\MSSQLLocalDB;AttachDbFilename=D:\\Programok\\WPF\\Macska\\Macska\\Fajta.mdf;Integrated Security=True;Connect Timeout=30");
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Macska>(entity =>
            {
                entity.ToTable("Cat");
                entity.HasKey(e => e.CatId);

                entity.Property(e => e.CatId).HasColumnName("CatId");
                entity.Property(e => e.Name).HasColumnName("Name");
                entity.Property(e => e.Age).HasColumnName("Age");
                entity.Property(e => e.Gender).HasColumnName("Gender");
                entity.Property(e => e.BreedId).HasColumnName("BreedId");

                entity.HasOne(e => e.Fajta)
                      .WithMany(f => f.Macskak)
                      .HasForeignKey(e => e.BreedId)
                      .OnDelete(DeleteBehavior.NoAction);
            });

            modelBuilder.Entity<Fajtak>(entity =>
            {
                entity.ToTable("Breed");
                entity.HasKey(e => e.BreedId);

                entity.Property(e => e.BreedId).HasColumnName("BreedId");
                entity.Property(e => e.BreedName).HasColumnName("BreedName");
                entity.Property(e => e.AvWeight).HasColumnName("AvWeight");
                entity.Property(e => e.LifeSpan).HasColumnName("LifeSpan");
                entity.Property(e => e.Description).HasColumnName("Description");
                entity.Property(e => e.FurLenght).HasColumnName("FurLenght");
                entity.Property(e => e.Personality).HasColumnName("Personality");
                entity.Property(e => e.ImgPath).HasColumnName("ImgPath");
                entity.Property(e => e.CountryId).HasColumnName("CountryId");

                entity.HasOne(e => e.Szarmazas)
                      .WithMany(sz => sz.OriginCountryId)
                      .HasForeignKey(e => e.CountryId)
                      .OnDelete(DeleteBehavior.NoAction);
            });

            modelBuilder.Entity<Szarmazas>(entity =>
            {
                entity.ToTable("Country");
                entity.HasKey(e => e.CountryId);

                entity.Property(e => e.CountryId).HasColumnName("CountryId");
                entity.Property(e => e.CountryName).HasColumnName("CountryName");
                entity.Property(e => e.Continent).HasColumnName("Continent");
            });
        }
    }
}