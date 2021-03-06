﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rendering
{
    using Engine;
    using Engine.Items;
    using Engine.Robotics;
    using Engine.Spaceship;
    using Rendering.Graphics;

    static class GraphicsFactory
    {
        public static IGraphics CreateFromObject(IObject obj)
        {
            if (obj is RobotObject)
                return new RobotMapSprite(obj as RobotObject);
            if(obj is CollectableItem)
                return new CollectableItemSprite(obj as CollectableItem);
            if (obj is Spaceship)
                return new SpaceshipMapSprite(obj as Spaceship);

            throw new Exception("Unknown object type");
        }
    }
}
