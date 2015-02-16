using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rendering
{
    using Engine.World;
    using Rendering.Graphics;
    using VectorMath;

    public class WorldRenderer
    {
        private IRenderEngine RenderEngine;
        private Map Map;
        private List<IGraphics> Graphics = new List<IGraphics>(); 

        public WorldRenderer(IRenderEngine renderEngine, World world)
        {
            RenderEngine = renderEngine;
            float center = RenderEngine.Viewport.X/2;
            float size = RenderEngine.Viewport.Y - 100;
            Map = new Map(new Vector2(center - size/2, 50f), new Vector2(size, size));

            Graphics.AddRange(world.GetObjects().Select(GraphicsFactory.CreateFromObject));
        }

        public void Render()
        {
            RenderEngine.Clear();

            // Renders a map widget
            Map.Render(RenderEngine, Graphics);
        }
    }
}
