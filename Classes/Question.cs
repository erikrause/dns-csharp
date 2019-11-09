using System;
using System.Collections.Generic;
using System.Text;

namespace dnc_csharp.Classes
{
    public class Question : Request
    {
        public Question(byte[] data, int numberOfRecords)
        {
            Data = data;
            Records = new List<Record>(numberOfRecords);
            InitializeRecords(numberOfRecords);
        }

        private void InitializeRecords(int numberOfRecords)
        {
            int startByte = 0;
            int endByte = startByte;
            byte byteData;

            for (int rn = 0; rn < numberOfRecords; rn++)
            {
                do
                {
                    byteData = Data[endByte];
                    endByte++;
                } while (byteData != 0);
                endByte = endByte + 4;  // Add 2 type & class bytes;
                endByte = endByte - 1;  // End of cycle. 

                int numberOfBytes = endByte - startByte;
                Records[rn].NAME = GetDataString(startByte * 8, numberOfBytes * 8);
                Records[rn].TYPE = GetDataInt((startByte + 2) * 8);
            }
              
        }
        private List<Record> Records;
    }
}
