using System.IO;
using System.Security.Cryptography;
using System.Xml.Linq;

namespace main
{
    public class MagicKeyMaker
    {
        public static void GenerateRSAKeyPair(out string publicKey, out string privateKey)
        {
            RSACryptoServiceProvider rsa = new RSACryptoServiceProvider(4096);
            publicKey = rsa.ToXmlString(false);
            privateKey = rsa.ToXmlString(true);
        }

        public static void GenerateRSAKeyPair(out string publicKey, out string privateKey, string outPutPath = null)
        {
            RSACryptoServiceProvider rsa = new RSACryptoServiceProvider(4096);
            publicKey = rsa.ToXmlString(false);
            privateKey = rsa.ToXmlString(true);

            if (outPutPath != null)
            {
                if (!Directory.Exists(outPutPath))
                {
                    Directory.CreateDirectory(outPutPath);
                }

                XDocument publicDoc = XDocument.Parse(publicKey);
                publicDoc.Save($"{outPutPath}/id_rsa.pub");
                XDocument privateDoc = XDocument.Parse(privateKey);
                privateDoc.Save($"{outPutPath}/id_rsa");
            }
        }
    }
}
