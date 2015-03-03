using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Engine.Computer
{
    using Engine.Exceptions;

    public sealed class ComputerTypeVoid : ComputerType
    {
        public override string TypeName
        {
            get
            {
                return "Void";
            }
        }

        public ComputerTypeVoid()
        {
        }

        public override string ToString()
        {
            return null;
        }
    }
}
