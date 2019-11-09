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
            Question = new Question(DataRange(12 * 8, questionSize * 8), Header.QDCOUNT);
            //Answer = new ResourseRequest(data);
        }

        private int GetQuestionSize(int numberOfRecords)
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
        public Question Question;
        //public ResourseRequest Answer;
    }
}
