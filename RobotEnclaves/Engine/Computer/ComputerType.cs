using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Engine.Computer
{
    using System.Globalization;
    using Engine.Exceptions;

    public class ComputerType : IComputerType
    {
        public ComputerType()
        {
        }

        public virtual string TypeName
        {
            get
            {
                return "";
            }
        }

        public override string ToString()
        {
            return "";
        }

        public virtual IComputerType Cast<T>() where T : IComputerType, new()
        {
            throw new ComputerInvalidCastException(this, new T());
        }

        public static ComputerType Parse(string value)
        {
            ComputerType methodArgument;

            if (string.IsNullOrEmpty(value))
            {
                methodArgument = new ComputerTypeVoid();
            }
            else
            {
                var argumentList = value.Split(',');
                if (argumentList.Length == 1)
                {
                    methodArgument = ParseSingleComputerType(argumentList[0].Trim());
                }
                else
                {
                    methodArgument = new ComputerTypeList(argumentList.Select(a => ComputerType.ParseSingleComputerType(a.Trim())));
                }
            }

            return methodArgument;
        }

        private static ComputerType ParseSingleComputerType(string value)
        {
            if (value.StartsWith("\"") && value.EndsWith("\""))
            {
                return new ComputerTypeString(value.Substring(1, value.Length - 2));
            }

            if (value.Equals("true", StringComparison.OrdinalIgnoreCase))
            {
                return new ComputerTypeBoolean(true);
            }
            
            if (value.Equals("false", StringComparison.OrdinalIgnoreCase))
            {
                return new ComputerTypeBoolean(false);
            }

            int intValue;
            if (int.TryParse(value, NumberStyles.Integer, CultureInfo.CurrentCulture, out intValue))
            {
                return new ComputerTypeInt(intValue);
            }

            float floatValue;
            if (float.TryParse(value, NumberStyles.Float, CultureInfo.CurrentCulture, out floatValue))
            {
                return new ComputerTypeFloat(floatValue);
            }

            throw new ComputerSyntaxException(String.Format("Syntax error, cannot parse type '{0}'", value));
        }
    }
}
