using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Engine.Exceptions
{
    class ComputerSyntaxException : RobotException
    {
        public ComputerSyntaxException()
        {
        }

        public ComputerSyntaxException(string message)
            : base(message)
        {
        }
    }
}
