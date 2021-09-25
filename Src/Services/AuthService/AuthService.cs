using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Contactr.DTOs.AuthenticationProvider;
using Contactr.Factories.Interfaces;
using Contactr.Models;
using Contactr.Models.Authentication;
using Contactr.Models.Cards;
using Contactr.Models.Enums;
using Contactr.Persistence;
using Contactr.Persistence.Repositories.Interfaces;
using Contactr.Services.Factories;
using Google.Apis.Auth;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;

namespace Contactr.Services.AuthService
{
    public class AuthService : IAuthService
    {
        private readonly IConfiguration _configuration;
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<AuthService> _logger;
        private readonly IUserFactory _userFactory;
        private readonly ICardFactory _cardFactory;
        private readonly IAuthenticationProviderFactory _authenticationProviderFactory;

        public AuthService(IConfiguration configuration, IUnitOfWork unitOfWork, ILogger<AuthService> logger, IUserFactory userFactory, ICardFactory cardFactory, IAuthenticationProviderFactory authenticationProviderFactory)
        {
            _configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
            _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _userFactory = userFactory ?? throw new ArgumentNullException(nameof(userFactory));
            _cardFactory = cardFactory ?? throw new ArgumentNullException(nameof(cardFactory));
            _authenticationProviderFactory = authenticationProviderFactory ?? throw new ArgumentNullException(nameof(authenticationProviderFactory));

            if (string.IsNullOrEmpty(_configuration.GetValue<string>("Jwt:Token")))
                throw new ArgumentException("Empty or null value", nameof(_configuration));
        }

        /// <summary>
        /// Creates JWT Token
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        private string CreateToken(User user)
        {
            var claims = new List<Claim>
            {
                new(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new(ClaimTypes.Email, user.Email)
            };

            var key = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(_configuration.GetSection("Jwt:Token").Value)
            );

            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.Now.AddDays(14),
                SigningCredentials = creds
            };

            var tokenHandler = new JwtSecurityTokenHandler();
            var token = tokenHandler.CreateToken(tokenDescriptor);

            return tokenHandler.WriteToken(token);
        }
        
        /// <summary>
        /// Creates an <see cref="AuthenticationProvider"/> and an <see cref="User"/> with <see cref="PersonalCard"/>
        /// </summary>
        /// <param name="payload"></param>
        /// <returns></returns>
        private async Task<User> Register(GoogleJsonWebSignature.Payload payload)
        {
            User user = _userFactory.Create(payload.Email, null);
            var personalCard = _cardFactory.CreatePersonalCard(user.Id);
            var authenticationProvider = _authenticationProviderFactory.Create(user.Id, payload.Subject, LoginProviders.Google);

            _unitOfWork.AuthenticationProviderRepository.Add(authenticationProvider);
            _unitOfWork.PersonalCardRepository.Add(personalCard);
            _unitOfWork.UserRepository.Add(user);
            await _unitOfWork.Save();
            
            return user;
        }
        
        public async Task<GoogleJsonWebSignature.Payload> VerifyGoogleToken(GoogleLoginDto externalAuth)
        {
            try
            {
                var settings = new GoogleJsonWebSignature.ValidationSettings
                {
                    Audience = new List<string> { _configuration.GetValue<string>("GoogleOAuth:clientId") }
                };
                var payload = await GoogleJsonWebSignature.ValidateAsync(externalAuth.IdToken, settings);
                return payload;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                throw;
            }
        }

        /// <summary>
        /// Creates a user and personal card or finds existing user
        /// Returns JWT
        /// </summary>
        /// <param name="payload"></param>
        /// <returns></returns>
        public async Task<string> Login(GoogleJsonWebSignature.Payload payload)
        {
            var authenticationProvider = _unitOfWork.AuthenticationProviderRepository.GetProviderWithUserOrDefault(payload.Subject);
            User user;
            if (authenticationProvider is null)
            {
                user = await Register(payload);
            }
            else
            {
                user = authenticationProvider.User;
            }

            return CreateToken(user);
        }
    }
}