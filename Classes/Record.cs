using System;
using System.Collections.Generic;
using System.Text;

namespace dnc_csharp.Classes
{
    public abstract class Record : Datagram
    {
        public Record() : this(new byte[] { })
        {

        }
        public Record(byte[] data) : base(data)
        {

        }
        protected virtual int NameEnd
        {
            get
            {
                return 2;
            }
        }
        public virtual string NAME
        {
            get
            {
                return GetDataString(0, NameEnd);
            }
            set
            {

            }
            //{
                //byte[] data = ToBytes(value);
                //SetData(NameEnd + 2, data);
            //}
        }

        public ushort TYPE
        {
            get
            {
                return GetDataInt(NameEnd);
            }
            set
            {
                byte[] data = ToBytes(value);
                SetData(NameEnd, data);
            }
        }

        public ushort CLASS
        {
            get
            {
                return GetDataInt(NameEnd + 2);
            }
            set
            {
                byte[] data = ToBytes(value);
                SetData(NameEnd + 2, data);
            }
        }
        public static Dictionary<string, int> Types = new Dictionary<string, int>()
        {
            { "A", 1 },
            { "AAAA", 28 },
            { "CNAME", 5 },
            { "MX", 15 },
            { "NS", 2 },
            { "PTR", 12 },
            { "SOA", 6},
            { "TXT", 16 }
        };
    }
}