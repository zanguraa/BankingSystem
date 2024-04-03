using Microsoft.Extensions.Configuration;
using System.Collections;
using System.Security.Cryptography;
using System.Text;

namespace BankingSystem.Core.Shared.Services
{
    public interface ICryptoService
    {
        string Encrypt(string plainText);
        string Decrypt(string plainText);
    }
    public class CryptoService : ICryptoService
    {
        private readonly string _aesKey;
        private readonly string _ivKey;
        public CryptoService(IConfiguration configuration)
        {
            _aesKey = configuration.GetSection("CryptedAESSecretKey").Value
                ?? throw new Exception("encryption key is missing");
            _ivKey = configuration.GetSection("CryptedIvKey").Value
                ?? throw new Exception("ecnryption iv key is missing");


            var iv = GenerateRandomKey(128);
        }
        public string Encrypt(string plainText)
        {
            byte[] key = HexToByteArray(_aesKey);
            byte[] iv = HexToByteArray(_ivKey);

            using (Aes aesAlg = Aes.Create())
            {
                aesAlg.Key = key;
                aesAlg.IV = iv;

                ICryptoTransform encryptor = aesAlg.CreateEncryptor(aesAlg.Key, aesAlg.IV);

                using (MemoryStream msEncrypt = new MemoryStream())
                {
                    using (CryptoStream csEncrypt = new CryptoStream(msEncrypt, encryptor, CryptoStreamMode.Write))
                    {
                        using (StreamWriter swEncrypt = new StreamWriter(csEncrypt))
                        {
                            swEncrypt.Write(plainText);
                        }
                    }

                    return Convert.ToBase64String(msEncrypt.ToArray());
                }

            }

        }

        public string Decrypt(string encryptedText)
        {
            byte[] key = Encoding.UTF8.GetBytes(_aesKey);
            byte[] iv = Encoding.UTF8.GetBytes(_ivKey);

            using (Aes aesAlg = Aes.Create())
            {
                aesAlg.Key = key;
                aesAlg.IV = iv;

                ICryptoTransform decryptor = aesAlg.CreateDecryptor(aesAlg.Key, aesAlg.IV);

                using (MemoryStream msDecrypt = new MemoryStream(Convert.FromBase64String(encryptedText)))
                {
                    using (CryptoStream csDecrypt = new CryptoStream(msDecrypt, decryptor, CryptoStreamMode.Read))
                    {
                        using (StreamReader srDecrypt = new StreamReader(csDecrypt))
                        {
                            return srDecrypt.ReadToEnd();
                        }
                    }
                }
            }
        }
        private static string GenerateRandomKey(int keySize)
        {
            using (var randomNumberGenerator = new RNGCryptoServiceProvider())
            {
                var key = new byte[keySize / 8]; // Key size is in bits, so divide by 8
                randomNumberGenerator.GetBytes(key);
                return BitConverter.ToString(key).Replace("-", "");
            }
        }

        private static byte[] HexToByteArray(string hex)
        {
            int length = hex.Length;
            byte[] bytes = new byte[length / 2];
            for (int i = 0; i < length; i += 2)
            {
                bytes[i / 2] = Convert.ToByte(hex.Substring(i, 2), 16);
            }
            return bytes;
        }

    }
}
