using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Engine.Exceptions
{
    public class CommException : Exception
    {
        public CommException()
        {
        }

        public CommException(string message) : base(message)
        {
        }
    }
}
