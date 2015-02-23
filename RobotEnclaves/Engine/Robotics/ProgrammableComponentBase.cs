using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Engine.Robotics
{
    using Engine.Exceptions;

    public abstract class ProgrammableComponentBase : IProgrammableComponent
    {
        public abstract string Name { get; }

        private readonly Dictionary<string,float> propertyValues = new Dictionary<string, float>();

        public float this[string propertyName]
        {
            get
            {
                return GetProperty(propertyName);
            }
            set
            {
                SetProperty(propertyName, value);
            }
        }

        public object EvaluatePropertyInstruction(string instruction)
        {
            var propertyTokens = instruction.Split(new char[] { '=' }, 2);
            if (propertyTokens.Length > 1)
            {
                var propertyName = propertyTokens[0].Trim();
                var propertyValue = float.Parse(propertyTokens[1].Trim());

                this[propertyName] = propertyValue;
            }

            return null;
        }

        public object EvaluateMethodInvocation(string instruction)
        {
            var methodTokens = instruction.Split(new char[] { '(' }, 2);
            if (methodTokens.Length > 1)
            {
                var methodName = methodTokens[0].Trim();
                var arguments = methodTokens[1].Trim();
                arguments = arguments.Substring(0, arguments.Length - 1);

                var method = Methods.FirstOrDefault(m => m.Key.Equals(methodName, StringComparison.OrdinalIgnoreCase));
                return method.Value(arguments.Split(','));
            }

            throw new InvalidRobotMethodException();
        }

        public virtual string[] Properties
        {
            get
            {
                return new string[0];
            }
        }

        public virtual KeyValuePair<string, Func<string[], object>>[] Methods
        {
            get
            {
                return new KeyValuePair<string, Func<string[], object>>[0];
            }
        }

        private float GetProperty(string propertyName)
        {
            float value;
            this.AssertPropertyExists(propertyName);
            return this.propertyValues.TryGetValue(propertyName, out value) ? value : 0.0f;
        }

        private void SetProperty(string propertyName, float value)
        {
            this.AssertPropertyExists(propertyName);
            if (this.propertyValues.ContainsKey(propertyName))
            {
                this.propertyValues[propertyName] = value;
            }
            else
            {
                this.propertyValues.Add(propertyName, value);
            }
        }

        private void AssertPropertyExists(string propertyName)
        {
            if (!Properties.Contains(propertyName))
            {
                throw new ComponentPropertyDoesNotExistException(propertyName);
            }
        }
    }
}
