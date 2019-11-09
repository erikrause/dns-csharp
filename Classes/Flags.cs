using System;
using System.Collections.Generic;
using System.Text;

namespace dnc_csharp.Classes
{
    public class Flags : Request
    {
        public Flags(byte[] data) : base(data)
        {
        }

        public int QR
        {
            get
            {
                byte b = DataRange(0, 1)[0];
                byte mask = 0x80;
                //var prob = 0b_1000_0000 & 0b_1000_0000;
                return b & mask;
            }
        }
        public int OPCODE;
        public int AA;
        public int TC;
        public int RD;
        public int RA;
        public int Z;
        public int RCODE;
    }
}
