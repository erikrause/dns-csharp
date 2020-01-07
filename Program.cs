using dns_csharp.Classes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace dns_csharp
{
    class Program
    {
        static void Main(string[] args)
        {
            // DEBUG
            //string debugMessage = "BB AA 01 00 00 01 00 00 00 00 00 00 07 65 78 61 6d 70 6c 65 03 63 6f 6d 00 00 01 00 01";
            //byte[] debugMsg = Datagram.ToByteArray(debugMessage);
            ////////

            #region Установка параметров сообщения

            string dnsAddress = args[0];
            string domainName = args[1];
            string domainType = args[2];
            int dnsPort = 53;

            var query = new Query(domainName, domainType);
            var request = new Message(query);

            // Параметры заголовка:
            request.Header.ID = 101;
            request.Header.Flags.RD = 1;    // Включить рекурсию.

            #endregion

            #region Установление соединения с DNS сервером и отправка сообщения

            var socket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
            IPHostEntry ipHost = Dns.GetHostEntry(dnsAddress);
            IPAddress ipAddr = ipHost.AddressList[0];
            EndPoint ipEndPoint = new IPEndPoint(ipAddr, dnsPort);
            socket.SendTo(request.Data, ipEndPoint);

            #endregion

            #region Прием ответа с сервера

            byte[] bytes = new byte[4096];
            int bytesRec = socket.Receive(bytes);
            var data = bytes.Take(bytesRec).ToArray();

            #endregion

            #region Output to console

            var response = new Message(data);
            PrintAnswer(response);

            var prob = response.Question.Records[0].NAME;
            #endregion

            socket.Close();

            Console.ReadKey();
        }

        static void PrintAnswer(Message response)
        {
            var answer = response.Answer;
            int i = 0;

            Console.Write("NAME");
            Console.SetCursorPosition(25, 0);
            Console.Write("TYPE");
            Console.SetCursorPosition(50, 0);
            Console.WriteLine("RDATA");

            foreach (ResourceRecord record in answer.Records)
            {
                //response.Question
                //Data<Query>.InitializeRecords(response.Question.Data, response.Question.Records.Count);
                int shift = Convert.ToInt32(record.NAME);
                string name = GetName(response.Question, shift - response.Header.Data.Length);
                Console.Write(name);
                Console.SetCursorPosition(25, i + 1);
                Console.Write(ConvertTypesToStr[record.TYPE]);


                Console.SetCursorPosition(50, i + 1);
                string data = ParseData(record, response);
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
        /// <summary>
        /// Преобразование RDATA ресурсной в строку.
        /// </summary>
        /// <param name="record"> Ресурсная запись ответа. </param>
        /// <param name="message"> Ответ. необходим для получения доменного имени по ссылке из ресурсной записи. </param>
        /// <returns></returns>
        static string ParseData(ResourceRecord record, Message message)
        {
            byte[] data = record.RDATA;
            int type = record.TYPE;
            string output = "";

            switch (type)
            {
                case 1:     // type A.
                    foreach (byte b in data)
                    {
                        output += b.ToString() + '.';
                    }
                    output = output.Remove(output.Length - 1);   // delete last dot.

                    break;

                case 5:     // type CNAME.
                    // need to implement.
                    output = Datagram.ToString(data);
                    break;
                case 15:    // MX.
                    var mailExchangerRecord = MXParser(data, message.Data);
                    output += $"Priority: {mailExchangerRecord.priority}, Domain: {mailExchangerRecord.domain}";
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

        public static Dictionary<int, string> ConvertTypesToStr = new Dictionary<int, string>()
        {
            { 1, "A" },
            { 28, "AAAA"  },
            { 5, "CNAME" },
            { 15, "MX" },
            { 33, "SRV" },
            { 2, "NS" },
            { 12, "PTR" },
            { 6, "SOA"},
            { 16, "TXT" }
        };
        /// <summary>
        /// Возвращает две строки: приоритет и имя почтового сервера.
        /// </summary>
        /// <param name="RDATA"> Resource record's data. </param>
        /// <param name="message"> Response datagram. </param>
        /// <returns> Кортеж приоритета и домена почтового сервера. </returns>
        public static (int priority, string domain) MXParser(byte[] RDATA, byte[] message)
        {
            int priority = RDATA[1];

            string domain = NameParser(RDATA.Skip(2).ToArray(), message);

            return (priority, domain);
        }
        // TODO: перенести этот метод в датаграмму:
        public static string NameParser(byte[] data, byte[] message)
        {
            int pointer = 0;
            int count;
            string fullDomain = "";

            while (pointer < data.Length && data[pointer] != 0)
            {

                string domain = "";
                count = data[pointer];
                if (data[pointer] < 192)   // Если это не смещение байт, то получить домен по количеству символов (байт).
                {
                    domain = Encoding.UTF8.GetString(data.Skip(pointer + 1).Take(count).ToArray());
                    pointer += count + 1;
                }
                else        // Если это смещение байт, то получить домен по смещению байт в сообщении.
                {
                    byte[] nameShiftBytes = data.Skip(pointer).Take(2).ToArray();
                    Array.Reverse(nameShiftBytes);
                    int nameShift = (ushort)BitConverter.ToInt16(nameShiftBytes, 0);
                    nameShift = nameShift - (0b11 << 14);

                    domain = NameParser(message.Skip(nameShift).ToArray(), message);
                    pointer += 2;
                }
                fullDomain += domain + ".";
            }
            fullDomain = fullDomain.Remove(fullDomain.Length - 1);       // Delete last '.' after the loop.

            return fullDomain;
        }
    }
}
