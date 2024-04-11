using BankingSystem.Core.Features.Users.CreateUser.Models.response;
using System.Text.Json.Serialization;

namespace BankingSystem.Core.Features.Users.CreateUser.Models.Requests
{
    public class RegisterUserRequest
    {
        public required string FirstName { get; set; }
        public required string LastName { get; set; }
        public required string Email { get; set; }
        public required string Password { get; set; }
        public required string PasswordConfirmed { get; set; }
        public required string PhoneNumber { get; set; }
        public DateTime BirthdayDate { get; set; }
        public required string PersonalId { get; set; }
        [JsonIgnore]
        public DateTime RegistrationDate { get; set; } = DateTime.UtcNow;

        public RegisteredUserResponse RegisteredUserResponse()
        {
            return new RegisteredUserResponse
            {
                Message = "New user has created successfully",
                FullName = FirstName + " " + LastName,
                Email = Email
            };

        }
    }
}