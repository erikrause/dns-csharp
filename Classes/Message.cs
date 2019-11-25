using System;
using System.Collections.Generic;
using System.Text;

namespace dns_csharp.Classes
{
    public class Message : Datagram
    {
        public Message(Query query) : this(new Query[]{ query })
        {
        }
        public Message(Query[] queryes)
        {
            List<byte> data = new List<byte>();

            Header = new Header();
            Question = InitializeQuestion(queryes);

            data.AddRange(Header.Data);
            data.AddRange(Question.Data);
            Data = data.ToArray();

            InitializeHandlers();
        }
        public Message(byte[] data) : base(data)
        {
            Header = new Header(DataRange(0, 12));

            int questionStartByte = 12;
            int questionSize = GetQuestionSize(questionStartByte, Header.QDCOUNT);
            Question = new Data<Query>(DataRange(questionStartByte, questionSize), Header.QDCOUNT);

            int answerStartByte = 12 + questionSize;
            int answerSize = GetAnswerSize(answerStartByte, Header.ANCOUNT);
            Answer = new Data<ResourceRecord>(DataRange(answerStartByte, answerSize), Header.ANCOUNT);

            InitializeHandlers();
        }
        protected void InitializeHandlers()
        {
            Header.PropertyChanged += OnDataChanged;
            Question.PropertyChanged += OnDataChanged;
            Answer.PropertyChanged += OnDataChanged;
        }
        protected void OnDataChanged(object sender, EventArgs e)
        {
            int startByte = 0;
            
            if (sender is Header)
            {
                startByte = 0;
            }
            else if (sender is Data<Query>)
            {
                startByte = 12;
            }
            else if (sender is Data<ResourceRecord>)
            {
                startByte = 12 + Question.Data.Length;
            }

            SetData(startByte, ((Datagram)sender).Data);
        }
        protected Data<Query> InitializeQuestion(Query[] queryes)
        {
            var question = new Data<Query>(queryes);

            Header.QDCOUNT = (ushort)question.Records.Count;

            return question;
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
                startByte += rLength + 1;
            }
            return dLength;
        }

        public Header Header = new Header();
        public Data<Query> Question = new Data<Query>();
        public Data<ResourceRecord> Answer = new Data<ResourceRecord>();
    }
}
