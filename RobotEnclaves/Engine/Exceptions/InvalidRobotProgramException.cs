using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Engine.Exceptions
{
    class InvalidRobotProgramException : RobotException
    {
        public InvalidRobotProgramException(string programName)
            : base(string.Format("Invalid program name '{0}'.", programName))
        {
        }
    }
}
