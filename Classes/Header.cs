using System;
using System.Collections.Generic;
using System.Text;

namespace dnc_csharp.Classes
{
    public class Header : Datagram
    {
        public Header()
        {
            byte[] data = new byte[12];
            Data = data;
            ID = 1;

        }
        public Header(byte[] data) : base(data)
        {
            Flags = new Flags(DataRange(2, 2));
        }
        public ushort ID
        {
            get
            {
                return GetDataInt(0);
            }
            set
            {
                byte[] data = GetBytes(value);
                SetData(0, data);
            }
        }
        /// <summary>
        /// Number of questions.
        /// </summary>
        public ushort QDCOUNT
        {
            get
            {
                return GetDataInt(4);
            }
            set
            {
                byte[] data = GetBytes(value);
                SetData(4, data);
            }
        }
        /// <summary>
        /// Number of answers.
        /// </summary>
        public ushort ANCOUNT
        {
            get
            {
                return GetDataInt(6);
            }
            set
            {
                byte[] data = GetBytes(value);
                SetData(6, data);
            }
        }
        /// <summary>
        /// Number of authority.
        /// </summary>
        public ushort NSCOUNT
        {
            get
            {
                return GetDataInt(8);
            }
            set
            {
                byte[] data = GetBytes(value);
                SetData(8, data);
            }
        }
        /// <summary>
        /// Number of additional.
        /// </summary>
        public ushort ARCOUNT
        {
            get
            {
                return GetDataInt(10);
            }
            set
            {
                byte[] data = GetBytes(value);
                SetData(10, data);
            }
        }
        public Flags Flags;
    }
}
