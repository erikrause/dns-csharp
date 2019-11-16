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
                var prob = ToHex(DataRange(NameEnd + 6, 2));
                return GetDataInt(NameEnd + 6, 2);
            }
        }

        public int RDLENGTH
        {
            get
            {
                return GetDataInt(NameEnd + 8, 2);
            }
        }

        public byte[] RDATA
        {
            get
            {
                //return GetDataString(NameEnd + 10, RDLENGTH);
                return DataRange(NameEnd + 10, RDLENGTH);
            }
        }
    }
}
