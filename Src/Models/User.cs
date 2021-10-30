using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using Contactr.Models.Cards;

namespace Contactr.Models
{
    [NotMapped]
    public class User
    {
        public Guid Id { get; set; }
        public string Auth0Id { get; set; }
        public virtual PersonalCard PersonalCard { get; set; } = null!;
        public virtual IEnumerable<BusinessCard> BusinessCards { get; set; } = null!;
    }
}