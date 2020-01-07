using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace dns_csharp.Classes
{
    public class Query : Record
    {
        public Query(string domainName, string domainType) : this(domainName, (ushort)Types[domainType])
        {
        }
        public Query(string domainName, ushort domainType) : base()
        {
            List<byte> data = new List<byte>();
            List<byte> nameInBytes = new List<byte>(NameToBytes(domainName));
            data.AddRange(nameInBytes);

            // Data pre-initialization (without TYPE and CLASS) for NameEnd getter.
            Data = new byte[data.Count + 4];
            data.CopyTo(Data);

            TYPE = domainType;
            CLASS = 1;
        }
        public Query(byte[] data) : base(data)
        {
        }
        protected byte[] NameToBytes(string name)
        {

            int startIndex = 0;
            List<byte> bytes = new List<byte>();
            while (startIndex < name.Length)
            {
                string domain = new string(name.Skip(startIndex).TakeWhile(ch => ch != '.').ToArray());
                bytes.Add((byte)domain.Count());
                byte[] domainBytes = ToBytes(domain);
                bytes.AddRange(domainBytes);
                startIndex = bytes.Count;
            }
            bytes.Add(0);   // Add zero byte to end of Name.

            return bytes.ToArray();
        }

        protected override int NameEnd
        {
            get
            {
                return IndexOf(Data, 0, 0) + 1;
            }
        }
        public override string NAME
        {
            get
            {
                string name = "";
                int start = 0;
                int count = 0;

                while (Data[start] != 0)
                {
                    count = Data[start];
                    string domain = ToString(Data.Skip(start + 1).Take(count).ToArray());
                    name += domain + '.';
                    start = start + count + 1;
                }
                name = name.Remove(name.Length - 1);   // Delete last '.' after the loop.

                return name;
            }
            set
            {
                byte[] data = ToBytes(value);
                SetData(NameEnd + 2, data);
            }
        }
    }
}
