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
        public abstract string Name { get; protected set; }

        private readonly List<IProgrammableProperty> properties = new List<IProgrammableProperty>();
        private readonly List<IProgrammableMethod> methods = new List<IProgrammableMethod>();

        public IEnumerable<IProgrammableProperty> Properties
        {
            get
            {
                return properties;
            }
        }

        public IEnumerable<IProgrammableMethod> Methods
        {
            get
            {
                return methods;
            }
        }

        protected ProgrammableComponentBase()
        {
            this.RegisterMethod(new ProgrammableMethod("properties", ct => ListProperties()));
        }

        private IComputerType ListProperties()
        {
            return new ComputerTypeList(Properties.Select(p => new ComputerTypeString(p.Name)));
        }

        protected void RegisterProperty(IProgrammableProperty property)
        {
            properties.Add(property);
        }

        protected void RegisterMethod(IProgrammableMethod method)
        {
            methods.Add(method);
        }

        public virtual IComputerType EvaluateInstruction(string instruction)
        {
            if (instruction.EndsWith(")"))
            {
                return EvaluateMethodInvocation(instruction);
            }

            return EvaluatePropertyInstruction(instruction);
        }

        protected IComputerType EvaluatePropertyInstruction(string instruction)
        {
            var propertyTokens = instruction.Split(new char[] { '=' }, 2);
            var propertyName = propertyTokens[0].Trim();
            var property = this.FindProperty(propertyName);

            // Get property
            if (propertyTokens.Length == 1)
            {
                return property.Get();
            }

            // Set property
            SetPropertyValue(property, propertyTokens);
            return new ComputerTypeVoid();
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
                throw new InvalidRobotPropertyException(name);
            }

            return property;
        }

        protected IComputerType EvaluateMethodInvocation(string instruction)
        {
            var methodTokens = instruction.Split(new char[] { '(' }, 2);
            if (methodTokens.Length > 1)
            {
                var methodName = methodTokens[0].Trim();
                var method = methods.FirstOrDefault(m => m.Name.Equals(methodName, StringComparison.OrdinalIgnoreCase));

                if (method == null)
                {
                    throw new InvalidRobotMethodException(methodName);
                }

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
                        methodArgument = new ComputerTypeList(argumentList.Select(ComputerType.Parse));
                    }
                }

                return method.Invoke(methodArgument);
            }

            throw new InvalidRobotMethodException();
        }

    }
}
