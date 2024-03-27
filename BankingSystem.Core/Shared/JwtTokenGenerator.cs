using BankingSystem.Core.Features.Atm.CardAuthorizations.Requests;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace BankingSystem.Core.Shared
{
    public interface IJwtTokenGenerator
    {
        string Generate(string userId, string userRole);
        string GenerateTokenForAtmOperations(CardAuthorizationRequest request);
    }

    public class JwtTokenGenerator : IJwtTokenGenerator
    {
        private readonly IConfiguration configuration;

        public JwtTokenGenerator(IConfiguration configuration)
        {
            this.configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
        }

        public string Generate(string id, string role)
        {
            var claims = new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.Sub, id),
                new Claim(ClaimTypes.Role, role)
            };

            return GenerateToken(claims);
        }

        public string GenerateTokenForAtmOperations(CardAuthorizationRequest request)
        {
            var claims = new List<Claim>
         {
               new Claim("CardNumber", request.CardNumber),
               new Claim(ClaimTypes.Role, "atm")
         };

            return GenerateToken(claims);
        }
        private string GenerateToken(List<Claim> claims)
        {
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["JwtTokenSecretKey"]!));
            var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: "myapp.com",
                audience: "myapp.com",
                claims: claims,
                expires: DateTime.Now.AddMinutes(3600),
                signingCredentials: credentials);

            var tokenGenerator = new JwtSecurityTokenHandler();
            var jwt = tokenGenerator.WriteToken(token);
            return jwt;
        }
    }
}
