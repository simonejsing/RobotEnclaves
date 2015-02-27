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
        private readonly RobotObject Robot;

        const float Radius = 10f;

        public RobotMapSprite(RobotObject robot) : base(robot.Position)
        {
            Robot = robot;
        }

        public override void Render(IRenderEngine renderEngine)
        {
            renderEngine.DrawCircle(Robot.Position, Radius, Color.Red, 5f);
            renderEngine.DrawVector(Robot.Position, Robot.Direction * 20, Color.Red, 3f);
        }
    }
}
