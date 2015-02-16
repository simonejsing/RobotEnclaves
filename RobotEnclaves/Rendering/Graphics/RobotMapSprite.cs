using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rendering.Graphics
{
    using Engine.World;
    using VectorMath;

    class RobotMapSprite : ObjectMapSprite
    {
        public RobotMapSprite(Vector2 objectPosition)
            : base(objectPosition)
        {
        }

        public override void Render(IRenderEngine renderEngine)
        {
            renderEngine.DrawCircle(ObjectPosition, 2f, Color.Red);
        }
    }
}
