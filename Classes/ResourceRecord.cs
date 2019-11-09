using System;
using System.Collections.Generic;
using System.Text;

namespace dnc_csharp.Classes
{
    public class ResourceRecord : Record
    {
        public ResourceRecord(byte[] data) : base(data)
        {

        }

        public int TTL
        {
            get
            {
                return GetDataInt(NameEnd + 32, 8 * 4);
            }
        }

        public int RDLENGTH
        {
            get
            {
                return GetDataInt(NameEnd + 64, 8 * 2);
            }
        }

        public string RDATA
        {
            get
            {
                return GetDataString(NameEnd + 80, RDLENGTH);
            }
        }
    }
}
