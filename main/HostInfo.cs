using System;
using System.Collections.Generic;
using System.IO;
using System.Net;

namespace main
{
    public class HostInfo
    {
        public List<string> LocalAddresses { get; private set; }
        public string PublicAddress { get; private set; }
        public string ComputerName { get; private set; }


        public HostInfo()
        {
            LocalAddresses = new List<string>();
            PublicAddress = GetIPAddress();
            ComputerName = Dns.GetHostName();
            try
            {
                var addresses = Dns.GetHostAddresses(ComputerName);
                foreach (IPAddress address in addresses)
                {
                    LocalAddresses.Add(address.ToString());
                }
            } catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        private string GetIPAddress()
        {
            string address = "";
            WebRequest request = WebRequest.Create("http://checkip.dyndns.org/");
            using (WebResponse response = request.GetResponse())
            using (StreamReader stream = new StreamReader(response.GetResponseStream()))
            {
                address = stream.ReadToEnd();
            }

            int first = address.IndexOf("Address: ") + 9;
            int last = address.LastIndexOf("</body>");
            address = address.Substring(first, last - first);

            return address;
        }
    }
}
