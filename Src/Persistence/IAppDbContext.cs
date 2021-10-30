using Contactr.Models;
using Contactr.Models.Authentication;
using Contactr.Models.Cards;
using Contactr.Models.Connection;
using Microsoft.EntityFrameworkCore;

namespace Contactr.Persistence
{
    public interface IAppDbContext
    {
        public DbSet<User> Users { get; }
        public DbSet<Address> Addresses { get; }
        public DbSet<Company> Companies { get; }
        public DbSet<Connection> Connections { get; }
        public DbSet<PersonalCard> PersonalCards { get; }
        public DbSet<BusinessCard> BusinessCards { get; }
        public DbSet<AuthenticationProvider> AuthenticationProviders { get; set; }
    }
}