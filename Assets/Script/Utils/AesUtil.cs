using System;
using System.IO;
using System.Linq;
using System.Security.Cryptography;

namespace Script.Utils
{
    public class AesUtil
    {
        private readonly byte[] _iv = { 101, 99, 157, 204, 134, 63, 201, 169, 163, 158, 12, 119, 138, 156, 187, 167 };

        public byte[] Encrypt(byte[] original, byte[] key) //key lenght are 32 symbols
        {
            return EncryptBytesDataToBytesAes(original, key, _iv);
        }

        public byte[] Decrypt(byte[] encrypted, byte[] key)
        {
            return DecryptBytesDataFromBytesAes(encrypted, key, _iv);
        }
        
        private byte[] EncryptBytesDataToBytesAes(byte[] encryptData, byte[] key, byte[] iv)
        {
            if (encryptData == null || encryptData.Length <= 0)
                throw new ArgumentNullException(nameof(encryptData));
            if (key == null || key.Length <= 0)
                throw new ArgumentNullException(nameof(key));
            if (iv == null || iv.Length <= 0)
                throw new ArgumentNullException(nameof(iv));
            
            byte[] encrypted;
            
            using (Aes aesAlg = Aes.Create())
            {
                aesAlg.Key = key;
                aesAlg.IV = iv;

                ICryptoTransform encryptor = aesAlg.CreateEncryptor(aesAlg.Key, aesAlg.IV);
                
                using (MemoryStream msEncrypt = new MemoryStream())
                {
                    using (CryptoStream csEncrypt = new CryptoStream(msEncrypt, encryptor, CryptoStreamMode.Write))
                    {
                        csEncrypt.Write(encryptData, 0, encryptData.Length);
                        csEncrypt.FlushFinalBlock();

                        encrypted = msEncrypt.ToArray();
                    }
                }
            }

            return encrypted;
        }

        private byte[] DecryptBytesDataFromBytesAes(byte[] cipherBytes, byte[] key, byte[] iv)
        {
            if (cipherBytes == null || cipherBytes.Length <= 0)
                throw new ArgumentNullException(nameof(cipherBytes));
            if (key == null || key.Length <= 0)
                throw new ArgumentNullException(nameof(key));
            if (iv == null || iv.Length <= 0)
                throw new ArgumentNullException(nameof(iv));

            byte[] bytes;
            
            using (Aes aesAlg = Aes.Create())
            {
                aesAlg.Key = key;
                aesAlg.IV = iv;

                ICryptoTransform decryptor = aesAlg.CreateDecryptor(aesAlg.Key, aesAlg.IV);
                
                using (MemoryStream msDecrypt = new MemoryStream(cipherBytes))
                {
                    using (CryptoStream csDecrypt = new CryptoStream(msDecrypt, decryptor, CryptoStreamMode.Read))
                    {
                        var buffer = new byte[cipherBytes.Length];
                        var bytesRead = csDecrypt.Read(buffer, 0, cipherBytes.Length);

                        bytes = buffer.Take(bytesRead).ToArray();
                    }
                }
            }

            return bytes;
        }
    }
}