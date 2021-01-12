using System;
using EFurni.Shared.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace EFurni.Infrastructure.Data
{
    public partial class LocationsContext : DbContext
    {
        public LocationsContext(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        private IConfiguration _configuration;
        public LocationsContext(DbContextOptions<LocationsContext> options, IConfiguration configuration)
            : base(options)
        {
            _configuration = configuration;
        }

        public virtual DbSet<Countries> Countries { get; set; }
        public virtual DbSet<Districts> Districts { get; set; }
        public virtual DbSet<Neighborhoods> Neighborhoods { get; set; }
        public virtual DbSet<Province> Province { get; set; }
        public virtual DbSet<ZipCodeLocationPair> ZipCodeLocationPairs { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                throw new Exception("Connection string not specified.");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<ZipCodeLocationPair>().HasNoKey();
            modelBuilder.Entity<Countries>(entity =>
            {
                entity.ToTable("countries");

                entity.HasIndex(e => e.CountryTag)
                    .HasName("countries_country_tag_uindex")
                    .IsUnique();

                entity.Property(e => e.Id)
                    .HasColumnName("id")
                    .ValueGeneratedNever();

                entity.Property(e => e.CountryTag)
                    .HasColumnName("country_tag")
                    .HasMaxLength(4);
            });

            modelBuilder.Entity<Districts>(entity =>
            {
                entity.HasKey(e => e.DistrictId)
                    .HasName("districts_pk");

                entity.ToTable("districts");

                entity.HasIndex(e => e.DistrictName)
                    .HasName("districts_district_name_index");

                entity.HasIndex(e => e.ProvinceId)
                    .HasName("districts_province_id_index");

                entity.Property(e => e.DistrictId)
                    .HasColumnName("district_id")
                    .ValueGeneratedNever();

                entity.Property(e => e.DistrictName)
                    .HasColumnName("district_name")
                    .HasMaxLength(128);

                entity.Property(e => e.ProvinceId).HasColumnName("province_id");

                entity.HasOne(d => d.Province)
                    .WithMany(p => p.Districts)
                    .HasForeignKey(d => d.ProvinceId)
                    .HasConstraintName("districts_province_province_id_fk");
            });

            modelBuilder.Entity<Neighborhoods>(entity =>
            {
                entity.HasKey(e => e.NeighborhoodId)
                    .HasName("neighborhoods_pk");

                entity.ToTable("neighborhoods");

                entity.HasIndex(e => e.DistrictId)
                    .HasName("neighborhoods_district_id_index");

                entity.HasIndex(e => e.PostalCode)
                    .HasName("neighborhoods_postal_code_index");

                entity.HasIndex(e => new { e.Longitude, e.Latitude })
                    .HasName("neighborhoods_longitude_latitude_index");

                entity.Property(e => e.NeighborhoodId).HasColumnName("neighborhood_id");

                entity.Property(e => e.DistrictId).HasColumnName("district_id");

                entity.Property(e => e.Latitude)
                    .HasColumnName("latitude")
                    .HasColumnType("numeric(10,8)");

                entity.Property(e => e.Longitude)
                    .HasColumnName("longitude")
                    .HasColumnType("numeric(11,8)");

                entity.Property(e => e.NeighborhoodName)
                    .HasColumnName("neighborhood_name")
                    .HasMaxLength(128);

                entity.Property(e => e.PostalCode)
                    .HasColumnName("postal_code")
                    .HasMaxLength(16);

                entity.HasOne(d => d.District)
                    .WithMany(p => p.Neighborhoods)
                    .HasForeignKey(d => d.DistrictId)
                    .HasConstraintName("neighborhoods_districts_district_id_fk");
            });

            modelBuilder.Entity<Province>(entity =>
            {
                entity.ToTable("province");

                entity.HasIndex(e => e.CountryId)
                    .HasName("province_country_id_index");

                entity.Property(e => e.ProvinceId).HasColumnName("province_id");

                entity.Property(e => e.CountryId).HasColumnName("country_id");

                entity.Property(e => e.ProvinceName)
                    .HasColumnName("province_name")
                    .HasMaxLength(128);

                entity.HasOne(d => d.Country)
                    .WithMany(p => p.Province)
                    .HasForeignKey(d => d.CountryId)
                    .HasConstraintName("province_countries_id_fk");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
