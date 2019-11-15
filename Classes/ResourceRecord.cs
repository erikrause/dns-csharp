using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace dnc_csharp.Classes
{
    public class ResourceRecord : Datagram
    {
        public ResourceRecord(byte[] data) : base(data)
        {

        }
        protected int NameEnd;
        public string NAME
        {
            get
            {
                return ToString(Data.Take(NameEnd - 1).ToArray());
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

        public int TTL
        {
            get
            {
                return GetDataInt(NameEnd + 4, 4);
            }
        }

        public int RDLENGTH
        {
            get
            {
                return GetDataInt(NameEnd + 8, 2);
            }
        }

        public string RDATA
        {
            get
            {
                return GetDataString(NameEnd + 10, RDLENGTH);
            }
        }
    }
}
