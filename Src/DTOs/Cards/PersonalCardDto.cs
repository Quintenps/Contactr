using System;
using Contactr.Models.Enums;

namespace Contactr.DTOs.Cards
{
    public class PersonalCardDto
    {
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
    }
}