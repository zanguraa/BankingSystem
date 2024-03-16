using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace BankingSystem.Core.Shared
{
    public class JwtTokenGenerator
    {
        private readonly IConfiguration configuration;

        public JwtTokenGenerator(IConfiguration configuration)
        {
            this.configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
        }

        public string Generate(string userId, string userRole)
        {
            var claims = new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.Sub, userId),
                new Claim(ClaimTypes.Role, userRole)
            };

            return GenerateToken(claims);
        }

        public string GenerateTokenForAtmOperations(string cardNumber, string pinCode)
        {
            var claims = new List<Claim>
    {
        new Claim("CardNumber", cardNumber),
        // Since the pinCode is sensitive, ensure you're handling it securely and consider if it should be included
        new Claim("PinCode", pinCode),
        // Add the required claim for the "CardHolder" policy
        new Claim("CardHolderStatus", "Active")
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
                expires: DateTime.Now.AddMinutes(3600), // Adjust token lifetime as necessary
                signingCredentials: credentials);

            var tokenGenerator = new JwtSecurityTokenHandler();
            var jwt = tokenGenerator.WriteToken(token);
            return jwt;
        }
    }
}
