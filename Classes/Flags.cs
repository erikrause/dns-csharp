using System;
using System.Collections.Generic;
using System.Text;

namespace dnc_csharp.Classes
{
    public class Flags : Request
    {
        public Flags(byte[] data) : base(data)
        {
            Field = GetDataInt();
        }

        protected int Field;
        public int QR
        {
            get
            {
                int mask = 0b1000_0000_0000_0000;
                return Field & mask;
            }
        }
        public int OPCODE
        {
            get
            {
                int mask = 0b0111_1000_0000_0000;
                return Field & mask;
            }
        }
        public int AA
        {
            get
            {
                int mask = 0b0000_0100_0000_0000;
                return Field & mask;
            }
        }
        public int TC
        {
            get
            {
                int mask = 0b0000_0010_0000_0000;
                return Field & mask;
            }
        }
        public int RD
        {
            get
            {
                int mask = 0b0000_0001_0000_0000;
                return Field & mask;
            }
        }
        public int RA
        {
            get
            {
                int mask = 0b0000_0000_1000_0000;
                return Field & mask;
            }
        }
        public int Z
        {
            get
            {
                int mask = 0b0000_0000_0111_0000;
                return Field & mask;
            }
        }
        public int RCODE
        {
            get
            {
                int mask = 0b0000_0000_0000_1111;
                return Field & mask;
            }
        }
    }
}
