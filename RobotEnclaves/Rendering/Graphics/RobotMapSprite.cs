using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rendering.Graphics
{
    using Engine;
    using Engine.Robotics;
    using Engine.World;
    using VectorMath;

    class RobotMapSprite : IGraphics
    {
        private Robot Robot;

        const float Radius = 10f;

        public RobotMapSprite(Robot robot)
        {
            Robot = robot;
        }

        public void Render(IRenderEngine renderEngine)
        {
            renderEngine.DrawCircle(Robot.Position, Radius, Color.Red);
        }
    }
}
