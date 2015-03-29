using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Engine.Computer;

namespace Engine.Robotics
{
    public class NullProgrammableComponent : IProgrammableComponent
    {
        public string Name
        {
            get
            {
                return "null";
            }
        }

        public IComputerType EvaluateInstruction(string instruction)
        {
            return new ComputerTypeVoid();
        }

        public IEnumerable<string> Errors
        {
            get
            {
                return Enumerable.Empty<string>();
            }
        }
    }
}
