using System;
using System.Collections.Generic;
using System.Text;

namespace dnc_csharp.Classes
{
    public class Record : Request
    {
        public Record(byte[] data)
        {
            Data = data;

        }
        public string NAME;
        public int TYPE;
        public int CLASS;
    }
}
