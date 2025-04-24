using System.Net;
using System.Security.Cryptography;
using System.Text;

namespace Product_Management_System.Helper
{
    public static class UrlEncryptor
    {
        private static readonly string EncryptionKey = "qwertyDake6plk8h";

        #region Encrypt
        public static string Encrypt(string text)
        {
            using (var aesAlg = Aes.Create())
            {
                aesAlg.Key = Encoding.UTF8.GetBytes(EncryptionKey);
                aesAlg.IV = new byte[16];

                var encryptor = aesAlg.CreateEncryptor(aesAlg.Key, aesAlg.IV);

                using (var msEncrypt = new MemoryStream())
                {
                    using (var csEncrypt = new CryptoStream(msEncrypt, encryptor, CryptoStreamMode.Write))
                    using (var swEncrypt = new StreamWriter(csEncrypt))
                    {
                        swEncrypt.Write(text);
                    }

                    var encryptedBytes = msEncrypt.ToArray();
                    return WebUtility.UrlEncode(Convert.ToBase64String(encryptedBytes)); 
                }
            }
        }
        #endregion

        #region Decrypt
        public static string Decrypt(string encryptedText)
        {
            using (var aesAlg = Aes.Create())
            {
                aesAlg.Key = Encoding.UTF8.GetBytes(EncryptionKey);
                aesAlg.IV = new byte[16];

                var decryptor = aesAlg.CreateDecryptor(aesAlg.Key, aesAlg.IV);

                using (var msDecrypt = new MemoryStream(Convert.FromBase64String(WebUtility.UrlDecode(encryptedText))))
                using (var csDecrypt = new CryptoStream(msDecrypt, decryptor, CryptoStreamMode.Read))
                using (var srDecrypt = new StreamReader(csDecrypt))
                {
                    return srDecrypt.ReadToEnd();
                }
            }
        }

        #endregion

    }

}
