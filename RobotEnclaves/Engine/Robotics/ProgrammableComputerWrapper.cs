using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Engine.Computer;

namespace Engine.Robotics
{
    public class ProgrammableComputerWrapper : IProgrammableComponent
    {
        private readonly IComputer WrappedComputer;

        public string Name { get; private set; }

        public ProgrammableComputerWrapper(string name, IComputer computer)
        {
            Name = name;
            WrappedComputer = computer;
            Errors = new List<string>();
        }

        public IComputerType EvaluateInstruction(string instruction)
        {
            return WrappedComputer.EvaluateInstruction(instruction);
        }

        public IEnumerable<string> Errors { get; private set; }
    }
}
