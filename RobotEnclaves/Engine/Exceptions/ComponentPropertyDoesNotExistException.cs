using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Engine.Exceptions
{
    public class ComponentPropertyDoesNotExistException : Exception
    {
        public ComponentPropertyDoesNotExistException()
        {
        }

        public ComponentPropertyDoesNotExistException(string message)
            : base(message)
        {
        }
    }
}
