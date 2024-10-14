using System.Security.Cryptography;

namespace prjFileUpload
{
    public static class EncryptionHelper
    {
        private static readonly byte[] Key = Convert.FromBase64String("Matv8WfUmrNGGiU6A4163cnHFxoMg/HFItENwNuJL8M=");
        private static readonly byte[] IV = Convert.FromBase64String("2+v/jS5L03v08I7dmzYPDA==");
        private static byte[] PerformCryptography(byte[] data, ICryptoTransform cryptoTransform)
        {
            using (var memoryStream = new MemoryStream())
            {
                using (var cryptoStream = new CryptoStream(memoryStream, cryptoTransform, CryptoStreamMode.Write))
                {
                    cryptoStream.Write(data, 0, data.Length);
                    cryptoStream.FlushFinalBlock();
                    return memoryStream.ToArray();
                }
            }
        }
        public static byte[] Encrypt(byte[] data)
        {
            using (Aes aes = Aes.Create())
            {
                aes.Key = Key;
                aes.IV = IV;
                using (var encryptor = aes.CreateEncryptor(aes.Key, aes.IV))
                {
                    return PerformCryptography(data, encryptor);
                }
            }
        }
        public static byte[] Decrypt(byte[] data)
        {
            using (Aes aes = Aes.Create())
            {
                aes.Key = Key;
                aes.IV = IV;
                using (var decryptor = aes.CreateDecryptor(aes.Key, aes.IV))
                {
                    return PerformCryptography(data, decryptor);
                }
            }
        }
    }
}
