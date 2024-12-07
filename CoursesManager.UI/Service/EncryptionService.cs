using System;
using System.Security.Cryptography;


namespace CoursesManager.UI.Service
{
    public class EncryptionService
    {
        private string key;

        public EncryptionService(string key)
        {
            if (string.IsNullOrWhiteSpace(key))
            {
                throw new ArgumentException("de sleutel mag niet leeg zijn", nameof(key));
            }

            this.key = key;
        }

        public string Encrypt(string plainText)
        {
            if (string.IsNullOrWhiteSpace(plainText))
            {
                throw new ArgumentException("de tekst om te versleutelen mag niet leeg zijn", nameof(plainText));
            }

            using (var aes = Aes.Create())
            {
                aes.Key = Convert.FromBase64String(key);
                aes.GenerateIV();

                using (var encryptor = aes.CreateEncryptor(aes.Key, aes.IV))
                {
                    var plainBytes = System.Text.Encoding.UTF8.GetBytes(plainText);
                    var encryptedBytes = encryptor.TransformFinalBlock(plainBytes, 0, plainBytes.Length);

                    return Convert.ToBase64String(aes.IV) + ":" + Convert.ToBase64String(encryptedBytes);
                }
            }
        }

        public string Decrypt(string encryptedText)
        {
            if (string.IsNullOrWhiteSpace(encryptedText))
            {
                Console.WriteLine("Decryptie mislukt: lege of ontbrekende tekst.");
                throw new ArgumentException("De tekst om te ontsleutelen mag niet leeg zijn.", nameof(encryptedText));
            }

            return encryptedText;
        }
    }
}
