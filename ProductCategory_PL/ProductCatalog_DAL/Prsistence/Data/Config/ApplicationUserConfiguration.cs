using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ProductCatalog_DAL.Models.IdentityModel;

namespace ProductCatalog_DAL.Prsistence.Data.Config
{
    internal class ApplicationUserConfiguration : IEntityTypeConfiguration<ApplicationUser>
    {
        public void Configure(EntityTypeBuilder<ApplicationUser> builder)
        {
            builder.HasKey(e => e.Id).HasName("PK__User__3214EC07C842DC7C");

            builder.Property(x => x.firstName).IsRequired();
            builder.Property(x => x.lastName).IsRequired();
            builder.Property(x => x.country).IsRequired();



            builder.Property(e => e.Id).ValueGeneratedOnAdd();
            builder.Property(e => e.country).HasMaxLength(50);
            builder.Property(e => e.Email).HasMaxLength(255);
            builder.Property(e => e.firstName).HasMaxLength(100);
            builder.Property(e => e.lastName).HasMaxLength(50);
            builder.Property(e => e.PhoneNumber).HasMaxLength(20);

        }
    }
}
