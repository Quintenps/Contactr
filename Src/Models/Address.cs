using System;

namespace Contactr.Models
{
    public class Address
    {
        public Guid Id { get; set; }
        public Guid CompanyId { get; set; }
        public virtual Company Company { get; set; } = null!;
        public string? Country { get; set; }
        public string? Street { get; set; }
        public string? Postalcode { get; set; }
        public string? City { get; set; }
    }
}