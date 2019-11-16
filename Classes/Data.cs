using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace dnc_csharp.Classes
{
    public class Data<T> : Datagram where T : Record
    {
        public Data(byte[] data, int numberOfRecords) : base(data)
        {
            Records = InitializeRecords(data, numberOfRecords);

        }
        protected List<T> InitializeRecords(byte[] data, int numberOfRecords)
        {
            List<T> records = new List<T>(numberOfRecords);
            int startIndex = 0;
            int endIndex = 0;

            if (typeof(T) == typeof(Query))
            {
                for (int rn = 0; rn < numberOfRecords; rn++)
                {
                    endIndex = IndexOf(data, 0, startIndex);
                    endIndex = endIndex + 4;  // Add 2 type & class bytes;
                    endIndex = endIndex + 1;

                    //var rec = new T(data.Skip(startIndex).Take(endIndex).ToArray());
                    var arg = data.Skip(startIndex).Take(endIndex).ToArray();
                    T rec = (T)Activator.CreateInstance(typeof(T), arg);
                    records.Insert(rn, rec);

                    startIndex = endIndex;
                }
            }
            if (typeof(T) == typeof(ResourceRecord))
            {
                for (int rn = 0; rn < numberOfRecords; rn++)
                {
                    int rLengthIndex = 10 + startIndex;
                    int rLength = GetDataInt(rLengthIndex) + 10 + 1;
                    endIndex = rLength + 1;

                    var arg = data.Skip(startIndex).Take(endIndex).ToArray();
                    T rec = (T)Activator.CreateInstance(typeof(T), arg);
                    records.Insert(rn, rec);

                    startIndex = endIndex;
                }
            }
            

            return records;
        }

        public List<T> Records;
    }
}
