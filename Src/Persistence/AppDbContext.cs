using Contactr.Models;
using Contactr.Models.Authentication;
using Contactr.Models.Cards;
using Contactr.Models.Connection;
using Microsoft.EntityFrameworkCore;

namespace Contactr.Persistence
{
    public class AppDbContext : DbContext, IAppDbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options)
        {
        }

        public DbSet<User> Users { get; set; }
        public DbSet<Address> Addresses { get; set; }
        public DbSet<Company> Companies { get; set; }
        public DbSet<Connection> Connections { get; set; }
        public DbSet<PersonalCard> PersonalCards { get; set; }
        public DbSet<BusinessCard> BusinessCards { get; set; }
        public DbSet<AuthenticationProvider> AuthenticationProviders { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.ApplyConfigurationsFromAssembly(typeof(AppDbContext).Assembly);
            base.OnModelCreating(builder);
        }
    }
}