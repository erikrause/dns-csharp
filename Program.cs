using System;
using System.Text;
using System.Net;
using System.Net.Sockets;
using System.Linq;

namespace dnc_csharp
{
    class Program
    {
        static void Main(string[] args)
        {
            Socket socket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
            string message = "BB AA 01 00 00 01 00 00 00 00 00 00 07 65 78 61 6d 70 6c 65 03 63 6f 6d 00 00 01 00 01";
            string dnsAddress = "8.8.8.8";
            int dnsPort = 53;
            byte[] msg = ToByteArray(message);

            IPHostEntry ipHost = Dns.GetHostEntry(dnsAddress);
            IPAddress ipAddr = ipHost.AddressList[0];       // Изменил адрес на IPv4
            EndPoint ipEndPoint = new IPEndPoint(ipAddr, dnsPort);
            socket.SendTo(msg, ipEndPoint);

            byte[] bytes = new byte[4096];
            int bytesRec = socket.Receive(bytes);
            string response = Encoding.UTF8.GetString(bytes, 0, bytesRec);
            string response2 = ToString(bytes, bytesRec);
            socket.Close();
        }

        public static byte[] ToByteArray(string hex)
        {
            hex = hex.Replace(" ", "").Replace("\n", "");
            return Enumerable.Range(0, hex.Length)
                             .Where(x => x % 2 == 0)
                             .Select(x => Convert.ToByte(hex.Substring(x, 2), 16))
                             .ToArray();
        }

        public static string ToString(byte[] bytes, int count = -1)
        {
            if (count == -1)
            {
                count = bytes.Length;
            }
            
            string result = BitConverter.ToString(bytes.Take(count).ToArray());

            return result.Replace("-", "");
        }
    }
}
