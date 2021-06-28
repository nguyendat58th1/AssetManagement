using BackEndAPI.Entities;
using BackEndAPI.Enums;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BackEndAPI.DBContext
{
    public class ReturnRequestConfiguration : IEntityTypeConfiguration<ReturnRequest>
    {
        public void Configure(EntityTypeBuilder<ReturnRequest> builder)
        {

            builder.HasKey(e => e.Id);

            builder.Property(e => e.Id)
                    .ValueGeneratedOnAdd();

        //     builder.Property(e => e.AssignmentId)
        //             .IsRequired();

            builder.Property(e => e.RequestedByUserId)
                    .IsRequired();

            builder.Property(e => e.State)
                    .IsRequired();

        //     builder.HasOne(a => a.Assignment)
        //             .WithOne(c => c.Request);

            builder.HasOne(a => a.RequestedByUser)
                    .WithMany(c => c.Requests)
                    .OnDelete(DeleteBehavior.NoAction);
            
        }
    }
}