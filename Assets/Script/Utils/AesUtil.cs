using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace Script.Utils
{
    public class AesUtil
    {
        public string Encrypt(string original, string key) //key - 32 символа
        {
            byte[] iv = Encoding.ASCII.GetBytes("My?favorite/game");
            byte[] keyBytes = Encoding.ASCII.GetBytes(key);
            byte[] encrypted = null;
            
            encrypted = EncryptStringToBytes_Aes(original, keyBytes, iv);
            string encryptedString = Convert.ToBase64String(encrypted);
            return encryptedString;
        }

        public string Decrypt(string encryptedString, string key)
        {
            byte[] inputBuffer = Convert.FromBase64String(encryptedString);
            byte[] iv = Encoding.ASCII.GetBytes("My?favorite/game");
            byte[] keyBytes = Encoding.ASCII.GetBytes(key);
            string outData = null;
            
            outData = DecryptStringFromBytes_Aes(inputBuffer, keyBytes, iv);
            return outData;
        }
        
        private byte[] EncryptStringToBytes_Aes(string plainText, byte[] Key, byte[] IV)
        {
            if (plainText == null || plainText.Length <= 0)
                throw new ArgumentNullException("plainText");
            if (Key == null || Key.Length <= 0)
                throw new ArgumentNullException("Key");
            if (IV == null || IV.Length <= 0)
                throw new ArgumentNullException("IV");
            byte[] encrypted;
            
            using (Aes aesAlg = Aes.Create())
            {
                aesAlg.Key = Key;
                aesAlg.IV = IV;

                ICryptoTransform encryptor = aesAlg.CreateEncryptor(aesAlg.Key, aesAlg.IV);
                
                using (MemoryStream msEncrypt = new MemoryStream())
                {
                    using (CryptoStream csEncrypt = new CryptoStream(msEncrypt, encryptor, CryptoStreamMode.Write))
                    {
                        using (StreamWriter swEncrypt = new StreamWriter(csEncrypt))
                        {
                            swEncrypt.Write(plainText);
                        }
                        encrypted = msEncrypt.ToArray();
                    }
                }
            }
            
            return encrypted;
        }

        private string DecryptStringFromBytes_Aes(byte[] cipherText, byte[] Key, byte[] IV)
        {
            if (cipherText == null || cipherText.Length <= 0)
                throw new ArgumentNullException("cipherText");
            if (Key == null || Key.Length <= 0)
                throw new ArgumentNullException("Key");
            if (IV == null || IV.Length <= 0)
                throw new ArgumentNullException("IV");
            
            string plaintext = null;
            
            using (Aes aesAlg = Aes.Create())
            {
                aesAlg.Key = Key;
                aesAlg.IV = IV;
                
                ICryptoTransform decryptor = aesAlg.CreateDecryptor(aesAlg.Key, aesAlg.IV);
                
                using (MemoryStream msDecrypt = new MemoryStream(cipherText))
                {
                    using (CryptoStream csDecrypt = new CryptoStream(msDecrypt, decryptor, CryptoStreamMode.Read))
                    {
                        using (StreamReader srDecrypt = new StreamReader(csDecrypt))
                        {
                            plaintext = srDecrypt.ReadToEnd();
                        }
                    }
                }
            }

            return plaintext;
        }
    }
}