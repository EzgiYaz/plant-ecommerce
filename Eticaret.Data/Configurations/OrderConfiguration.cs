using Eticaret.Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Eticaret.Data.Configurations
{
    public class OrderConfiguration : IEntityTypeConfiguration<Order>
    {
        public void Configure(EntityTypeBuilder<Order> b)
        {
            b.Property(x => x.OrderNumber).HasMaxLength(16);
            
            b.Property(x => x.TotalAmount).HasColumnType("money");

            b.Property(x => x.ShipFullName).HasMaxLength(120);
            b.Property(x => x.ShipPhone).HasMaxLength(30);
            b.Property(x => x.ShipCity).HasMaxLength(60);
            b.Property(x => x.ShipDistrict).HasMaxLength(60);
            b.Property(x => x.ShipAddressLine).HasMaxLength(500);
            b.Property(x => x.ShipPostalCode).HasMaxLength(15);

            b.HasOne(x => x.AppUser)
             .WithMany()
             .HasForeignKey(x => x.AppUserId)
             .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
