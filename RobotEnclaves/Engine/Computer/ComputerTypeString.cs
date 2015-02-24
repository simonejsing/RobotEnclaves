using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Engine.Computer
{
    public class ComputerTypeString : ComputerType
    {
        public string Value { get; private set; }

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
