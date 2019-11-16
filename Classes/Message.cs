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

            int questionStartByte = 12;
            int questionSize = GetQuestionSize(questionStartByte, Header.QDCOUNT);
            Question = new Data<Query>(DataRange(questionStartByte, questionSize), Header.QDCOUNT);

            int answerStartByte = 12 + questionSize;
            int answerSize = GetAnswerSize(answerStartByte, Header.ANCOUNT);
            Answer = new Data<ResourceRecord>(DataRange(answerStartByte, answerSize), Header.ANCOUNT);
        }

        protected int GetQuestionSize(int startByte, int numberOfRecords)
        {
            int size = -1;

            for (int rNumber = 0; rNumber < numberOfRecords; rNumber++)
            {
                size = IndexOf(Data, 0, startByte + size + 1) + 1 - startByte;
                size += 4;  // Add 2 type & class bytes;
            }

            return size;
        }

        protected int GetAnswerSize(int startByte, int numberOfRecords)
        {
            int dLength = 0;
            //Data
            for (int rNumber = 0; rNumber < numberOfRecords; rNumber++)
            {
                int rLeghthIndex = 10 + startByte;
                //int rLength = Data[rLeghthIndex] + rLeghthIndex;
                int rLength = GetDataInt(rLeghthIndex, 2) + 10 + 1;
                dLength += rLength + 1;  // Add 2 type & class bytes;
                startByte += dLength;
            }
            return dLength;
        }

        public Header Header;
        public Data<Query> Question;
        public Data<ResourceRecord> Answer;
    }
}
