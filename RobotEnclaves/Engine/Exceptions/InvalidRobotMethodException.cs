using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Engine.Exceptions
{
    public class InvalidRobotMethodException : RobotException
    {
        public InvalidRobotMethodException()
        {
        }

        public InvalidRobotMethodException(string methodName)
            : base(string.Format("Unknown method '{0}'", methodName))
        {
        }
    }
}
