namespace BankingSystem.Api.Controllers
{
    public class RegisterUserRequest
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string PasswordConfirmed { get; set; }
        public string PhoneNumber { get; set; }
        public DateTime BirthdayDate { get; set; }
        public string PersonalId { get; set; }

    }
}