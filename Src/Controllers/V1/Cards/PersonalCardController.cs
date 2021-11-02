using System;
using System.Threading.Tasks;
using AutoMapper;
using Contactr.DTOs;
using Contactr.DTOs.Cards;
using Contactr.Services.CardService;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
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

        [HttpPost]
        public async Task<ActionResult> Create(PersonalCardDto personalCardDto)
        {
            var userId = new Guid(User.FindFirst("https://contactr/claims/uuid").Value ?? throw new InvalidOperationException());
            await _cardService.CreatePersonalCard(userId, personalCardDto);

            return Ok();
        }

        [HttpGet]
        public PersonalCardDto Get()
        {
            var userId = new Guid(User.FindFirst("https://contactr/claims/uuid").Value ?? throw new InvalidOperationException());
            var personalCard = _cardService.GetPersonalCard(userId);
            return _mapper.Map<PersonalCardDto>(personalCard);
        }

        [HttpPut]
        public async Task<ActionResult> Put(PersonalCardDto personalCardDto)
        {
            var userId = new Guid(User.FindFirst("https://contactr/claims/uuid").Value ?? throw new InvalidOperationException());
            await _cardService.UpdatePersonalCard(userId, personalCardDto);

            return Ok();
        }

        [HttpPost("avatar")]
        public async Task<ActionResult> UploadAvatar([FromForm] IFormFile avatar)
        {
            var userId = new Guid(User.FindFirst("https://contactr/claims/uuid").Value ?? throw new InvalidOperationException());
            try
            {
                await _cardService.UploadAvatar(userId, avatar);
            }
            catch
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Something went wrong");
            }
            
            return Ok();
        }
    }
}