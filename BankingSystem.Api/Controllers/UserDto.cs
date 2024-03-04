
namespace BankingSystem.Api.Controllers
{
    internal class UserDto
    {
        public string PersonalId { get; set; }
        public DateTime BirthdayDate { get; set; }
        public string Email { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string PhoneNumber { get; set; }
    }
}