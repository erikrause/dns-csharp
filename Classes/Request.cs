using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel;
using System.Linq;

namespace dnc_csharp.Classes
{
    public abstract class Request
    {
        public readonly byte[] Data;

        public Request(byte[] data)
        {
            Data = data;
        }

        public int IndexOf(byte[] data, int value, int startByte = 0)
        {
            int i = startByte;
            byte byteData;
            i--;
            
            do
            {
                i++;
                byteData = data[i];
            } while (byteData != value);

            return i;
        }
        // Get data from message by bits flags.
        protected byte[] DataRange(int startBit, int numberOfBits = 16)
        {
            var byteList = new List<byte>();
            int numberOfBytes = numberOfBits / 8;
            int remainOfBits = numberOfBits % 8;

            int i;
            for (i = startBit; i < numberOfBytes*8 + startBit; i=i+8)
            {
                byteList.Add(Data[i/8]);
            }
            if (remainOfBits != 0)
            {
                byte mask = (byte)(2 ^ remainOfBits);
                byte lastBits = (byte)(Data[i/8] & mask);  // i + 1?
                byteList.Add(lastBits);
            }

            return byteList.ToArray();
        }

        protected string GetDataString(int startBit, int numberOfBits = 16)
        {
            byte[] byteArray = DataRange(startBit, numberOfBits);
            return ToString(byteArray);
        }

        protected int GetDataInt(int startBit, int numberOfBits = 16)
        {
            byte[] byteArray = DataRange(startBit, numberOfBits);
            Array.Reverse(byteArray);
            return BitConverter.ToInt16(byteArray);
        }

        public static byte[] ToByteArray(string hex)
        {
            hex = hex.Replace(" ", "").Replace("\n", "");
            return Enumerable.Range(0, hex.Length)
                             .Where(x => x % 2 == 0)
                             .Select(x => Convert.ToByte(hex.Substring(x, 2), 16))
                             .ToArray();
        }

        public static string ToString(byte[] bytes, int count = -1)
        {
            if (count == -1)
            {
                count = bytes.Length;
            }

            string result = BitConverter.ToString(bytes.Take(count).ToArray());

            return result.Replace("-", "");
        }
    }
}
