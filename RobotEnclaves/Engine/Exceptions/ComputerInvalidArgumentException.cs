using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Engine.Exceptions
{
    using Engine.Computer;

    public class ComputerInvalidArgumentException : ComputerException
    {
        public ComputerInvalidArgumentException(IComputerType expected, IComputerType actual) 
            : this(string.Format("Invalid argument type, expected type '{0}' actual type was '{1}'", expected.TypeName, actual.TypeName))
        {
        }

        private ComputerInvalidArgumentException(string message)
            : base(message)
        {
        }
    }
}
