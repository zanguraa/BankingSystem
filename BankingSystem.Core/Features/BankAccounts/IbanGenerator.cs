﻿public class IbanGenerator
{
    private static Random random = new Random();

    public static string GenerateIban(string countryCode, string bankInitials, string bban)
    {
        if (bban.Length != 16 || !bban.All(char.IsDigit))
        {
            throw new ArgumentException("BBAN must be 16 digits.");
        }

        string tempIban = bban + GetCountryCodeNumeric(countryCode) + GetBankInitialsNumeric(bankInitials) + "00"; // Adding "00" as placeholder for check digits
        int checkDigits = CalculateCheckDigits(tempIban);
        return countryCode + checkDigits.ToString("D2") + bankInitials + bban;
    }

    private static string GetCountryCodeNumeric(string countryCode)
    {
        return ((int)countryCode[0] - 'A' + 10).ToString() + ((int)countryCode[1] - 'A' + 10).ToString();
    }

    private static string GetBankInitialsNumeric(string bankInitials)
    {
        if (bankInitials.Length != 2)
        {
            throw new ArgumentException("Bank initials must be two characters.");
        }

        return ((int)bankInitials[0] - 'A' + 10).ToString() + ((int)bankInitials[1] - 'A' + 10).ToString();
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