using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Engine.Exceptions
{
    public class ComputerException : RobotException
    {
        public ComputerException()
        {
        }

        public ComputerException(string message)
            : base(message)
        {
        }
    }
}
