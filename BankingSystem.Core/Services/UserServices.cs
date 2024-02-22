using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BankingSystem.Core.Data;
using BankingSystem.Core.Models.Requests;

namespace BankingSystem.Core.Services
{
	public class UserService
	{//  in-memory საცავი მომხმარებლებისთვის 
		private readonly List<UserEntity> _users = new List<UserEntity>();

		public async Task<UserEntity> RegisterUser(AccountRegisterRequest registerRequest)
		{
			// ამოწმებს ემაილის უნიკალურობას
			if (_users.Any(u => u.Email == registerRequest.Email))
			{
				throw new ApplicationException("Email is already registered");
			}

			// ჰეშავს პაროლს
			string hashedPassword = HashPassword(registerRequest.Password);


			var newUser = new UserEntity
			{
				FirstName = registerRequest.FirstName,
				LastName = registerRequest.LastName,
				PersonalNumber = registerRequest.PersonalNumber,
				BirthdayDate = registerRequest.BirthdayDate,
				Email = registerRequest.Email,
				Password = hashedPassword
			};

			// ახალ იუზერს ანიჭებს უნიკალურ userId ს
			newUser.Id = _users.Count + 1;

			// იუზერს ამატებს საცავში
			_users.Add(newUser);

			return newUser;
		}

		public async Task<UserEntity> GetUserByEmail(string email)
		{
			// იუზერს პოულობს ემაილით
			return _users.FirstOrDefault(u => u.Email == email);
		}

		private string HashPassword(string password)
		{
			return password;
		}
	}
}

