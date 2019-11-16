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
            var prob1 = request.Header.Flags.RD;
            var prob2 = request.Question.Records[0].NAME;
            
            ///////////////

            IPHostEntry ipHost = Dns.GetHostEntry(dnsAddress);
            IPAddress ipAddr = ipHost.AddressList[0]; 
            EndPoint ipEndPoint = new IPEndPoint(ipAddr, dnsPort);
            socket.SendTo(request.Data, ipEndPoint);

            byte[] bytes = new byte[4096];
            int bytesRec = socket.Receive(bytes);
            string response1 = Encoding.UTF8.GetString(bytes, 0, bytesRec);     //debug
            string response2 = Datagram.ToString(bytes, bytesRec);      //debug

            var data = bytes.Take(bytesRec).ToArray();
            Message response = new Message(data);
            var prob3 = request.Header.Flags.OPCODE;    //debug

            socket.Close();

            string[] domains = { "domain" };
            string[] types = { "type" };
            Query query = new Query("example.com", 1);

            Message prob = new Message(query);
        }
    }
}
