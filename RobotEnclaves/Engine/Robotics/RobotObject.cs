using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Engine.Robotics
{
    using VectorMath;

    public class RobotObject : IObject
    {
        private readonly Robot robot;

        public Vector2 Position { get; set; }
        public UnitVector2 Direction { get; set; }

        public IObjectHealth ObjectHealth { get; private set; }

        public float Mass
        {
            get
            {
                return robot.BaseMass + robot.Hull.CargoBay.TotalMass;
            }
        }

        public RobotObject(Robot robot)
        {
            this.ObjectHealth = new ObjectHealth(100);
            this.Position = Vector2.Zero;
            this.Direction = UnitVector2.GetInstance(1, 0);
            this.robot = robot;
        }
    }
}
