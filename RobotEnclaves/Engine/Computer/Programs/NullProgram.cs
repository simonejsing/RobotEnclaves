using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Engine.Computer.Programs
{
    class NullProgram : IProgram
    {
        public string Name
        {
            get
            {
                return "null";
            }
        }

        public bool Finished
        {
            get
            {
                return true;
            }
        }

        public void Execute(IComputerType arguments)
        {
        }

        public IStatement GetNextStatement()
        {
            return new GenericStatement(ct => { });
        }
    }
}
