﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rendering
{
    using Engine;
    using Engine.World;
    using Rendering.Graphics;

    static class GraphicsFactory
    {
        public static IGraphics CreateFromObject(IObject obj)
        {
            if (obj is Robot)
                return new RobotMapSprite(obj.Position);
            if (obj is Headquarter)
                return new HeadquarterMapSprite(obj.Position);

            throw new Exception("Unknown object type");
        }
    }
}