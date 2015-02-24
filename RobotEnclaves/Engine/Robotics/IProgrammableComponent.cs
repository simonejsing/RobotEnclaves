using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Engine.Robotics
{
    using Engine.Computer;

    public interface IProgrammableComponent
    {
        string Name { get; }
        KeyValuePair<string, Func<ComputerType, ComputerType>>[] Methods { get; }

        IComputerType EvaluatePropertyInstruction(string instruction);
        IComputerType EvaluateMethodInvocation(string instruction);
    }
}
