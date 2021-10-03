using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using AutoMapper;
using Contactr.DTOs.Cards;
using Contactr.Services.CardService;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace Contactr.Controllers.Cards
{
    [Route("api/[controller]")]
    [ApiController]
    public class BusinessCardController : ControllerBase
    {
        private readonly ILogger<BusinessCardController> _logger;
        private readonly IMapper _mapper;
        private readonly ICardService _cardService;

        public BusinessCardController(ILogger<BusinessCardController> logger, ICardService cardService, IMapper mapper)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _cardService = cardService ?? throw new ArgumentNullException(nameof(cardService));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }
        
        [HttpGet]
        public IEnumerable<BusinessCardDto> GetALl()
        {
            Guid userId = new Guid(User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? throw new InvalidOperationException());
            var businessCards = _cardService.GetBusinessCards(userId);
            return _mapper.Map<IEnumerable<BusinessCardDto>>(businessCards);
        }

        [HttpGet("{cardId:guid}")]
        public BusinessCardDto Get(Guid cardId)
        {
            Guid userId = new Guid(User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? throw new InvalidOperationException());
            var personalCard = _cardService.GetBusinessCard(userId, cardId);
            return _mapper.Map<BusinessCardDto>(personalCard);
        }

        [HttpPost]
        public async Task<ActionResult> Create(BusinessCardCreateDto businessCardDto)
        {
            Guid userId = new Guid(User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? throw new InvalidOperationException());
            await _cardService.CreateBusinessCard(userId, businessCardDto);

            return Ok();
        }
        
        [HttpPut("{cardId:guid}")]
        public async Task<ActionResult> Update(Guid cardId, BusinessCardCreateDto businessCardDto)
        {
            Guid userId = new Guid(User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? throw new InvalidOperationException());
            await _cardService.UpdateBusinessCard(userId, cardId, businessCardDto);

            return Ok();
        }

        [HttpDelete("{cardId:guid}")]
        public async Task<ActionResult> Delete(Guid cardId)
        {
            Guid userId = new Guid(User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? throw new InvalidOperationException());
            await _cardService.DeleteBusinessCard(userId, cardId);

            return Ok();
        }
    }
}