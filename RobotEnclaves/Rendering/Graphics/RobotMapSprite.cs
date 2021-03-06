﻿using System;
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

        public RobotMapSprite(RobotObject robot) : base(robot.Position)
        {
            Robot = robot;
        }

        public override void Render(IRenderEngine renderEngine)
        {
            renderEngine.DrawTexture("RepairBot", Robot.Position, new Vector2(40, 40), Robot.Direction.Angle, TextureDrawMode.Centered);
        }
    }
}
