using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankingSystem.Core.Data.Entities
{
    public class CardEntity
    {
        public int Id { get; set; }
        public string? CardNumber { get; set; }
        public string? CardHolderName { get; set; }
        public DateTime CardExpirationDate { get; set; }
        public string? CvvCode { get; set; }
        public string? PinCode { get; set; }
        public int AccountId { get; set; }
        public virtual AccountEntity Account { get; set; }

    }
}
