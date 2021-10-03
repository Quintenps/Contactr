using Contactr.Models.Cards;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Contactr.Persistence.EntityConfiguration.Cards
{
    public class BusinessCardConfiguration : IEntityTypeConfiguration<BusinessCard>
    {
        public void Configure(EntityTypeBuilder<BusinessCard> builder)
        {
            builder.HasOne(bc => bc.User);

            builder
                .HasOne(bc => bc.Address)
                .WithMany()
                .HasForeignKey(bc => bc.AddressId)
                .HasPrincipalKey(a => a.Id)
                .OnDelete(DeleteBehavior.NoAction);

            builder
                .HasOne(bc => bc.Company)
                .WithMany()
                .HasForeignKey(bc => bc.CompanyId)
                .HasPrincipalKey(c => c.Id)
                .OnDelete(DeleteBehavior.NoAction);
        }
    }
}