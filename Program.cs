using System;
using System.Text;
using System.Net;
using System.Net.Sockets;
using System.Linq;

using dnc_csharp.Classes;

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
            byte[] msg = Datagram.ToByteArray(message);

            // DEGUG:
            Message request = new Message(msg);
            var prob = request.Header.Flags.RD;
            ///////////////

            IPHostEntry ipHost = Dns.GetHostEntry(dnsAddress);
            IPAddress ipAddr = ipHost.AddressList[0]; 
            EndPoint ipEndPoint = new IPEndPoint(ipAddr, dnsPort);
            socket.SendTo(request.Data, ipEndPoint);

            byte[] bytes = new byte[4096];
            int bytesRec = socket.Receive(bytes);
            string response = Encoding.UTF8.GetString(bytes, 0, bytesRec);
            string response2 = Datagram.ToString(bytes, bytesRec);
            socket.Close();
        }
    }
}
