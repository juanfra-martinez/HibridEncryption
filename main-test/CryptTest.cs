using System;
using System.IO;
using main;
using Xunit;

namespace main_test
{
    public class CryptTest
    {
        Crypt crypt;

        public CryptTest()
        {
            crypt = new Crypt();
        }

        [Fact]
        public void ShouldCreatAesKeys()
        {
            Assert.NotNull(crypt.EncryptionKey);
            Assert.NotNull(crypt.EncryptionIV);
        }

        [Fact]
        public void ShouldCreateEncryptedAndDecryptedFile()
        {
            string filePath = "test/SessionID.xml";
            string fileEncryptedPath = "test/SessionID.encrypted";
            crypt.Encrypt(filePath, fileEncryptedPath);

            Assert.True(File.Exists(fileEncryptedPath));

            string fileDecryptedPath = "test/SessionID.decrypted";
            crypt.DecryptFile(fileDecryptedPath, fileEncryptedPath, crypt.EncryptionKey, crypt.EncryptionIV);

            Assert.True(File.Exists(fileDecryptedPath));

            Assert.True(crypt.EncryptedFiles.Count == 1);
        }

        [Fact]
        public void ShouldGetKeyID()
        {
            MagicKeyMaker.GenerateRSAKeyPair(out string xmlPublicKey, out _);

            string id = Crypt.GetKeyID(xmlPublicKey, 5);

            Assert.True(id.Length == 5);

            string filePath = "test/SessionID.xml";
            string encryptedFilePath = crypt.MakeEncryptedFilePath(filePath, id);

            Assert.True(Path.GetExtension(encryptedFilePath) == "."+id);
        }


    }
}
