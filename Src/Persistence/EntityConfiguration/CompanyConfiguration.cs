﻿using Contactr.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Contactr.Persistence.EntityConfiguration
{
    public class CompanyConfiguration : IEntityTypeConfiguration<Company>
    {
        public void Configure(EntityTypeBuilder<Company> builder)
        {
            builder.HasIndex(c => c.Name).IsUnique();
            builder.HasMany(c => c.Addresses).WithOne(c => c.Company);
        }
    }
}