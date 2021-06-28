using System;
using BackEndAPI.Entities;
using BackEndAPI.Enums;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BackEndAPI.DBContext
{
    public class AssetConfiguration : IEntityTypeConfiguration<Asset>
    {
        public void Configure(EntityTypeBuilder<Asset> builder)
        {
            builder.HasKey(e => e.Id);

            builder.Property(e => e.Id)
                    .ValueGeneratedOnAdd();

            builder.Property(e => e.AssetCode)
                    .IsRequired()
                    .HasMaxLength(30);

            builder.Property(e => e.AssetName)
                    .IsRequired()
                    .HasMaxLength(100);

            builder.Property(e => e.CategoryId)
                    .IsRequired();

            builder.Property(e => e.State)
                    .IsRequired();

            builder.Property(e => e.Location)
                    .IsRequired();

            builder.HasOne(a => a.Category)
                    .WithMany(c => c.Assets);

            builder.HasData(
                    new Asset
                    {
                        Id = 1,
                        AssetCode = "LA000001",
                        AssetName = "Laptop 1",
                        CategoryId = 1,
                        Specification = "Balls-to-the-walls laptop, specced with the latest CPU and GPU",
                        InstalledDate = new DateTime(2020, 11, 1),
                        Location = Location.HaNoi,
                        State = AssetState.NotAvailable
                    },
                    new Asset
                    {
                        Id = 2,
                        AssetCode = "LA000002",
                        AssetName = "Laptop 2",
                        CategoryId = 1,
                        Specification = "An even more balls-to-the-walls laptop, specced with even better CPU and GPU than laptop 1",
                        InstalledDate = new DateTime(2021, 1, 1),
                        Location = Location.HaNoi,
                        State = AssetState.NotAvailable
                    },
                    new Asset
                    {
                        Id = 3,
                        AssetCode = "PC000001",
                        AssetName = "PC 1",
                        CategoryId = 2,
                        Specification = "Balls-to-the-walls desktop, designed for ultimate Word experience",
                        InstalledDate = new DateTime(2020, 11, 1),
                        Location = Location.HoChiMinh,
                        State = AssetState.Available
                    },
                    new Asset
                    {
                        Id = 4,
                        AssetCode = "PC000002",
                        AssetName = "PC 2",
                        CategoryId = 2,
                        Specification = "An even more balls-to-the-walls laptop, designed for the performant Excel workflow",
                        InstalledDate = new DateTime(2021, 1, 1),
                        Location = Location.HoChiMinh,
                        State = AssetState.NotAvailable
                    }
            );
        }
    }
}