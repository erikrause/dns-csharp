using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace dnc_csharp.Classes
{
    public abstract class Data<T> : Datagram where T : Record, new()
    {
        public Data(byte[] data, int numberOfRecords) : base(data)
        {
            Records = new List<T>(numberOfRecords);
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

                //var rec = new T(data.Skip(startByte).Take(endByte).ToArray());
                var arg = data.Skip(startByte).Take(endByte).ToArray();
                T rec = (T)Activator.CreateInstance(typeof(T), arg);
                _records.Insert(rn, rec);

                startByte = endByte;    // need to debug
            }
        }
        protected List<T> _records;
        public abstract List<T> Records
        {
            get;
            set;
        }
    }
    public class MyClass<T> where T : Record, new()
    {
        protected T GetObject(byte[] data)
        {
            return (T)Activator.CreateInstance(typeof(T), 0);
        }
    }
}
