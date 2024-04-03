using BankingSystem.Core.Features.Cards.CreateCard.Models.Response;
using System.Net.NetworkInformation;

namespace BankingSystem.Core.Shared.Models
{
    public class Card
    {
        public int Id { get; set; }
        public string Pin { get; set; }
        public bool IsActive { get; set; } = true;
        public string CardNumber { get; set; }
        public string FullName { get; set; }
        public DateTime ExpirationDate { get; set; }
        public int Cvv { get; set; }
        public int MaxTried { get; set; } = 0;
        public bool IsLocked { get; set; } = false;
        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public int UserId { get; set; }
        public int AccountId { get; set; }
        public CardDto ToDto(string pin)
        {
            return new CardDto
            {
                CardNumber = CardNumber,
                FullName = FullName,
                ExpirationDate = ExpirationDate,
                Pin = pin
            };

        }
    }
}
