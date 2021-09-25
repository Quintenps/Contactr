using Contactr.Models.Connection.Resources;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Contactr.Persistence.EntityConfiguration.Connection.Resources
{
    public class GoogleConfiguration : IEntityTypeConfiguration<Models.Connection.Resources.Google>
    {
        public void Configure(EntityTypeBuilder<Models.Connection.Resources.Google> builder)
        {

        }
    }
}