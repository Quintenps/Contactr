using System;
using System.Security.Claims;
using System.Threading.Tasks;
using AutoMapper;
using Contactr.DTOs.Cards;
using Contactr.Services.CardService;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Contactr.Controllers.V1.Cards
{
    [Route("api/v1/cards/[controller]")]
    [ApiController]
    [Authorize]
    public class PersonalCardController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly ICardService _cardService;

        public PersonalCardController(ICardService cardService, IMapper mapper)
        {
            _cardService = cardService ?? throw new ArgumentNullException(nameof(cardService));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        [HttpGet]
        public PersonalCardDto Get()
        {
            Guid userId = new Guid(User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? throw new InvalidOperationException());
            var personalCard = _cardService.GetPersonalCard(userId);
            return _mapper.Map<PersonalCardDto>(personalCard);
        }
        
        [HttpPut]
        public async Task<ActionResult> Put(PersonalCardDto personalCardDto)
        {
            Guid userId = new Guid(User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? throw new InvalidOperationException());
            await _cardService.UpdatePersonalCard(userId, personalCardDto);

            return Ok();
        }
    }
}
