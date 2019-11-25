using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel;
using System.Collections.Specialized;
using System.Linq;
using System.Runtime.CompilerServices;

namespace dns_csharp.Classes
{
    public abstract class Datagram : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged([CallerMemberName] String propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        protected byte[] _data;
        public byte[] Data
        {
            get
            {
                return _data;
            }
            protected set
            {
                _data = value;
                OnPropertyChanged();    // not working when changing elements in array. Need to implement IObservable?
            }
        }
        public Datagram()
        {
        }
        public Datagram(byte[] data) : this()
        {
            Data = data;
        }

        public static int IndexOf(byte[] data, int value, int startByte = 0)
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
        protected void SetData(int startByte, byte[] data)
        {
            int i = startByte;
            foreach(byte b in data)
            {
                Data[i] = b;
                i++;
            }
            OnPropertyChanged();        // Need to implement IObservable in Data array instead this code.
        }
        protected string GetDataString(int startByte = 0, int numberOfBytes = 2)
        {
            byte[] byteArray = DataRange(startByte, numberOfBytes);
            return ToString(byteArray);
        }

        protected ushort GetDataInt(int startByte = 0, int numberOfBytes = 2)
        {
            byte[] byteArray = DataRange(startByte, numberOfBytes);
            Array.Reverse(byteArray);
            return (ushort)BitConverter.ToInt16(byteArray, 0);
        }

        protected byte[] ToBytes(ushort data)
        {
            byte[] byteArray = BitConverter.GetBytes(data);
            Array.Reverse(byteArray);
            return byteArray;
        }
        protected byte[] ToBytes(string data)
        {
            byte[] byteArray = Encoding.UTF8.GetBytes(data);    // Check vs BitConverter.
            return byteArray;
        }

        public static byte[] ToByteArray(string hex)
        {
            hex = hex.Replace(" ", "").Replace("\n", "");
            return Enumerable.Range(0, hex.Length)
                             .Where(x => x % 2 == 0)
                             .Select(x => Convert.ToByte(hex.Substring(x, 2), 16))
                             .ToArray();
        }

        public static string ToHex(byte[] bytes, int count = -1)
        {
            if (count == -1)
            {
                count = bytes.Length;
            }

            string result = BitConverter.ToString(bytes.Take(count).ToArray());

            return result.Replace("-", "");
        }

        public static string ToString(byte[] bytes, int count = -1)
        {
            if (count == -1)
            {
                count = bytes.Length;
            }
            return Encoding.UTF8.GetString(bytes.Take(count).ToArray());//.TrimEnd('\0'); ;
        }
    }
}
