using Contactr.Models.Cards;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Contactr.Persistence.EntityConfiguration.Cards
{
    public class BusinessCardConfiguration : IEntityTypeConfiguration<BusinessCard>
    {
        public void Configure(EntityTypeBuilder<BusinessCard> builder)
        {
            builder.HasOne(bc => bc.User).WithMany();
        }
    }
}