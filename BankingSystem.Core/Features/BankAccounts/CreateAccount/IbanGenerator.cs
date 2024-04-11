namespace BankingSystem.Core.Features.BankAccounts.CreateAccount;

public class IbanGenerator
{
    private static Random random = new Random();

    public static string GenerateIban()
    {

        string countryCode = "GE";
        string bankInitials = "CD";
        string randomBban = GenerateRandomNumeric(16);

        if (randomBban.Length != 16 || !randomBban.All(char.IsDigit))
        {
            throw new ArgumentException("IBAN must be 16 digits.");
        }

        string tempIban = randomBban + GetCountryCodeNumeric(countryCode) + GetBankInitialsNumeric(bankInitials) + "00";
        int checkDigits = CalculateCheckDigits(tempIban);
        return countryCode + checkDigits.ToString("D2") + bankInitials + randomBban;
    }

    private static string GetCountryCodeNumeric(string countryCode)
    {
        return (countryCode[0] - 'A' + 10).ToString() + (countryCode[1] - 'A' + 10).ToString();
    }

    private static string GetBankInitialsNumeric(string bankInitials)
    {
        if (bankInitials.Length != 2)
        {
            throw new ArgumentException("Bank initials must be two characters.");
        }

        return (bankInitials[0] - 'A' + 10).ToString() + (bankInitials[1] - 'A' + 10).ToString();
    }

    private static int CalculateCheckDigits(string iban)
    {
        int remainder = int.Parse(iban.Substring(0, 2)) % 97;
        for (int i = 2; i < iban.Length; i += 2)
        {
            string segment = remainder.ToString() + iban.Substring(i, Math.Min(2, iban.Length - i));
            remainder = int.Parse(segment) % 97;
        }
        return 98 - remainder;
    }

    public static string GenerateRandomNumeric(int length)
    {
        const string chars = "0123456789";
        return new string(Enumerable.Repeat(chars, length)
          .Select(s => s[random.Next(s.Length)]).ToArray());
    }
}