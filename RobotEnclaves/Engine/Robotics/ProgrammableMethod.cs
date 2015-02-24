using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Engine.Robotics
{
    using Engine.Computer;

    public class ProgrammableMethod : IProgrammableMethod
    {
        private readonly Func<IComputerType, IComputerType> method;

        public string Name { get; private set; }

        public ProgrammableMethod(string name, Func<IComputerType, IComputerType> method)
        {
            this.Name = name;
            this.method = method;
        }

        public IComputerType Invoke(IComputerType arguments)
        {
            return method(arguments);
        }
    }
}
