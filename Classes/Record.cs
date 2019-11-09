using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel;

namespace dnc_csharp.Classes
{
    public abstract class Record
    {
        private byte[] Message;

        // Get data from message by bits flags.
        private byte[] GetData(int startBit, int numberOfBits)
        {
            var byteList = new List<byte>();
            int numberOfBytes = numberOfBits / 8;
            int remainOfBits = numberOfBits % 8;

            int i;
            for (i = 0; i < numberOfBytes; i++)
            {
                byteList.Add(Message[i]);
            }
            if (remainOfBits != 0)
            {
                byte mask = (byte)(2 ^ remainOfBits);
                byte lastBits = (byte)(Message[i] & mask);  // i + 1?
                byteList.Add(lastBits);
            }

            return byteList.ToArray();
        }
    }
}
