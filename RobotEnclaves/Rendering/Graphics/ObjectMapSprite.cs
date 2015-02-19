using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rendering.Graphics
{
    using Engine;
    using VectorMath;

    abstract class ObjectMapSprite : IGraphics
    {
        protected readonly Vector2 ObjectPosition;

        protected ObjectMapSprite(Vector2 objectPosition)
        {
            ObjectPosition = objectPosition;
        }

        public abstract void Render(IRenderEngine renderEngine);
    }
}
