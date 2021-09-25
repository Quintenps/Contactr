using System;

namespace Contactr.Models.Cards
{
    public class BusinessCard
    {
        public Guid Id { get; set; }
        public Guid UserId { get; set; }
        public virtual User User { get; set; }
        public Guid? CompanyId { get; set; }
        public virtual Company Company { get; set; }
        public Guid? AddressId { get; set; }
        public virtual Address Address { get; set; }
        public string? JobTitle { get; set; }
        public string? Email { get; set; }
        public string? Phone { get; set; }
        public string? Avatar { get; set; }
    }
}