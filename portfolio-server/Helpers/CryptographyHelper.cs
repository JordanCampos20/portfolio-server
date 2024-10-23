using System.Text;
using System.Security.Cryptography;

namespace portfolio_server.Helpers
{
    public static class CryptographyHelper
    {
        public static string EncryptString(string plainText, string key)
        {
            byte[] iv = new byte[16];
            byte[] array;

            using (Aes aes = Aes.Create())
            {
                aes.Key = Encoding.UTF8.GetBytes(key);
                aes.IV = iv;

                ICryptoTransform encryptor = aes.CreateEncryptor(aes.Key, aes.IV);

                using (MemoryStream memoryStream = new MemoryStream())
                {
                using (CryptoStream cryptoStream = new CryptoStream(memoryStream, encryptor, CryptoStreamMode.Write))
                {
                    using (StreamWriter streamWriter = new StreamWriter(cryptoStream))
                    {
                    streamWriter.Write(plainText);
                    }

                    array = memoryStream.ToArray();
                }
                }
            }

            return SanitizeBase64(Convert.ToBase64String(array));
        }

        public static string DecryptString(string cipherText, string key)
        {
            byte[] iv = new byte[16];
            byte[] buffer = Convert.FromBase64String(SanitizeBase64(cipherText, true));

            using (Aes aes = Aes.Create())
            {
                aes.Key = Encoding.UTF8.GetBytes(key);
                aes.IV = iv;

                ICryptoTransform decryptor = aes.CreateDecryptor(aes.Key, aes.IV);

                using (MemoryStream memoryStream = new MemoryStream(buffer))
                {
                using (CryptoStream cryptoStream = new CryptoStream(memoryStream, decryptor, CryptoStreamMode.Read))
                {
                    using (StreamReader streamReader = new StreamReader(cryptoStream))
                    {
                    return streamReader.ReadToEnd();
                    }
                }
                }
            }
        }

        public static string SanitizeBase64(string base64, bool decode = false)
        {
            if (decode)
                return base64.Replace("_", "/");
            else
                return base64.Replace("/", "_");
        }
    }
}