using Contactr.Models.Authentication;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Contactr.Persistence.EntityConfiguration.Authentication
{
    public class AuthenticationProviderConfiguration : IEntityTypeConfiguration<AuthenticationProvider>
    {
        public void Configure(EntityTypeBuilder<AuthenticationProvider> builder)
        {
            builder.HasIndex(ap => new {ap.LoginProvider, ap.UserId}).IsUnique();
            builder.HasOne(ap => ap.User);
        }
    }
}