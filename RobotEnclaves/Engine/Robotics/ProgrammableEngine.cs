using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Engine.Robotics
{
    using Engine.Computer;
    using Engine.Exceptions;

    public class ProgrammableEngine : ProgrammableComponentBase
    {
        const float MaxSpeed = 80.0f / 3.6f;

        public ProgrammableEngine()
        {
            var throttleProperty = new ProgrammableProperty<ComputerTypeFloat>(
                "throttle",
                () => new ComputerTypeFloat(this.Throttle),
                ct => { this.Throttle = ct.Value; });
            var speedProperty = new ProgrammableProperty<ComputerTypeFloat>(
                "speed",
                () => new ComputerTypeFloat(this.Speed));

            this.RegisterProperty(throttleProperty);
            this.RegisterProperty(speedProperty);

            Throttle = 0f;
        }

        public float Speed
        {
            get
            {
                return Throttle*MaxSpeed;
            }
        }

        private float throttle;
        public float Throttle
        {
            get
            {
                return throttle;
            }
            set
            {
                if (value > 1.0f)
                {
                    throw new RobotException(string.Format("Throttle level cannot exceed 1.0, attempt to set to {0}", value));
                }

                throttle = value;
            }
        }

        public override string Name
        {
            get
            {
                return "engine";
            }
            protected set
            {
            }
        }
    }
}
