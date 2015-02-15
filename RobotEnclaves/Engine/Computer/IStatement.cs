using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Engine.Computer
{
    public interface IStatement
    {
        void Execute(IMemoryBank memory);
    }
}
