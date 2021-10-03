﻿using System;

namespace Contactr.DTOs.Cards
{
    public class BusinessCardDto
    {
        public Guid Id { get; set; }
        public CompanyDto Company { get; set; }
        public AddressDto Address { get; set; }
        public string? JobTitle { get; set; }
        public string? Email { get; set; }
        public string? Phone { get; set; }
        public string? Avatar { get; set; }
    }
}