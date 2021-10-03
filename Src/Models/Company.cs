using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace Contactr.Models
{
    public class Company
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = null!;
        public string? Logo { get; set; } = null!; 
        public virtual ICollection<Address> Addresses { get; set; } = null!;
    }
}