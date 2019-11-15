using System;
using System.Collections.Generic;
using System.Text;

namespace dnc_csharp.Classes
{
    public class Header : Datagram
    {
        public Header(byte[] data) : base(data)
        {
            Flags = new Flags(DataRange(2, 2));
        }
        public byte[] ID
        {
            get
            {
                return DataRange(0);
            }
        }
        /// <summary>
        /// Number of questions.
        /// </summary>
        public int QDCOUNT
        {
            get
            {
                return GetDataInt(4);
            }
        }
        /// <summary>
        /// Number of answers.
        /// </summary>
        public int ANCOUNT
        {
            get
            {
                return GetDataInt(6);
            }
        }
        /// <summary>
        /// Number of authority.
        /// </summary>
        public int NSCOUNT
        {
            get
            {
                return GetDataInt(8);
            }
        }
        /// <summary>
        /// Number of additional.
        /// </summary>
        public int ARCOUNT
        {
            get
            {
                return GetDataInt(10);
            }
        }



        public Flags Flags;
    }
}
