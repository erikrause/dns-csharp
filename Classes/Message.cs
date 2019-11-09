using System;
using System.Collections.Generic;
using System.Text;

namespace dnc_csharp.Classes
{
    public class Message : Request
    {
        public Message(byte[] data)
        {
            Data = data;
            Header = new Header(DataRange(0, 12 * 8));
            int questionSize = GetQuestionSize(Header.QDCOUNT);
            Question = new Question(DataRange(12 * 8, questionSize), Header.QDCOUNT);
            //Answer = new ResourseRequest(data);
        }

        private int GetQuestionSize(int numberOfRecords)
        {
            int startByte = 12;
            int i = startByte;
            byte byteData;

            for (int rn = 0; rn < numberOfRecords; rn++)
            {
                do
                {
                    byteData = Data[i];
                    i++;
                } while (byteData != 0);
                i = i + 4;  // Add 2 type & class bytes;
            }
            i = i - 1;  // End of cycle.     

            return i;
        }

        public Header Header;
        public Question Question;
        //public ResourseRequest Answer;
    }
}
