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
            // DEBUG
            string debugMessage = "BB AA 01 00 00 01 00 00 00 00 00 00 07 65 78 61 6d 70 6c 65 03 63 6f 6d 00 00 01 00 01";
            byte[] debugMsg = Datagram.ToByteArray(debugMessage);
            ////////

            string dnsAddress = args[0];
            string domainName = args[1];
            string domainType = args[2];
            int dnsPort = 53;

            var query = new Query(domainName, domainType);
            var request = new Message(query);

            // Параметры заголовка:
            request.Header.ID = 101;
            request.Header.Flags.RD = 1;        // Включить рекурсию.

            // Установление соединения с DNS сервером:
            var socket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
            IPHostEntry ipHost = Dns.GetHostEntry(dnsAddress);
            IPAddress ipAddr = ipHost.AddressList[0]; 
            EndPoint ipEndPoint = new IPEndPoint(ipAddr, dnsPort);
            //////////////////////////////////////////
            
            socket.SendTo(request.Data, ipEndPoint);

            // Прием сообщения с сервера:
            byte[] bytes = new byte[4096];
            int bytesRec = socket.Receive(bytes);
            var data = bytes.Take(bytesRec).ToArray();

            var response = new Message(data);
            PrintAnswer(response);

            var probData = response.Answer.Records[0].RDATA;
            var prob = Encoding.UTF8.GetString(probData).TrimEnd('\0');     //debug
            var prob2 = BitConverter.ToString(probData);

            socket.Close();
        }

        static void PrintAnswer(Message response)
        {
            var answer = response.Answer;
            int i = 0;

            Console.Write("NAME");
            Console.SetCursorPosition(25, 0);
            Console.WriteLine("RDATA");

            foreach (ResourceRecord record in answer.Records)
            {
                //response.Question
                //Data<Query>.InitializeRecords(response.Question.Data, response.Question.Records.Count);
                int shift = Convert.ToInt32(record.NAME);
                string name = GetName(response.Question, shift - response.Header.Data.Length);
                Console.Write(name);
                Console.SetCursorPosition(25, i + 1);

                string data = ConvertData(record.RDATA, record.TYPE);
                Console.WriteLine(data);
                i++;
            }
        }
        /// <summary>
        /// Возвращает имя для в ResourseRecord по смещению shift.
        /// </summary>
        /// <param name="question"></param>
        /// <param name="shift"></param>
        /// <returns></returns>
        static string GetName(Data<Query> question, int shift)
        {
            string name = "";
            int currentShift = 0;

            foreach (Query rec in question.Records)
            {
                if (currentShift == shift)
                {
                    name = rec.NAME;
                    break;
                }
                currentShift += rec.Data.Length;
            }


            return name;
        }
        static string ConvertData(byte[] data, int type)
        {
            string output = "";

            switch (type)
            {
                case 1:     // type A.
                    foreach(byte b in data)
                    {
                        output += b.ToString() + '.';
                    }
                    output.Remove(output.Length - 1);   // delete last dot.

                    break;

                case 5:     // type CNAME.
                    // need to implement.
                    output = Datagram.ToString(data);
                    break;

                case 6:     // SOA.
                    // need to implement.
                    output = Datagram.ToString(data);
                    break;

                case 16:    // TXT.
                    output = Datagram.ToString(data.Skip(1).ToArray());
                    break;

                default:
                    output = Datagram.ToString(data);
                    break;
            }

            return output;
        }
    }
}
