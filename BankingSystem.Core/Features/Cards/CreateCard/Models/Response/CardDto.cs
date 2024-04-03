
namespace BankingSystem.Core.Features.Cards.CreateCard.Models.Response
{
    public class CardDto
    {
        public string CardNumber { get; set; }
        public string FullName { get; set; }
        public string Pin { get; set; }
        public DateTime ExpirationDate { get; set; }
    }
}
