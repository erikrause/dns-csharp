using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace dnc_csharp.Classes
{
    public class Question : Request
    {
        public Question(byte[] data, int numberOfRecords) : base(data)
        {
            Records = new List<Record>(numberOfRecords +1);
            InitializeRecords(numberOfRecords);
        }

        private void InitializeRecords(int numberOfRecords)
        {
            int startByte = 0;
            int endByte;

            for (int rn = 0; rn < numberOfRecords; rn++)
            {
                endByte = IndexOf(Data, 0, startByte);
                endByte = endByte + 4;  // Add 2 type & class bytes;
                endByte = endByte + 1;

                var rec = new Record(Data.Skip(startByte).Take(endByte).ToArray());
                Records[rn] = rec;

                startByte = endByte;    // debug
            }              
        }
        private List<Record> Records;
    }
}
