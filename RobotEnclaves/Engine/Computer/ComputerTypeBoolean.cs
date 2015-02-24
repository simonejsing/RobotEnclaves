using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Engine.Computer
{
    public class ComputerTypeBoolean : ComputerType
    {
        public bool Value { get; private set; }

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
    }
}
