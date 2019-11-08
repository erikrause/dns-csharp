using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Collections.Generic;

namespace dnc_csharp.Classes
{
    public abstract class Record : INotifyPropertyChanged
    {
        private byte[] Message;

        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged([CallerMemberName] String propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));      // Упрощенный делегат (?.Invoke вместо условия if).
        }

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
