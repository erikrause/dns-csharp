using System;
using System.Collections.Generic;
using System.Text;

namespace dns_csharp.Classes
{
    public class Flags : Datagram
    {
        public Flags(byte[] data) : base(data)
        {
            Field = GetDataInt();
        }

        protected int _field;
        protected int Field
        {
            get
            {
                return _field;
            }
            set
            {
                _field = value;
                SetData(0, ToBytes((ushort)_field));
            }
        }
               
        protected int GetValue(int mask)
        {
            int flag = Field & mask;
            // Убрает нули в флаге справа:
            if (flag > 0)
            {
                int digits = GetDigit(mask);
                flag >>= digits;
            }
            return flag;
        }
        protected void SetValue(int value, int mask)
        {
            int digits = GetDigit(mask);
            value <<= digits;

            Field &= ~mask;      // reset old value;
            Field |= value;      // set new value.
        }
        /// <summary>
        /// Возвращает количество нулей справа в маске.
        /// </summary>
        /// <returns></returns>
        protected int GetDigit(int mask)
        {
            int prob = 0b0001;

            int digits = 0;
            while ((prob & mask) != 1)
            {
                digits++;
                mask >>= 1;
            }
            return digits;
        }

        public int QR
        {
            get
            {
                int mask = 0b1000_0000_0000_0000;
                return GetValue(mask);
            }
            set
            {
                int mask = 0b1000_0000_0000_0000;
                SetValue(value, mask);
            }
        }
        public int OPCODE
        {
            get
            {
                int mask = 0b0111_1000_0000_0000;
                return GetValue(mask);
            }
            set
            {
                int mask = 0b0111_1000_0000_0000;
                SetValue(value, mask);
            }
        }
        public int AA
        {
            get
            {
                int mask = 0b0000_0100_0000_0000;
                return GetValue(mask);
            }
            set
            {
                int mask = 0b0000_0100_0000_0000;
                SetValue(value, mask);
            }
        }
        public int TC
        {
            get
            {
                int mask = 0b0000_0010_0000_0000;
                return GetValue(mask);
            }
            set
            {
                int mask = 0b0000_0010_0000_0000;
                SetValue(value, mask);
            }
        }
        public int RD
        {
            get
            {
                int mask = 0b0000_0001_0000_0000;
                return GetValue(mask);
            }
            set
            {
                int mask = 0b0000_0001_0000_0000;
                SetValue(value, mask);
            }
        }
        public int RA
        {
            get
            {
                int mask = 0b0000_0000_1000_0000;
                return GetValue(mask);
            }
            set
            {
                int mask = 0b0000_0000_1000_0000;
                SetValue(value, mask);
            }
        }
        public int Z
        {
            get
            {
                int mask = 0b0000_0000_0111_0000;
                return GetValue(mask);
            }
            set
            {
                int mask = 0b0000_0000_0111_0000;
                SetValue(value, mask);
            }
        }
        public int RCODE
        {
            get
            {
                int mask = 0b0000_0000_0000_1111;
                return GetValue(mask);
            }
            set
            {
                int mask = 0b0000_0000_0000_1111;
                SetValue(value, mask);
            }
        }
    }
}
