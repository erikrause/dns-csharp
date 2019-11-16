using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace dnc_csharp.Classes
{
    public class Answers// : Data
    {
        public Answers(byte[] data, int numberOfRecords)// : base(data, numberOfRecords)
        {

        }

        public List<IRecord> Records
        {
            get
            {
                //return (List<ResourceRecord>)_records;
                //List<ResourceRecord> prob = _records.Cast<ResourceRecord>().ToList();
                //return prob;
                return null;
            }
            set => throw new NotImplementedException();
        }
    }
}
