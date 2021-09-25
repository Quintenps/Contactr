using Contactr.Models.Cards;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Contactr.Persistence.EntityConfiguration.Cards
{
    public class PersonalCardConfiguration : IEntityTypeConfiguration<PersonalCard>
    {
        public void Configure(EntityTypeBuilder<PersonalCard> builder)
        {
            builder.HasOne(pc => pc.User).WithOne();
        }
    }
}