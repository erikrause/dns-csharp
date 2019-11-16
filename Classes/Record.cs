using System;
using System.Collections.Generic;
using System.Text;

namespace dnc_csharp.Classes
{
    public abstract class Record : Datagram
    {
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

        public static void GetDataLength(byte[] data)
        {

        }
    }
}