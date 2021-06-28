using BackEndAPI.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BackEndAPI.DBContext
{
    public class AssetCategoryConfiguration : IEntityTypeConfiguration<AssetCategory>
    {
        public void Configure(EntityTypeBuilder<AssetCategory> builder)
        {
            builder.HasKey(e => e.Id);

            builder.Property(e => e.Id)
                    .ValueGeneratedOnAdd();

            builder.Property(e => e.CategoryCode)
                    .IsRequired()
                    .HasMaxLength(10);

            builder.Property(e => e.CategoryName)
                    .IsRequired()
                    .HasMaxLength(30);

            builder.HasData(
                        new AssetCategory
                        {
                            Id = 1,
                            CategoryName = "Laptop",
                            CategoryCode = "LA"
                        },
                        new AssetCategory
                        {
                            Id = 2,
                            CategoryName = "PC",
                            CategoryCode = "PC"
                        }
                        );
        }
    }
}