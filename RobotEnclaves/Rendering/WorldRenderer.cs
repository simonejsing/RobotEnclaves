using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rendering
{
    using Engine.World;
    using VectorMath;

    public class WorldRenderer
    {
        private IRenderEngine RenderEngine;
        
        public WorldRenderer(IRenderEngine renderEngine)
        {
            RenderEngine = renderEngine;
        }

        public void Render(World world)
        {
            RenderEngine.Clear();

            foreach (var obj in world.GetObjects())
            {
                RenderEngine.DrawCircle(obj.Position, 2f, Color.Red);
            }
        }
    }
}
