using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rendering
{
    using Engine;
    using Engine.Robotics;
    using Engine.Spaceship;
    using Engine.World;
    using Rendering.Graphics;

    static class GraphicsFactory
    {
        public static IGraphics CreateFromObject(IObject obj)
        {
            if (obj is Robot)
                return new RobotMapSprite(obj as Robot);
            if (obj is Spaceship)
                return new SpaceshipMapSprite(obj as Spaceship);

            throw new Exception("Unknown object type");
        }
    }
}
