using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace dnc_csharp.Classes
{
    public class Record : Request
    {
        public Record(byte[] data)
        {
            Data = data;
            NameEnd = IndexOf(Data, 0, 0) + 1;
        }
        private int NameEnd;
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
                return GetDataInt(NameEnd + 16);
            }
        }
    }
}
