using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Engine.Computer
{
    using Engine.Exceptions;

    public sealed class ComputerTypeInt : ComputerType
    {
        public int Value { get; private set; }

        public ComputerTypeInt()
        {
        }

        public ComputerTypeInt(IComputerType ct)
        {
            ComputerTypeInt ctInt = (ComputerTypeInt)ct;

            if(ctInt == null)
                throw new ComputerSyntaxException(string.Format("Invalid cast from {0} to {1}", ct.TypeName, this.TypeName));

            Value = ctInt.Value;
        }

        public ComputerTypeInt(int value)
        {
            this.Value = value;
        }

        public override string TypeName
        {
            get
            {
                return "Int";
            }
        }

        public override string ToString()
        {
            return Value.ToString();
        }

        public override IComputerType Cast<T>()
        {
            if (typeof(T) == typeof(ComputerTypeFloat))
                return new ComputerTypeFloat(Value);

            throw new ComputerInvalidCastException(this, new T());
        }
    }
}
