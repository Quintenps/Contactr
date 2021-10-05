using System;
using System.Collections.Generic;
using Contactr.Models.Cards;

namespace Contactr.Models
{
    public class User
    {
        public Guid Id { get; set; }
        public string Email { get; set; } = null!;
        public string? Avatar { get; set; }
        public virtual PersonalCard PersonalCard { get; set; } = null!;
        public virtual IEnumerable<BusinessCard> BusinessCards { get; set; } = null!;
    }
}