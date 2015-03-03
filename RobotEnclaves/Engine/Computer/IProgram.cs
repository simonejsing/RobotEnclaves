using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Engine.Computer
{
    public interface IProgram
    {
        string Name { get; }
        bool Finished { get; }

        void Execute(IComputerType arguments);

        IStatement GetNextStatement();
    }
}
