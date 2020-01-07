namespace dns_csharp.Classes
{
    public class ResourceRecord : Record
    {
        public ResourceRecord(byte[] data) : base(data)
        {

        }
        /// <summary>
        /// Возвращает смещение битов, указывающее на домен в запросе.
        /// </summary>
        public override string NAME
        {
            get
            {
                int nameField = GetDataInt(0, NameEnd);
                int nameShift = nameField - (0b11 << 14);

                return nameShift.ToString();
            }
        }

        public int TTL
        {
            get
            {
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
