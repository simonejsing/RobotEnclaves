using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Engine.Computer
{
    public interface IMemoryBank
    {
        int SizeMB { get; }

        void Set(int address, byte value);

        byte GetByte(int address);
    }
}
