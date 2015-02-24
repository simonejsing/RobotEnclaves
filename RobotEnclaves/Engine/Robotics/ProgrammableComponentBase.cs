using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Engine.Robotics
{
    using Engine.Computer;
    using Engine.Exceptions;

    public abstract class ProgrammableComponentBase : IProgrammableComponent
    {
        public abstract string Name { get; }

        private readonly List<IProgrammableProperty> properties = new List<IProgrammableProperty>();

        public void RegisterProperty(IProgrammableProperty property)
        {
            properties.Add(property);
        }

        public IComputerType EvaluatePropertyInstruction(string instruction)
        {
            var propertyTokens = instruction.Split(new char[] { '=' }, 2);
            var property = this.FindProperty(propertyTokens[0].Trim());

            // Get property
            if (propertyTokens.Length == 1)
            {
                return property.Get();
            }

            // Set property
            if (propertyTokens.Length > 1)
            {
                SetPropertyValue(property, propertyTokens);
                return new ComputerTypeVoid();
            }

            throw new InvalidRobotPropertyException();
        }

        private static void SetPropertyValue(IProgrammableProperty property, string[] propertyTokens)
        {
            if (property.IsReadOnly)
            {
                throw new SettingReadOnlyPropertyException(property.Name);
            }

            property.Set(ComputerType.Parse(propertyTokens[1].Trim()));
        }

        private IProgrammableProperty FindProperty(string name)
        {
            var property = properties.FirstOrDefault(p => p.Name.Equals(name, StringComparison.OrdinalIgnoreCase));
            if (property == null)
            {
                throw new PropertyDoesNotExistException(name);
            }

            return property;
        }

        public IComputerType EvaluateMethodInvocation(string instruction)
        {
            var methodTokens = instruction.Split(new char[] { '(' }, 2);
            if (methodTokens.Length > 1)
            {
                var methodName = methodTokens[0].Trim();
                var method = Methods.FirstOrDefault(m => m.Key.Equals(methodName, StringComparison.OrdinalIgnoreCase));

                var arguments = methodTokens[1].Trim();
                arguments = arguments.Substring(0, arguments.Length - 1);

                ComputerType methodArgument;
                if (string.IsNullOrEmpty(arguments))
                {
                    methodArgument = new ComputerTypeVoid();
                }
                else
                {
                    var argumentList = arguments.Split(',');
                    if (argumentList.Length == 1)
                    {
                        methodArgument = ComputerType.Parse(argumentList[0]);
                    }
                    else
                    {
                        methodArgument = new ComputerTypeList(argumentList.Select(ComputerType.Parse).ToArray());
                    }
                }

                return method.Value(methodArgument);
            }

            throw new InvalidRobotMethodException();
        }

        public virtual KeyValuePair<string, Func<ComputerType, ComputerType>>[] Methods
        {
            get
            {
                return new KeyValuePair<string, Func<ComputerType, ComputerType>>[0];
            }
        }
    }
}
