using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rendering
{
    using Engine.World;
    using VectorMath;

    public class Map
    {
        const float GridSpacing = 100.0f;

        private Vector2 CanvasPosition;
        private Vector2 CanvasSize;
        private float ZoomFactor = 1.0f;

        public Map(Vector2 canvasPosition, Vector2 canvasSize)
        {
            this.CanvasPosition = canvasPosition;
            this.CanvasSize = canvasSize;
        }

        public void Render(IRenderEngine renderEngine, IEnumerable<IGraphics> graphics)
        {
            renderEngine.Translate(this.CanvasPosition);
            renderEngine.FillRectangle(Vector2.Zero, this.CanvasSize - this.CanvasPosition, Color.Sand);

            // Center (0,0)
            renderEngine.Translate(this.CanvasSize / 2);
            renderEngine.Scale(ZoomFactor);

            this.RenderGridLines(renderEngine);

            foreach (var graphic in graphics)
            {
                graphic.Render(renderEngine);
            }

            renderEngine.ResetTransformation();
        }

        private void RenderGridLines(IRenderEngine renderEngine)
        {
            Vector2 mapSize = CanvasSize / this.ZoomFactor;

            DrawVerticalGridLine(renderEngine, 0, mapSize.Y);
            DrawHorizontalGridLine(renderEngine, 0, mapSize.X);
            for (float x = GridSpacing; x < mapSize.X / 2.0f; x += GridSpacing)
            {
                DrawVerticalGridLine(renderEngine, x, mapSize.Y);
            }
            for (float x = -GridSpacing; x > -mapSize.X / 2.0f; x -= GridSpacing)
            {
                DrawVerticalGridLine(renderEngine, x, mapSize.Y);
            }
            for (float y = GridSpacing; y < mapSize.Y / 2.0f; y += GridSpacing)
            {
                DrawHorizontalGridLine(renderEngine, y, mapSize.X);
            }
            for (float y = -GridSpacing; y > -mapSize.Y / 2.0f; y -= GridSpacing)
            {
                DrawHorizontalGridLine(renderEngine, y, mapSize.X);
            }
        }

        private static void DrawVerticalGridLine(IRenderEngine renderEngine, float x, float height)
        {
            Vector2 start = new Vector2(x, -height / 2.0f);
            Vector2 vector = new Vector2(0.0f, height);
            renderEngine.DrawVector(start, vector, Color.Gray);
        }

        private static void DrawHorizontalGridLine(IRenderEngine renderEngine, float y, float width)
        {
            Vector2 start = new Vector2(-width / 2.0f, y);
            Vector2 vector = new Vector2(width, 0.0f);
            renderEngine.DrawVector(start, vector, Color.Gray);
        }
    }
}
