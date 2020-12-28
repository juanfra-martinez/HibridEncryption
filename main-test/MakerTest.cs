using System.IO;
using main;
using Xunit;

namespace main_test
{
    public class MakerTest
    {
        public MakerTest()
        {
        }

        [Fact]
        public void ShouldCreateKeyPairFiles()
        {
            string path = "test/keys";
            MagicKeyMaker.GenerateRSAKeyPair(out string pubKey, out string privKey, path);
            Assert.True(pubKey.Length > 0);
            Assert.True(privKey.Length > 0);

            bool exist = File.Exists($"{path}/id_rsa");
            Assert.True(exist);
            exist = File.Exists($"{path}/id_rsa.pub");
            Assert.True(exist);

        }
    }
}
