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

        void Set(int address, IComputerType value);

        IComputerType GetByte(int address);

        void Upgrade(int extraMb);
    }
}
