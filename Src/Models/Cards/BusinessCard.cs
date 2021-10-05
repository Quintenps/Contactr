using System;

namespace Contactr.Models.Cards
{
    public class BusinessCard
    {
        public Guid Id { get; set; }
        public Guid UserId { get; set; }
        public User User { get; set; } = null!;
        public Guid CompanyId { get; set; }
        public virtual Company Company { get; set; } = null!;
        public Guid AddressId { get; set; }
        public virtual Address Address { get; set; } = null!;
        public string? JobTitle { get; set; }
        public string? Email { get; set; }
        public string? Phone { get; set; }
        public string? Avatar { get; set; }
    }
}