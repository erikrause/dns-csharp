using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

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
