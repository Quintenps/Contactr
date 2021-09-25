using AutoMapper;
using Contactr.DTOs.Cards;
using Contactr.Models.Cards;

namespace Contactr
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<PersonalCard, PersonalCardDto>();
        }
    }
}