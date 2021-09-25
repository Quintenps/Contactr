using System;

namespace Contactr.Models
{
    public class User
    {
        public Guid Id { get; set; }
        public string Email { get; set; }
        public string? Avatar { get; set; }
    }
}