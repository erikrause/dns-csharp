using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace dnc_csharp.Classes
{
    public class Questions : Datagram
    {
        public Questions(byte[] data, int numberOfRecords) : base(data)
        {
            Records = new List<ResourceRecord>(numberOfRecords);
            InitializeRecords(data, numberOfRecords);
        }

        private void InitializeRecords(byte[] data, int numberOfRecords)
        {
            int startByte = 0;
            int endByte;

            for (int rn = 0; rn < numberOfRecords; rn++)
            {
                endByte = IndexOf(data, 0, startByte);
                endByte = endByte + 4;  // Add 2 type & class bytes;
                endByte = endByte + 1;

                var rec = new ResourceRecord(data.Skip(startByte).Take(endByte).ToArray());
                Records.Insert(rn, rec);

                startByte = endByte;    // need to debug
            }
        }

        public List<ResourceRecord> Records; 
    }
}
