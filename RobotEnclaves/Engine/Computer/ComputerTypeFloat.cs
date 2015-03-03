using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Engine.Computer
{
    public sealed class ComputerTypeFloat : ComputerType
    {
        public float Value { get; private set; }

        public ComputerTypeFloat()
        {
        }

        public ComputerTypeFloat(float value)
        {
            this.Value = value;
        }

        public override string TypeName
        {
            get
            {
                return "Float";
            }
        }

        public override string ToString()
        {
            return Value.ToString();
        }
    }
}
