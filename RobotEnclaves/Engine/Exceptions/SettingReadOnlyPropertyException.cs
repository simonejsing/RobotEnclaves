using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Engine.Exceptions
{
    public class SettingReadOnlyPropertyException : RobotException
    {
        public SettingReadOnlyPropertyException()
        {
        }

        public SettingReadOnlyPropertyException(string message)
            : base(message)
        {
        }
    }
}
