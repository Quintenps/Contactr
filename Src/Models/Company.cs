using System;
using System.Collections.Generic;

namespace Contactr.Models
{
    public class Company
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string? Logo { get; set; }
        public virtual ICollection<Address> Addresses { get; set; }
    }
}