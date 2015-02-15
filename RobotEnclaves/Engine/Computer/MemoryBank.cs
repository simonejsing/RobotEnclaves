using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Engine.Computer
{
    public class MemoryBank : IMemoryBank
    {
        private readonly byte[] Storage;

        public MemoryBank(int size)
        {
            Storage = new byte[size];
        }

        public void Set(int address, byte value)
        {
            Storage[address] = value;
        }

        public byte GetByte(int address)
        {
            return Storage[address];
        }
    }
}
