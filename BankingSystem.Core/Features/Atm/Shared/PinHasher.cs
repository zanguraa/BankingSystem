using Microsoft.IdentityModel.Tokens;
using System.Security.Cryptography;
using System.Text;

namespace BankingSystem.Core.Features.Atm.Shared;

public interface IPinHasher
{
    string HashHmacSHA256(string password);
}

public class PinHasher : IPinHasher
{

    public string HashHmacSHA256(string password)
    {
        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(password));

        using var hmac = new HMACSHA256(key.Key);

        byte[] hashBytes = hmac.ComputeHash(Encoding.UTF8.GetBytes(password));
        return Convert.ToBase64String(hashBytes);
    }
}

