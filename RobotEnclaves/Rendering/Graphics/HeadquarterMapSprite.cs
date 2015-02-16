using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rendering.Graphics
{
    using VectorMath;

    class HeadquarterMapSprite : ObjectMapSprite
    {
        public HeadquarterMapSprite(Vector2 objectPosition)
            : base(objectPosition)
        {
        }

        public override void Render(IRenderEngine renderEngine)
        {
            renderEngine.DrawCircle(ObjectPosition, 10f, Color.Blue);
        }
    }
}
