using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace dnc_csharp.Classes
{
    public class Record : Datagram, IRecord
    {
        public Record(byte[] data) : base(data)
        {
            NameEnd = IndexOf(Data, 0, 0) + 1;
        }
        protected int NameEnd;
        public string NAME
        {
            get
            {
                string name = "";
                int start = 0;
                int count = 0;

                while (Data[start] != 0)
                {
                    count = Data[start];
                    string domain = ToString(Data.Skip(start + 1).Take(count).ToArray());
                    name += domain + '.';
                    start = start + count + 1;
                }
                name = name.Remove(name.Length - 1);   // Delete last '.' after loop.

                return name;
            }
        }
        public int TYPE
        {
            get
            {
                return GetDataInt(NameEnd);
            }
        }
        public int CLASS
        {
            get
            {
                return GetDataInt(NameEnd + 2);
            }
        }
    }
}
