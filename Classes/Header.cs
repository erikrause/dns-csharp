using System;
using System.Collections.Generic;
using System.Text;

namespace dnc_csharp.Classes
{
    public class Header : Request
    {
        public Header(byte[] data) : base(data)
        {
            Flags = new Flags(DataRange(16, 16));
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
                return GetDataInt(32);
            }
        }
        /// <summary>
        /// Number of answers.
        /// </summary>
        public int ANCOUNT
        {
            get
            {
                return GetDataInt(48);
            }
        }
        /// <summary>
        /// Number of authority.
        /// </summary>
        public int NSCOUNT
        {
            get
            {
                return GetDataInt(64);
            }
        }
        /// <summary>
        /// Number of additional.
        /// </summary>
        public int ARCOUNT
        {
            get
            {
                return GetDataInt(48);
            }
        }



        public Flags Flags;
    }
}
