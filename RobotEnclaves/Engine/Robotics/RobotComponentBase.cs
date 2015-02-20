using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Engine.Robotics
{
    using Engine.Exceptions;

    public abstract class RobotComponentBase : IRobotComponent
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

        public abstract string[] Properties { get; }

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
