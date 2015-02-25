using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rendering.Graphics
{
    using Common;
    using Engine;
    using Engine.Robotics;
    using VectorMath;

    class RobotMapSprite : ObjectMapSprite
    {
        private readonly Robot Robot;

        const float Radius = 10f;

        public RobotMapSprite(Robot robot) : base(robot.Position)
        {
            Robot = robot;
        }

        public override void Render(IRenderEngine renderEngine)
        {
            renderEngine.DrawCircle(Robot.Position, Radius, Color.Red);
        }
    }
}
