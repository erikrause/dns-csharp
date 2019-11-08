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
            //Console.WriteLine("Hello World!");
            //byte[] ba = Encoding.Default.GetBytes("Hello world!!!");
            //var hexString = BitConverter.ToString(ba);
            //hexString = hexString.Replace("-", "");
            Socket socket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
            string message = "BB AA 01 00 00 01 00 00 00 00 00 00 07 65 78 61 6d 70 6c 65 03 63 6f 6d 00 00 01 00 01";
            message = message.Replace(" ", "").Replace("\n", "");
            string address = "8.8.8.8";
            int port = 53;
            //server_address = (address, port);
            //socket.Connect(address, port);
            byte[] msg = StringToByteArray(message);

            IPHostEntry ipHost = Dns.GetHostEntry("8.8.8.8");
            IPAddress ipAddr = ipHost.AddressList[0];       // Изменил адрес на IPv4
            // В новых версиях Framework следует использовать
            // IPAddress ipAddr = ipHost.AddressList[1];
            EndPoint ipEndPoint = new IPEndPoint(ipAddr, 53);
            socket.SendTo(msg, ipEndPoint);


            //socket.Send(msg);
            //byte response = socket.Receive(4096);
            byte[] bytes = new byte[4096];
            //int bytesRec = socket.Receive(bytes);
            int bytesRec = socket.ReceiveFrom(bytes, ref ipEndPoint);
            string rescponse = Encoding.UTF8.GetString(bytes, 0, bytesRec);
            //string rescponse = BitConverter.ToString(bytes);
            socket.Close();
        }

        public static byte[] StringToByteArray(string hex)
        {
            return Enumerable.Range(0, hex.Length)
                             .Where(x => x % 2 == 0)
                             .Select(x => Convert.ToByte(hex.Substring(x, 2), 16))
                             .ToArray();
        }
    }
}
