using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Engine.Exceptions
{
    using Engine.Computer;

    public class ComputerInvalidCastException : ComputerException
    {
        public ComputerInvalidCastException(IComputerType from, IComputerType to) 
            : this(string.Format("Invalid cast from type '{0}' to '{1}'.", from.TypeName, to.TypeName))
        {
        }

        private ComputerInvalidCastException(string message)
            : base(message)
        {
        }
    }
}
