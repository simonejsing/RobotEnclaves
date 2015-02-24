using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Engine.Exceptions
{
    public class InvalidRobotPropertyException : RobotException
    {
        public InvalidRobotPropertyException()
        {
        }

        public InvalidRobotPropertyException(string property)
            : base(string.Format("Unknown property '{0}'", property))
        {
        }
    }
}
