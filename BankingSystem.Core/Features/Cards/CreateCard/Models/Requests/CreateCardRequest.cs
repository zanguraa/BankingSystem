namespace BankingSystem.Core.Features.Cards.CreateCard.Models.Requests
{
    public class CreateCardRequest
    {
        public int UserId { get; set; }
        public int AccountId { get; set; }
    }
}
