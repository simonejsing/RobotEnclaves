using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Engine.UnitTests.Stubs
{
    using Engine.Computer;
    using Robotics;

    internal class TestableRobot : Robot
    {
        private List<IProgrammableComponent> components = new List<IProgrammableComponent>();

        public override IEnumerable<IProgrammableComponent> Components
        {
            get
            {
                return components;
            }
        }

        public TestableRobot(string name)
            : base(name)
        {
        }

        public void AddComponent(IProgrammableComponent component)
        {
            components.Add(component);
        }

        public void AddProperty(IProgrammableProperty property)
        {
            this.RegisterProperty(property);
        }

        public void AddMethod(IProgrammableMethod method)
        {
            this.RegisterMethod(method);
        }

        public void AddProgram(IProgram program)
        {
            this.RegisterProgram(program);
        }
    }
}
