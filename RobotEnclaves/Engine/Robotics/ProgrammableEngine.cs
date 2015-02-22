using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Engine.Robotics
{
    public class ProgrammableEngine : ProgrammableComponentBase
    {
        const float MaxSpeed = 80.0f / 3.6f;
        const string ThrottlePropertyName = "throttle";

        public float Speed
        {
            get
            {
                return Throttle*MaxSpeed;
            }
        }

        public float Throttle
        {
            get
            {
                return this[ThrottlePropertyName];
            }
            set
            {
                this[ThrottlePropertyName] = value;
            }
        }

        public override string Name
        {
            get
            {
                return "engine";
            }
        }

        public override string[] Properties
        {
            get
            {
                return new[] { ThrottlePropertyName };
            }
        }

        public override KeyValuePair<string, Func<string[], object>>[] Methods
        {
            get
            {
                return new KeyValuePair<string, Func<string[], object>>[0];
            }
        }
    }
}
