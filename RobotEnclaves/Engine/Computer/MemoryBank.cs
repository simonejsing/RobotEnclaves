using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Engine.Computer
{
    public class MemoryBank : IMemoryBank
    {
        private readonly byte[] storage;

        public MemoryBank(int size)
        {
            this.storage = new byte[size];
        }

        public int SizeMB
        {
            get
            {
                return this.storage.Length / 1024;
            }
        }

        public void Set(int address, byte value)
        {
            this.storage[address] = value;
        }

        public byte GetByte(int address)
        {
            return this.storage[address];
        }
    }
}
