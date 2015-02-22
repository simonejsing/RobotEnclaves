using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Engine.Exceptions
{
    public class RobotException : Exception
    {
        public RobotException()
        {
        }

        public RobotException(string message)
            : base(message)
        {
        }
    }
}
