using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Engine.Computer
{
    public interface IComputer
    {
        IMemoryBank MemoryBank { get; }
        IProgram CurrentProgram { get; }

        ComputerType ExecuteStatement(string statement);
    }
}
