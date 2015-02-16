using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rendering
{
    using Engine.World;
    using VectorMath;

    class Map
    {
        private Vector2 Position;
        private Vector2 Size;

        public Map(Vector2 position, Vector2 size)
        {
            Position = position;
            Size = size;
        }

        public void Render(IRenderEngine renderEngine, IEnumerable<IGraphics> graphics)
        {
            renderEngine.Translate(Position);
            renderEngine.FillRectangle(Vector2.Zero, Size - Position, Color.Black);

            // Center (0,0)
            renderEngine.Translate(Size / 2);

            foreach (var graphic in graphics)
            {
                graphic.Render(renderEngine);
            }

            renderEngine.ResetTransformation();
        }
    }
}
