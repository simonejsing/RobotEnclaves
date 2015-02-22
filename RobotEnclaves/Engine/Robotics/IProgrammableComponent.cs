using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Engine.Robotics
{
    public interface IProgrammableComponent
    {
        string Name { get; }
        string[] Properties { get; }
        KeyValuePair<string, Func<string[], object>>[] Methods { get; }

        float this[string propertyName] { get; set; }

        object EvaluatePropertyInstruction(string instruction);
        object EvaluateMethodInvocation(string instruction);
    }
}
