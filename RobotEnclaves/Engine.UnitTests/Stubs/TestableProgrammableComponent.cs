using Engine.Robotics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Engine.UnitTests.Stubs
{
    internal class TestableProgrammableComponent : ProgrammableComponentBase
    {
        public void AddProperty(IProgrammableProperty property)
        {
            this.RegisterProperty(property);
        }

        public void AddMethod(IProgrammableMethod method)
        {
            this.RegisterMethod(method);
        }

        public override string Name
        {
            get
            {
                return "testable";
            }
            protected set
            {
                throw new NotImplementedException();
            }
        }
    }
}
