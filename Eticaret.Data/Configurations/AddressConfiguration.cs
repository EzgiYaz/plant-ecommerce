using Eticaret.Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Eticaret.Data.Configurations
{
    public class AddressConfiguration : IEntityTypeConfiguration<Address>
    {
        public void Configure(EntityTypeBuilder<Address> b)
        {
            b.Property(x => x.Title).HasMaxLength(100).IsRequired();
            b.Property(x => x.FullName).HasMaxLength(120).IsRequired();
            b.Property(x => x.Phone).HasMaxLength(30).IsRequired();
            b.Property(x => x.City).HasMaxLength(60).IsRequired();
            b.Property(x => x.District).HasMaxLength(60).IsRequired();
            b.Property(x => x.Neighborhood).HasMaxLength(120);
            b.Property(x => x.AddressLine).HasMaxLength(500).IsRequired();
            b.Property(x => x.PostalCode).HasMaxLength(15);

            b.HasOne(x => x.AppUser)
             .WithMany()
             .HasForeignKey(x => x.AppUserId)
             .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
