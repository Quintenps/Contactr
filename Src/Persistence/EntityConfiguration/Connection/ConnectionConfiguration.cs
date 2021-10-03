using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Contactr.Persistence.EntityConfiguration.Connection
{
    public class ConnectionConfiguration : IEntityTypeConfiguration<Models.Connection.Connection>
    {
        public void Configure(EntityTypeBuilder<Models.Connection.Connection> builder)
        {
            builder.HasOne(c => c.SenderUser).WithMany().OnDelete(DeleteBehavior.NoAction);
            builder.HasOne(c => c.ReceiverUser).WithMany().OnDelete(DeleteBehavior.NoAction);
        }
    }
}