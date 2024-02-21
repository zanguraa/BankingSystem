using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace BankingSystem.Core.Shared
{
	public class PasswordHasher
	{
		public static string HashHmacSHA256(string password)
		{
			var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(password));
			using var hmac = new HMACSHA256(key.Key);
			byte[] hashBytes = hmac.ComputeHash(Encoding.UTF8.GetBytes(password));
			return Convert.ToBase64String(hashBytes);
		}
	}
}
