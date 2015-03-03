using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Engine.Computer
{
    using Engine.Exceptions;

    public sealed class ComputerTypeBoolean : ComputerType
    {
        public bool Value { get; private set; }

        public ComputerTypeBoolean()
        {
        }

        public ComputerTypeBoolean(bool value)
        {
            this.Value = value;
        }

        public override string TypeName
        {
            get
            {
                return "Boolean";
            }
        }

        public override string ToString()
        {
            return Value.ToString();
        }

        public override IComputerType Cast<T>()
        {
            if(typeof(T) == typeof(ComputerTypeInt))
                return new ComputerTypeInt(Value ? 0 : 1);

            throw new ComputerInvalidCastException(this, new T());
        }
    }
}
