using System;
using System.Collections.Generic;
using System.Text;

namespace dnc_csharp.Classes
{
    public class Flags : Request
    {
        public Flags(byte[] data)
        {
            Data = data;
        }

        public byte[] QR
        {
            get
            {
                return DataRange(0, 1);
            }
        }
        public byte[] OPCODE;
        public byte[] AA;
        public byte[] TC;
        public byte[] RD;
        public byte[] RA;
        public byte[] Z;
        public byte[] RCODE;
    }
}
