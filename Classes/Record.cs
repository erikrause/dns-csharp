using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace dnc_csharp.Classes
{
    public class Record : Datagram
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
                return ToString(Data.TakeWhile(x => x != 0).ToArray());
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
