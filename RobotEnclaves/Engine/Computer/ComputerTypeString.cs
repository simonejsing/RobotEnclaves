using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Engine.Computer
{
    using Engine.Exceptions;

    public sealed class ComputerTypeString : ComputerType
    {
        public string Value { get; private set; }

        public ComputerTypeString()
        {
        }

        public ComputerTypeString(IComputerType ct)
        {
            ComputerTypeString stringType = (ComputerTypeString)ct;
            if(stringType == null)
                throw new ComputerInvalidArgumentException(this, ct);

            Value = stringType.Value;
        }

        public ComputerTypeString(string value)
        {
            this.Value = value;
        }

        public override string TypeName
        {
            get
            {
                return "String";
            }
        }

        public override string ToString()
        {
            return Value;
        }
    }
}
