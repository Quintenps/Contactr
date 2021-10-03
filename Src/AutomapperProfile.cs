using AutoMapper;
using Contactr.DTOs;
using Contactr.DTOs.Cards;
using Contactr.Models;
using Contactr.Models.Cards;

namespace Contactr
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<PersonalCard, PersonalCardDto>();
            CreateMap<BusinessCard, BusinessCardDto>();
            CreateMap<Company, CompanyDto>();
            CreateMap<Address, AddressDto>();
        }
    }
}