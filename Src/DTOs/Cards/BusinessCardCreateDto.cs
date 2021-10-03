using System;

namespace Contactr.DTOs.Cards
{
    public class BusinessCardCreateDto
    {
        public Guid CompanyId { get; set; }
        public Guid AddressId { get; set; }
        public string? JobTitle { get; set; }
        public string? Email { get; set; }
        public string? Phone { get; set; }
        public string? Avatar { get; set; }
    }
}