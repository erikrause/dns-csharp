using System;
using System.Collections.Generic;
using System.Text;

namespace dnc_csharp.Classes
{
    public class Message : Datagram
    {
        public Message(byte[] data) : base(data)
        {
            Header = new Header(DataRange(0, 12));
            int questionSize = GetRecordSize(Header.QDCOUNT);
            Questions = new Data<Query>(DataRange(12, questionSize), Header.QDCOUNT);
            int answerSize = GetRecordSize(Header.ANCOUNT);
            Answers = new Data<ResourceRecord>(DataRange(12 + questionSize, answerSize), Header.ANCOUNT);
        }

        private int GetRecordSize(int numberOfRecords)
        {
            int startByte = 12;
            int i = -1;

            for (int rn = 0; rn < numberOfRecords; rn++)
            {
                i = IndexOf(Data, 0, startByte) + 1;
                i = i + 4;  // Add 2 type & class bytes;
            }

            int size = i - startByte;
            return size;
        }

        public Header Header;
        public Data<Query> Questions;
        public Data<ResourceRecord> Answers;
    }
}
