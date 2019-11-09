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
        protected byte[] DataRange(int startByte = 0, int numberOfBytes = 2)
        {
            var byteList = new List<byte>();

            int i;
            for (i = startByte; i < numberOfBytes + startByte; i++)
            {
                byteList.Add(Data[i]);
            }

            return byteList.ToArray();
        }

        protected string GetDataString(int startByte = 0, int numberOfBytes = 2)
        {
            byte[] byteArray = DataRange(startByte, numberOfBytes);
            return ToString(byteArray);
        }

        protected int GetDataInt(int startByte = 0, int numberOfBytes = 2)
        {
            byte[] byteArray = DataRange(startByte, numberOfBytes);
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
