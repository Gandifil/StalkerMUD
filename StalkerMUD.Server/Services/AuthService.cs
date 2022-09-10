using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using StalkerMUD.Common.Models;
using StalkerMUD.Server.Data;
using StalkerMUD.Server.Entities;
using StalkerMUD.Server.Models;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace StalkerMUD.Server.Services
{
    public interface IAuthService
    {
        Task<AuthenticateResponse> LoginAsync(AuthenticateRequest request);
        Task<AuthenticateResponse> RegisterAsync(AuthenticateRequest request);
    }

    public class AuthService : IAuthService
    {
        private readonly JwtSettings _settings;
        private readonly ILogger _logger;
        private readonly IRepository<User> _users;

        public AuthService(IOptions<JwtSettings> settings, ILogger<AuthService> logger, IRepository<User> users)
        {
            _settings = settings.Value;
            _logger = logger;
            _users = users;
        }

        public async Task<AuthenticateResponse> RegisterAsync(AuthenticateRequest request)
        {
            if (request.Username?.Length < 5 ||
                request.Password?.Length < 5)
                throw new InvalidDataException();

            var user = new User()
            {
                Name = request.Username,
                PasswordHash = GeneratePasswordHash(request)
            };
            user.Id = await _users.InsertAsync(user);
            return new AuthenticateResponse()
            {
                ID = user.Id,
                Token = GenerateToken(user)
            };
        }

        public async Task<AuthenticateResponse> LoginAsync(AuthenticateRequest request)
        {
            var user = await _users.SelectSingleAsync(user => user.Name == request.Username);
            if (user.PasswordHash == GeneratePasswordHash(request))
                return new AuthenticateResponse()
                {
                    ID = user.Id,
                    Token = GenerateToken(user)
                };

            throw new AccessViolationException("");
        }

        private static string GeneratePasswordHash(AuthenticateRequest request)
        {
            return Convert.ToBase64String(KeyDerivation.Pbkdf2(
                                password: request.Password,
                                salt: Encoding.UTF8.GetBytes("12345678"),
                                prf: KeyDerivationPrf.HMACSHA256,
                                iterationCount: 100000,
                                numBytesRequested: 256 / 8));
        }

        private string GenerateToken(User user)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_settings.Secret);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[] { new Claim("id", user.Id.ToString()) }),
                Expires = DateTime.UtcNow.AddDays(7),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature),
                Audience = _settings.Audience,
                Issuer = _settings.Issuer,
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }
    }
}
