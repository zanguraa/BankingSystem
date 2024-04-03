using BankingSystem.Core.Features.Users.CreateUser.Models.response;
using System.Text.Json.Serialization;

namespace BankingSystem.Core.Features.Users.CreateUser.Models.Requests
{
    public class RegisterUserRequest
    {
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? Email { get; set; }
        public string? Password { get; set; }
        public string? PasswordConfirmed { get; set; }
        public string? PhoneNumber { get; set; }
        public DateTime BirthdayDate { get; set; }
        public string? PersonalId { get; set; }
        [JsonIgnore]
        public DateTime RegistrationDate { get; set; } = DateTime.UtcNow;

        public RegisteredUserResponse RegisteredUserResponse()
        {
            return new RegisteredUserResponse
            {
                FullName = FirstName  +  LastName,
                Email = Email,
                Message = "New user has created successfully"
            };
            
        }
    }
}