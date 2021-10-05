using System;
using Contactr.Models.Enums;

namespace Contactr.Models.Cards
{
    public class PersonalCard
    {
        public Guid Id { get; set; }
        public Guid UserId { get; set; }
        public virtual User User { get; set; } = null!;
        public string? Firstname { get; set; }
        public string? Lastname { get; set; }
        public Genders? Gender { get; set; }
        public DateTime? Birthday { get; set; }
        public string? Email { get; set; }
        public string? Phone { get; set; }
        public string? Address { get; set; }
        public string? Country { get; set; }
        public string? Postalcode { get; set; }
        public string? City { get; set; }
        public string? Avatar { get; set; }

        public string GetFullName()
        {
            return $"{Firstname} {Lastname}";
        }
    }
}