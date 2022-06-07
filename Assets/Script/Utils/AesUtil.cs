using System;
using System.Security.Cryptography;
using System.Text;

namespace Script.Utils
{
    public static class AesUtil
    {
        private static readonly byte[] _ivBytes;
        private static readonly byte[] _keyBytes;

        static AesUtil()
        {
            _ivBytes = new byte[16];
            _keyBytes = new byte[16];
        }
        
        public static string Encrypt(string data)
        {
            GenerateIvBytes();
            GenerateKeyBytes();

            SymmetricAlgorithm algorithm = Aes.Create();
            ICryptoTransform transform = algorithm.CreateEncryptor(_keyBytes, _ivBytes);
            byte[] inputBuffer = Encoding.Unicode.GetBytes(data);
            byte[] outputBuffer = transform.TransformFinalBlock(inputBuffer, 0, inputBuffer.Length);

            string ivString = Encoding.Unicode.GetString(_ivBytes);
            string encryptedString = Convert.ToBase64String(outputBuffer);

            return ivString + encryptedString;
        }

        public static string Decrypt(string text)
        {
            GenerateIvBytes();
            GenerateKeyBytes();

            int endOfIvBytes = _ivBytes.Length / 2;

            string ivString = text.Substring(0, endOfIvBytes);
            byte[] extractedIvBytes = Encoding.Unicode.GetBytes(ivString);

            string encryptedString = text.Substring(endOfIvBytes);
            
            SymmetricAlgorithm algorithm = Aes.Create();
            ICryptoTransform transform = algorithm.CreateDecryptor(_keyBytes, extractedIvBytes);
            byte[] inputBuffer = Convert.FromBase64String(encryptedString);
            byte[] outputBuffer = transform.TransformFinalBlock(inputBuffer, 0, inputBuffer.Length);

            string decryptedString = Encoding.Unicode.GetString(outputBuffer);
            return decryptedString;
        }

        private static void GenerateIvBytes()
        {
            Random random = new Random();
            random.NextBytes(_ivBytes);
        }

        private static void GenerateKeyBytes()
        {
            int sum = 0;
            string name = "My favorite game";
            
            foreach (var symbol in name)
            {
                sum += symbol;
            }

            Random random = new Random(sum);
            random.NextBytes(_keyBytes);
        }
    }
}